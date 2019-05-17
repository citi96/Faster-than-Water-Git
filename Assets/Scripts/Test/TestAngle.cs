using UnityEngine;
using UnityEditor;

public class TestAngle : MonoBehaviour {
    public GameObject node0;
    public GameObject node1;
    public Canvas canvas;

    private void Start() {
        GameObject dot = Resources.Load("Prefabs/Dot") as GameObject;
        
        Vector3 differrence = node1.transform.localPosition - node0.transform.localPosition;
        float angle = Mathf.Atan2(differrence.y, differrence.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

        GameObject tempDot = UnityEngine.Object.Instantiate(dot);
        tempDot.transform.parent = canvas.transform;
        tempDot.transform.localRotation = q;
        tempDot.transform.localPosition = node0.transform.localPosition;
        setDotPosition(tempDot, 2.5f * tempDot.transform.localScale.x);
        Vector3 dotPosition = tempDot.transform.localPosition;

        float distance = (dotPosition - node1.transform.localPosition).magnitude;

        int i = 0;
        while (distance > 5 && i++ < 10) {
            GameObject newDot = UnityEngine.Object.Instantiate(dot);
            newDot.transform.parent = canvas.transform;
            newDot.transform.localRotation = q;
            newDot.transform.localPosition = dotPosition;
            setDotPosition(newDot, 7.5f * newDot.transform.localScale.x);
            dotPosition = newDot.transform.localPosition;
            tempDot = newDot;

            distance = (dotPosition - node1.transform.localPosition).magnitude;
        }
    }

    private float? getM(Vector3 point1, Vector3 point2) {
        float? m = null;
        if (Mathf.Abs(point2.x - point1.x) > 1f) {
            m = (point2.y - point1.y) / (point2.x - point1.x);
        } 

        return m;
    }

        private void setDotPosition(GameObject dot, float shift) {
        float x = 1;
        float? m = getM(node0.transform.position, node1.transform.position);

        if(m == null) {
            m = node0.transform.position.x < node1.transform.position.x ? -1 : 1;
            x = 0;
        }

        if (node0.transform.position.x < node1.transform.position.x) {
            dot.transform.localPosition += new Vector3(shift * x, shift * (float)m, 0);
        } else {
            dot.transform.localPosition -= new Vector3(shift * x, shift * (float)m, 0);
        }
    }
}