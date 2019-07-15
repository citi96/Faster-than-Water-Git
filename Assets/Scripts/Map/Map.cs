using System.Collections.Generic;
using System.Linq;
using Managers;
using Managers.States;
using UnityEngine;

namespace Map {
    public class Map : MonoBehaviour {
        [SerializeField] private GameObject mapUI;
        [SerializeField] private GameObject[] island;
        [SerializeField] private GameObject[] enemyShip;

        private State state;

        public int Difficulty { get; private set; }
        public static Map Instance { get; private set; }
        public List<Node> Nodes { private set; get; }
        public Node CurrentNode { get; set; }

        private void Awake() {
            if (Instance == null) {
                Instance = this;
                state = new MapState();
            } else {
                Destroy(this);
            }
            Nodes = new List<Node>();
            Difficulty = 0;
        }

        public void Setup() {
            mapUI.SetActive(false);
            Debug.Log(Nodes.Count);

            SetStartingNode();
            Nodes = Nodes.OrderBy(n => n.Height).ToList();
            Nodes[Nodes.Count - 1].IsBossRoom = true;
            DrawDots();
            HideNodesAndConnections();
            ShowCurrentNodePaths();

            SetUpIslands();
            SetUpPirates();
        }

        /// <summary>
        /// Change map visibility according to the active parameter and change the state accordingly.
        /// </summary>
        /// <param name="active">Parameter used inside the SetActive method.</param>
        public void HideShowMapUi(bool active) {
            mapUI.SetActive(active);

            GameManager.Instance.State = active ? state : null;
        }

        public bool IsMapUiActive() {
            return mapUI.activeSelf;
        }

        private void SetStartingNode() {
            var startingNode = Nodes[Random.Range(0, Nodes.Count)];
            startingNode.IsStartingNode = true;
            startingNode.Height = 0;
            CurrentNode = startingNode;
        }

        private void DrawDots() {
            foreach (var n in Nodes) {
                foreach (var neighbor in n.Neighbors) {
                    if (n.Height < neighbor.Height) {
                        n.InstantiateDots(neighbor);
                    }
                }
            }
        }

        private void HideNodesAndConnections() {
            foreach (var n in Nodes) {
                foreach (var neighbor in n.Neighbors) {
                    if (n.Height < neighbor.Height) {
                        neighbor.SetActive(false);
                    }
                }
            }
        }

        private void ShowCurrentNodePaths() {
            CurrentNode.SetActive(true);
            foreach (var n in CurrentNode.Neighbors) {
                n.gameObject.SetActive(true);
            }
        }

        private void SetUpIslands() {
            int maxIsland = Difficulty > 9 ? 2 : Difficulty > 5 ? 3 : Difficulty > 3 ? 4 : 5;

            while (maxIsland > 0) {
                var node = Nodes[Random.Range(0, Nodes.Count)];
                if (node.CanBeIsland) {
                    node.AssignIslandEvent(island[0]);
                    maxIsland--;
                }
            }
        }

        private void SetUpPirates() {
            foreach (var node in Nodes) {
                if (node.Height > 0) {
                    node.AssignEnemyEvent(enemyShip[0]);
                }
            }
        }
    }
}