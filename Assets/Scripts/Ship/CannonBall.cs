using System.Collections;
using UnityEngine;

namespace Ship {
    public class CannonBall : MonoBehaviour {
        public IEnumerator Move(float speed) {
            while (true) {
                transform.Translate(Vector3.up * Time.deltaTime * speed, Space.Self);
                yield return null;
            }
        }
    }
}