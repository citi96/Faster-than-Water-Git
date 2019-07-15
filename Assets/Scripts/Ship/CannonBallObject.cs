using System.Collections;
using Items.Interfaces.CannonBall;
using UnityEngine;

namespace Ship {
    public class CannonBallObject : MonoBehaviour {
        private ITarget target;
        private ICannonBall cannonBall;

        public IEnumerator Move(float speed, ITarget target, ICannonBall cannonBall) {
            this.target = target;
            this.cannonBall = cannonBall;

            while (true) {
                transform.Translate(Vector3.up * Time.unscaledDeltaTime * speed, Space.Self);
                yield return null;
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            var hitTarget = other.GetComponent<ITarget>();

            if (hitTarget != null && hitTarget.Equals(target)) {
                cannonBall.Bonus(hitTarget);
                hitTarget.ApplyDamage(cannonBall);
                Destroy(this);
            }
        }
    }
}