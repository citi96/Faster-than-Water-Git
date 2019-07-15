using Events;
using Events.Island;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Event = Events.Event;

namespace Map {
    public class Node : MonoBehaviour {
        private bool isStartingNode;
        private bool isBossRoom;
        private int? height;
        private Event eventType;
        private readonly LinkedList<GameObject> connections = new LinkedList<GameObject>();
        private bool isCompletelyShown;

        public bool CanBeIsland { get; private set; } = true;

        public bool IsStartingNode {
            private get => isStartingNode;
            set {
                isStartingNode = value;
                gameObject.GetComponent<Image>().color = Color.black;
                CanBeIsland = false;
                isCompletelyShown = true;
            }
        }

        public bool IsBossRoom {
            get => isBossRoom;
            set {
                if (value) {
                    GetComponent<Image>().color = Color.black;
                }

                isBossRoom = this;
                CanBeIsland = false;
            }
        }

        public int? Height {
            get => height;
            set {
                if (value.HasValue && Height == null || value < Height) {
                    if (IsStartingNode) {
                        height = 0;
                    } else {
                        height = value.Value;
                        GetComponent<Image>().color = Color.Lerp(Color.yellow, Color.red, height.Value / 10f);
                    }

                    SetupNeighborHeight();
                }
            }
        }

        public int Difficulty { get; set; }

        public LinkedList<Node> Neighbors { get; } = new LinkedList<Node>();

        private void SetupNeighborHeight() {
            foreach (var n in Neighbors) {
                n.Height = Height + 1;
            }
        }

        public void AddNeighbor(Node n) {
            Neighbors.AddLast(n);
        }

        public void OnButtonPressed() {
            if (eventType != null) {
                eventType.StartEvent();
            } else {
                print("Argh!");
            }

            if (!isCompletelyShown) {
                ShowNeighbor();
            }
        }

        private void ShowNeighbor() {
            SetActive(true);
            foreach (var n in Neighbors) {
                n.gameObject.SetActive(true);
            }

            isCompletelyShown = true;
        }

        public void AssignIslandEvent(GameObject island) {
            foreach (var n in Neighbors) {
                n.CanBeIsland = false;
            }

            var newIsland = Instantiate(island, GameObject.Find("Event Holder").transform, true);
            eventType = newIsland.GetComponent<Island>();
            eventType.InstantiateEventOnScreen(this);
        }

        public void AssignEnemyEvent(GameObject pirates) {
            if (eventType == null) {
                var newPirate = Instantiate(pirates, GameObject.Find("Event Holder").transform, true);
                eventType = newPirate.GetComponent<PiratesEvent>();
                eventType.InstantiateEventOnScreen(this);
            }
        }

        public void SetActive(bool active) {
            gameObject.SetActive(active);
            foreach (var c in connections) {
                c.SetActive(active);
            }
        }

        public void InstantiateDots(Node neighbor) {
            var holder = GameObject.Find("UI").transform.Find("Canvas/MapElements/Connections");
            var canvas = holder.parent.parent.gameObject.GetComponent<RectTransform>();

            var node0 = gameObject;
            var node1 = neighbor.gameObject;

            var dot = Resources.Load("Prefabs/Dot") as GameObject;

            var node0LocalPosition = node0.transform.localPosition;
            var node1LocalPosition = node1.transform.localPosition;
            var difference = node1LocalPosition - node0LocalPosition;
            float angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            var q = Quaternion.AngleAxis(angle, Vector3.forward);

            var tempDot = Instantiate(dot, holder, true);
            connections.AddLast(tempDot);
            var tempDotLocalScale = tempDot.transform.localScale;
            var canvasLocalScale = canvas.localScale;
            tempDotLocalScale = new Vector3(tempDotLocalScale.x * canvasLocalScale.x / 0.145f,
                tempDotLocalScale.y * canvasLocalScale.x / 0.145f, 1);
            tempDot.transform.localScale = tempDotLocalScale;
            neighbor.connections.AddLast(tempDot);
            tempDot.transform.localRotation = q;
            tempDot.transform.localPosition = node0LocalPosition;
            SetDotPosition(tempDot, 4f * tempDotLocalScale.x * canvasLocalScale.x / 0.145f);
            var dotPosition = tempDot.transform.localPosition;

            float distance = (dotPosition - node1LocalPosition).magnitude;

            int i = 0;
            while (distance > 5 && i++ < 100) {
                var newDot = Instantiate(dot, holder, true);
                connections.AddLast(newDot);
                var newDorLocalScale = newDot.transform.localScale;
                newDorLocalScale = new Vector3(newDorLocalScale.x * canvas.localScale.x / 0.145f,
                    newDorLocalScale.y * canvasLocalScale.x / 0.145f, 1);
                newDot.transform.localScale = newDorLocalScale;
                neighbor.connections.AddLast(newDot);
                newDot.transform.localRotation = q;
                newDot.transform.localPosition = dotPosition;
                SetDotPosition(newDot, 9f * newDorLocalScale.x * canvasLocalScale.x / 0.145f);
                dotPosition = newDot.transform.localPosition;
                tempDot = newDot;

                distance = (dotPosition - node1.transform.localPosition).magnitude;
            }
        }

        private static void SetDotPosition(GameObject dot, float shift) {
            dot.transform.Translate(new Vector3(shift, 0, 0), Space.Self);
        }
    }
}