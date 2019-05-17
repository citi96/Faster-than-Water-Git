using System;
using UnityEngine;

public class Ship : MonoBehaviour {
    public enum Floor {
        Upper,
        Lower
    }

    [SerializeField] private Transform upperSpawn;
    [SerializeField] private Transform lowerSpawn;

    private GameObject upperFloor;
    private GameObject lowerFloor;
    private Floor currentFloor;

    public Floor CurrentFloor {
        get => currentFloor;
        set {
            currentFloor = value;
            SwitchFloor(value);
        }
    }

    public Stairs Stairs { get; private set; }

    private void Awake() {
        upperFloor = GameObject.FindGameObjectWithTag("Upper Floor");
        lowerFloor = GameObject.FindGameObjectWithTag("Lower Floor");
        Stairs = GetComponentInChildren<Stairs>();
    }

    private void SwitchFloor(Floor floor) {
        switch (floor) {
            case Floor.Upper:
                SetupSwitch(upperFloor, lowerFloor);
                break;
            case Floor.Lower:
                SetupSwitch(lowerFloor, upperFloor);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(floor), floor, null);
        }
    }

    private void SetupSwitch(GameObject roseFloor, GameObject loweredFloor) {
        roseFloor.GetComponent<SpriteRenderer>().sortingOrder = 0;
        loweredFloor.GetComponent<SpriteRenderer>().sortingOrder = -3;
        ChangeFloorChildren(roseFloor, loweredFloor);
    }

    private void ChangeFloorChildren(GameObject roseFloor, GameObject loweredFloor) {
        foreach (var child in roseFloor.transform.GetComponentsInChildren<SpriteRenderer>()) {
            ChangeFloor(child, 1);
            child.gameObject.transform.position -= Vector3.forward;
        }

        foreach (var child in loweredFloor.transform.GetComponentsInChildren<SpriteRenderer>()) {
            ChangeFloor(child, -2);
            child.gameObject.transform.position += Vector3.forward;
        }
    }

    private static void ChangeFloor(Renderer sprite, int? floor = null) {
        if (floor != null) {
            sprite.sortingOrder = (int) floor;
        } else {
            int sortingOrder = sprite.sortingOrder;
            sortingOrder = sortingOrder == 1 ? -2 : 1; //Controllare se va bene 1
            sprite.sortingOrder = sortingOrder;
        }
    }

    public void PirateChangeFloor(Pirate pirate, Floor floor) {
        switch (floor) {
            case Floor.Upper:
                pirate.gameObject.transform.SetParent(lowerFloor.transform);
                pirate.CurrentGraph = pirate.graphs[1];
                pirate.transform.position = lowerSpawn.position;
                break;
            case Floor.Lower:
                pirate.gameObject.transform.SetParent(upperFloor.transform);
                pirate.CurrentGraph = pirate.graphs[0];
                pirate.transform.position = upperSpawn.position;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(floor), floor, null);
        }

        pirate.Move(pirate.position, pirate.CurrentGraph);
        ChangeFloor(pirate.gameObject.GetComponent<SpriteRenderer>());
    }
}