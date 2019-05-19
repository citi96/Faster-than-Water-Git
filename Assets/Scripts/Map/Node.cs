using System.Collections.Generic;
using Events.Island;
using UnityEngine;
using UnityEngine.UI;
using Event = Assets.Scripts.Events.Event;

namespace Assets.Scripts.Map
{
    public class Node : MonoBehaviour {
        public LinkedList<Node> neighbours { get; } = new LinkedList<Node>();
        public bool canBeIsland { get; private set; } = true;

        private bool isStartingNode;
        private bool isBossRoom = false;
        private int? height = null;
        private Event eventType;
        private LinkedList<GameObject> connections = new LinkedList<GameObject>();
        private bool isCompletelyShown = false;

        public bool IsStartingNode {
             get {
                return isStartingNode;
            }

            set {
                isStartingNode = value;
                gameObject.GetComponent<Image>().color = Color.black;
                canBeIsland = false;
                isCompletelyShown = true;
            }
        }
        public bool IsBossRoom {
            get {
                return isBossRoom;
            }
            set {
                if (value) {
                    GetComponent<Image>().color = Color.black;
                }
                isBossRoom = this;
                canBeIsland = false;
            }
        }

        private void setupNeighbourHeight() {
            foreach (Node n in neighbours) {
                n.SetHeight(height + 1);
            }
        }

        public void addNeighbour(Node n) {
            neighbours.AddLast(n);
        }

        public int? GetHeight() {
            return height;
        }

        public void SetHeight(int? value) {
            if (GetHeight() == null || value < GetHeight()) {
                if (IsStartingNode) {
                    height = 0;
                } else {
                    height = value;
                    GetComponent<Image>().color = Color.Lerp(Color.yellow, Color.red, (float)height / 10f);
                }
                setupNeighbourHeight();
            }
        }

        public void onButtonPressed() {
            if (eventType != null) {
                eventType.StartEvent();
            } else {
                print("Argh!");
            }

            if (!isCompletelyShown) {
                showNeighbour();
            }
        }

        private void showNeighbour() {
            setActive(true);
            foreach (Node n in neighbours) {
                n.gameObject.SetActive(true);
            }

            isCompletelyShown = true;
        }

        public void assignIslandEvent(GameObject island) {
            foreach (Node n in neighbours) {
                n.canBeIsland = false;
            }
            GameObject newIsland = Instantiate(island);
            newIsland.transform.SetParent(GameObject.Find("Event Holder").transform);
            eventType = newIsland.GetComponent<Island>();
            eventType.InstantiateEventOnScreen(this);
        }

        public void assignEnemyEvent(GameObject pirates) {
            
        }

        public void setActive(bool active) {
            gameObject.SetActive(active);
            foreach (GameObject c in connections) {
                c.SetActive(active);
            }
        }

        public void instantiateDots(Node neighbour) {
            Transform holder = GameObject.Find("Map").transform.Find("Canvas/MapElements/Connections");
            RectTransform canvas = holder.parent.parent.gameObject.GetComponent<RectTransform>();

            GameObject node0 = gameObject;
            GameObject node1 = neighbour.gameObject;

            GameObject dot = Resources.Load("Prefabs/Dot") as GameObject;

            Vector3 differrence = node1.transform.localPosition - node0.transform.localPosition;
            float angle = Mathf.Atan2(differrence.y, differrence.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

            GameObject tempDot = Instantiate(dot);
            connections.AddLast(tempDot);
            tempDot.transform.localScale = new Vector3(tempDot.transform.localScale.x * canvas.localScale.x / 0.145f, tempDot.transform.localScale.y * canvas.localScale.x / 0.145f, 1);
            neighbour.connections.AddLast(tempDot);
            tempDot.transform.SetParent(holder);
            tempDot.transform.localRotation = q;
            tempDot.transform.localPosition = node0.transform.localPosition;
            setDotPosition(tempDot, node0, node1, 4f * tempDot.transform.localScale.x * canvas.localScale.x / 0.145f);
            Vector3 dotPosition = tempDot.transform.localPosition;

            float distance = (dotPosition - node1.transform.localPosition).magnitude;

            int i = 0;
            while (distance > 5 && i++ < 100) {
                GameObject newDot = Instantiate(dot);
                connections.AddLast(newDot);
                newDot.transform.localScale = new Vector3(newDot.transform.localScale.x * canvas.localScale.x / 0.145f, newDot.transform.localScale.y * canvas.localScale.x / 0.145f, 1);
                neighbour.connections.AddLast(newDot);
                newDot.transform.SetParent(holder);
                newDot.transform.localRotation = q;
                newDot.transform.localPosition = dotPosition;
                setDotPosition(newDot, node0, node1, 9f * newDot.transform.localScale.x * canvas.localScale.x / 0.145f);
                dotPosition = newDot.transform.localPosition;
                tempDot = newDot;

                distance = (dotPosition - node1.transform.localPosition).magnitude;
            }
        }

        private void setDotPosition(GameObject dot, GameObject node0, GameObject node1, float shift) {
            dot.transform.Translate(new Vector3(shift, 0, 0), Space.Self);
        }
    }
}
