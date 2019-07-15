using System.Collections;
using System.Collections.Generic;
using Managers;
using Pathfinding;
using Ship;
using UnityEngine;

public class Pirate : AIPath {
    private ShipObject ship;
    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    private GameManager gameManager;

    public GraphMask CurrentGraph { get; set; }
    public Queue<KeyValuePair<Vector3, GraphMask>> QueuedDestination { get; private set; }

    protected override void Start() {
        base.Start();
        gameManager = GameManager.Instance;
        CurrentGraph = gameManager.Graphs[0];
        ship = GameObject.FindGameObjectWithTag("Ship").GetComponent<ShipObject>();
        QueuedDestination = new Queue<KeyValuePair<Vector3, GraphMask>>();
    }

    public override void OnTargetReached() {
        path?.Release(this);
        path = null;
        interpolator.SetPath(null);

        GetComponent<Animator>().SetBool(IsMoving, false);
        if (QueuedDestination.Count > 0) {
            Move(QueuedDestination.Dequeue());
        }
    }

    protected override void OnPathComplete(Path newPath) {
        GetComponent<Animator>().SetBool(IsMoving, true);
        seeker.ReleaseClaimedPath();
        base.OnPathComplete(newPath);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.GetComponentInParent<CannonObject>() != null) {
            var cannon = collision.gameObject.GetComponentInParent<CannonObject>();
            if (cannon.IsRequestedPirate(this) && !cannon.IsBusy) {
                StartCoroutine(WaitDestinationBeforeRotation(cannon.transform.rotation));
                cannon.IsBusy = true;
            }
        } else if (collision.transform.parent.gameObject.GetComponent<Stairs>() != null) {
            var stairs = collision.gameObject.GetComponentInParent<Stairs>();
            if (stairs.Floor == ShipObject.Floor.Lower && CurrentGraph == gameManager.Graphs[1] ||
                stairs.Floor == ShipObject.Floor.Upper && CurrentGraph == gameManager.Graphs[0]) {
                ship.PirateChangeFloor(this, stairs.Floor);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.GetComponentInParent<CannonObject>() != null) {
            var cannon = collision.gameObject.GetComponentInParent<CannonObject>();
            if (cannon.IsRequestedPirate(this)) {
                cannon.Pirate = null;
                cannon.IsBusy = false;
            }
        }
    }


    private IEnumerator WaitDestinationBeforeRotation(Quaternion cannonRotation) {
        while (!reachedEndOfPath) {
            yield return new WaitForEndOfFrame();
        }

        print(Mathf.Abs(Quaternion.Dot(transform.rotation, cannonRotation)));

        while (Mathf.Abs(Quaternion.Dot(transform.rotation, cannonRotation)) < 0.9f) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, cannonRotation, 10f);
            yield return null;
        }
    }

    public void Move(Vector3 destination, GraphMask graphMask) {
        seeker.StartPath(transform.position, destination, OnPathComplete, graphMask);
    }

    private void Move(KeyValuePair<Vector3, GraphMask> nextDestination) {
        Move(nextDestination.Key, nextDestination.Value);
    }
}