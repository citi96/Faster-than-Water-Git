using UnityEngine;
using System.Collections;

public class CannonBall : MonoBehaviour {
    [SerializeField] private float speed = default;

    public IEnumerator move() {
        while (true) {
            transform.Translate(Vector3.up * Time.deltaTime * speed, Space.Self);
            yield return null;
        }
    }
}
