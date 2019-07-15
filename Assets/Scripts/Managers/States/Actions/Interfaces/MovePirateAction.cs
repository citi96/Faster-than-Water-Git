using System;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using Ship;
using UnityEngine;

namespace Managers.States.Actions.Interfaces {
    /// <inheritdoc />
    /// <summary>
    /// Used, during game play, to handle user Mouse click for player movement. 
    /// </summary>
    public abstract class MovePirateAction : MouseClickAction {
        /// <inheritdoc />
        /// <summary>
        /// Implementation of the template method. Find the clicked object and if the GameManager reference to the SelectedPirate is not null, move the pirate to the clicked spot.
        /// </summary>
        protected override void FindClickedObject() {
            if (Camera.main != null) {
                var ray = new Ray(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);
                var hit = Physics2D.GetRayIntersection(ray);
                if (gameManager.SelectedPirate != null && hit.collider.gameObject.layer == 9) {
                    int index = hit.collider.gameObject.CompareTag("Upper Floor")
                        ? 0
                        : 1;

                    var graph = gameManager.Graphs[index];
                    var stairs = gameManager.Ship.Stairs.First(s => (int) s.Floor == Math.Abs(index - 1));

                    if (!graph.Equals(gameManager.SelectedPirate.CurrentGraph)) {
                        MoveToStairs(stairs);
                        gameManager.SelectedPirate.QueuedDestination.Enqueue(
                            new KeyValuePair<Vector3, GraphMask>(Camera.main.ScreenToWorldPoint(Input.mousePosition),
                                graph));
                    } else {
                        gameManager.SelectedPirate.Move(Camera.main.ScreenToWorldPoint(Input.mousePosition), graph);
                    }
                } else if (gameManager.SelectedPirate != null && hit.collider.gameObject.layer == 11) {
                    MoveToCannon(hit.collider.gameObject.GetComponent<CannonObject>());
                } else if (gameManager.SelectedPirate != null && hit.collider.gameObject.layer == 14) {
                    MoveToStairs(hit.collider.gameObject.GetComponent<Stairs>());
                }
            }
        }

        private void MoveToStairs(Stairs stairs) {
            gameManager.SelectedPirate.Move(stairs.StairsSpot.position, gameManager.SelectedPirate.CurrentGraph);
        }

        /// <summary>
        /// Template method to move the selected pirate to the cannon and begin the associated event.
        /// </summary>
        /// <param name="cannon"></param>
        protected abstract void MoveToCannon(CannonObject cannon);
    }
}