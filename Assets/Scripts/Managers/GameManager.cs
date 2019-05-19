using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using Pathfinding;
using Ship;
using UnityEngine;

namespace Managers {
    public class GameManager : MonoBehaviour {
        public static GameManager instance { get; private set; }
        public Pirate selectedPlayer { get; private set; }

        private SpriteRenderer spriteRenderer;
        private ShipObject ship;

        private void Awake() {
            instance = this;
            ship = GameObject.FindGameObjectWithTag("Ship").GetComponent<ShipObject>();
        }

        private void Update() {
            if (!Assets.Scripts.Map.Map.Instance.isCanvasActive()) {
                if (Input.GetMouseButtonDown(0)) {
                    FindClickedObject();
                }

                if (Input.GetKeyDown(KeyCode.Escape) && selectedPlayer != null) {
                    DeselectPlayer();
                }

                if (Input.GetKeyDown(KeyCode.Space)) {
                    AstarPath.active.Scan();
                }

                if (Input.GetKeyDown(KeyCode.UpArrow) && ship.CurrentFloor != ShipObject.Floor.Upper) {
                    ship.CurrentFloor = ShipObject.Floor.Upper;
                } else if (Input.GetKeyDown(KeyCode.DownArrow) && ship.CurrentFloor != ShipObject.Floor.Lower) {
                    ship.CurrentFloor = ShipObject.Floor.Lower;
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
                    int index = hit.collider.gameObject.CompareTag("Upper Floor")
                        ? 0
                        : 1;

                    var graph = selectedPlayer.graphs[index];
                    var stairs = ship.Stairs.First(s => (int) s.Floor == Math.Abs(index - 1));

                    if (!graph.Equals(selectedPlayer.CurrentGraph)) {
                        MoveToStairs(stairs);
                        selectedPlayer.QueuedDestination.Enqueue(
                            new KeyValuePair<Vector3, GraphMask>(Camera.main.ScreenToWorldPoint(Input.mousePosition),
                                graph));
                    } else {
                        selectedPlayer.Move(Camera.main.ScreenToWorldPoint(Input.mousePosition), graph);
                    }
                } else if (selectedPlayer != null && hit.collider.gameObject.layer == 11) {
                    MoveToCannon(hit.collider.gameObject.GetComponent<CannonObject>());
                } else if (selectedPlayer != null && hit.collider.gameObject.layer == 14) {
                    MoveToStairs(hit.collider.gameObject.GetComponent<Stairs>());
                }
            }
        }

        private void MoveToStairs(Stairs stairs) {
            selectedPlayer.Move(stairs.StairsSpot.position, selectedPlayer.CurrentGraph);
        }

        private void DeselectPlayer() {
            spriteRenderer.color = Color.white;
            selectedPlayer = null;
        }

        private void MoveToCannon(CannonObject cannon) {
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