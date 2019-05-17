using Pathfinding;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class GameManager : MonoBehaviour {
        public static GameManager instance { get; private set; }
        public Pirate selectedPlayer { get; private set; }

        private SpriteRenderer spriteRenderer;
        private Ship ship;

        private void Awake() {
            instance = this;
            ship = GameObject.FindGameObjectWithTag("Ship").GetComponent<Ship>();
        }

        private void Update() {
            if (!Map.Map.Instance.isCanvasActive()) {
                if (Input.GetMouseButtonDown(0)) {
                    FindClickedObject();
                }

                if (Input.GetKeyDown(KeyCode.Escape) && selectedPlayer != null) {
                    DeselectPlayer();
                }

                if (Input.GetKeyDown(KeyCode.Space)) {
                    AstarPath.active.Scan();
                }

                if (Input.GetKeyDown(KeyCode.UpArrow) && ship.CurrentFloor != Ship.Floor.Upper) {
                    ship.CurrentFloor = Ship.Floor.Upper;
                } else if (Input.GetKeyDown(KeyCode.DownArrow) && ship.CurrentFloor != Ship.Floor.Lower) {
                    ship.CurrentFloor = Ship.Floor.Lower;
                }
            }
        }

        private void FindClickedObject() {
            if (Camera.main != null) {
                var ray = new Ray(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);
                var hit = Physics2D.GetRayIntersection(ray);
                if (hit.collider.gameObject.CompareTag("Player")) {
                    if (selectedPlayer != null) {
                        DeselectPlayer();
                    }

                    SelectPlayer(hit);
                } else if (selectedPlayer != null && hit.collider.gameObject.layer == 9) {
                    var graph = hit.collider.gameObject.CompareTag("Upper Floor")
                        ? selectedPlayer.graphs[0]
                        : selectedPlayer.graphs[1];

                    if (!graph.Equals(selectedPlayer.CurrentGraph)) {
                        MoveToStairs(ship.Stairs);
                        selectedPlayer.QueuedDestination.Enqueue(
                            new KeyValuePair<Vector3, GraphMask>(Camera.main.ScreenToWorldPoint(Input.mousePosition),
                                graph));
                    } else {
                        selectedPlayer.Move(Camera.main.ScreenToWorldPoint(Input.mousePosition), graph);
                    }
                } else if (selectedPlayer != null && hit.collider.gameObject.layer == 11) {
                    MoveToCannon(hit.collider.gameObject.GetComponent<Cannon>());
                } else if (selectedPlayer != null && hit.collider.gameObject.layer == 14) {
                    MoveToStairs(hit.collider.gameObject.GetComponent<Stairs>());
                }
            }
        }

        private void MoveToStairs(Stairs stairs) {
            if (selectedPlayer.CurrentGraph == selectedPlayer.graphs[0]) {
                selectedPlayer.Move(stairs.StairsSpot.position, selectedPlayer.CurrentGraph);
            }
        }

        private void DeselectPlayer() {
            spriteRenderer.color = Color.white;
            selectedPlayer = null;
        }

        private void MoveToCannon(Cannon cannon) {
            if (!cannon.IsBusy) {
                cannon.Pirate = selectedPlayer;
                selectedPlayer.Move(cannon.PiratePosts.position, selectedPlayer.CurrentGraph);
            }
        }

        private void SelectPlayer(RaycastHit2D hit) {
            selectedPlayer = hit.collider.gameObject.GetComponent<Pirate>();
            spriteRenderer = selectedPlayer.gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.color = Color.red;
        }
    }
}