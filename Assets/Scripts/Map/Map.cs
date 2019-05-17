using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Map {
    public class Map : MonoBehaviour {
        [SerializeField] private GameObject canvas;
        [SerializeField] private GameObject[] island;
        [SerializeField] private GameObject[] enemyShip;
        private Node currentNode;
        private int difficulty = 0;

        public bool isMapOpenable { private get; set; } = true;
        public static Map Instance { get; private set; }
        public List<Node> nodes { private set; get; }
        public Node CurrentNode { private get => currentNode; set => currentNode = value; }

        void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(this);
            }

            nodes = new List<Node>();
        }

        public void setup() {
            canvas.SetActive(false);
            Debug.Log(nodes.Count);

            setStartingNode();
            selectionSort();
            nodes[nodes.Count - 1].IsBossRoom = true;
            drawDots();
            hideNodesAndConnections();
            showCurrentNodePaths();

            setUpIslands();
        }

        private void hideNodesAndConnections() {
            foreach (Node n in nodes) {
                foreach (Node neighbour in n.neighbours) {
                    if (n.GetHeight() < neighbour.GetHeight()) {
                        neighbour.setActive(false);
                    }
                }
            }
        }

        private void showCurrentNodePaths() {
            currentNode.setActive(true);
            foreach (Node n in currentNode.neighbours) {
                n.gameObject.SetActive(true);
            }
        }

        private void drawDots() {
            foreach (Node n in nodes) {
                foreach (Node neighbour in n.neighbours) {
                    if (n.GetHeight() < neighbour.GetHeight()) {
                        n.instantiateDots(neighbour);
                    }
                }
            }
        }

        private void setUpIslands() {
            int maxIsland = difficulty > 9 ? 2 : difficulty > 5 ? 3 : difficulty > 3 ? 4 : 5;

            while (maxIsland > 0) {
                Node node = nodes[UnityEngine.Random.Range(0, nodes.Count)];
                if (node.canBeIsland) {
                    node.assignIslandEvent(island[0]);
                    maxIsland--;
                }
            }
        }

        private void setStartingNode() {
            Node startingNode = nodes[UnityEngine.Random.Range(0, nodes.Count)];
            startingNode.IsStartingNode = true;
            startingNode.SetHeight(0);
            CurrentNode = startingNode;
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.M) && isMapOpenable) {
                hideShowCanvas(!canvas.activeSelf);
            }
        }

        public void hideShowCanvas(bool active) {
            canvas.SetActive(active);
        }

        public bool isCanvasActive() {
            return canvas.activeSelf;
        }

        private void selectionSort() {
            int length = nodes.Count;

            for (int i = 0; i < length - 1; i++) {

                for (int j = i + 1; j < length; j++) {
                    if (nodes[i].GetHeight() > nodes[j].GetHeight())
                        swap(i, j); // ordine ascendente
                }
            }
        }

        private void swap(int i, int j) {
            Node tmp = nodes[i];
            nodes[i] = nodes[j];
            nodes[j] = tmp;
        }
    }
}
