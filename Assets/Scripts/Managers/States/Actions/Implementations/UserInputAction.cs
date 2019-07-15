using Managers.States.Actions.Interfaces;
using Ship;
using UnityEngine;

namespace Managers.States.Actions.Implementations
{
    /// <inheritdoc />
    /// <summary>
    /// Class used, during game play, to handle User inputs not concerning player movement and map opening.
    /// </summary>
    public class UserInputAction : MouseClickAction {
        private SpriteRenderer spriteRenderer;

        public override void Execute() {
            base.Execute();

            if (Input.GetKeyDown(KeyCode.Escape) && gameManager.SelectedPirate != null) {
                DeselectPlayer();
            }

            if (Input.GetKeyDown(KeyCode.Space)) {
                AstarPath.active.Scan();
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0f && gameManager.Ship.CurrentFloor != ShipObject.Floor.Upper) {
                gameManager.Ship.CurrentFloor = ShipObject.Floor.Upper;
            } else if (Input.GetAxis("Mouse ScrollWheel") < 0f && gameManager.Ship.CurrentFloor != ShipObject.Floor.Lower) {
                gameManager.Ship.CurrentFloor = ShipObject.Floor.Lower;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Handle click on Pirate object.
        /// </summary>
        protected override void FindClickedObject() {
            if (Camera.main != null) {
                var ray = new Ray(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);
                var hit = Physics2D.GetRayIntersection(ray);
                if (hit.collider.gameObject.CompareTag("Player")) {
                    if (gameManager.SelectedPirate != null) {
                        DeselectPlayer();
                    }

                    SelectPlayer(hit);
                }
            }
        }

        /// <summary>
        /// Sets to null the reference of the GameManager's SelectedPirate and colors it to white (transparent).
        /// </summary>
        private void DeselectPlayer() {
            spriteRenderer.color = Color.white;
            gameManager.SelectedPirate = null;
        }

        /// <summary>
        /// Sets the SelectedPirate to the clicked Pirate, takes a reference to its sprite renderer and color it to red. If Initially SelectedPlayer is not null, it calls DeselectPlayer first.
        /// </summary>
        /// <param name="hit"></param>
        private void SelectPlayer(RaycastHit2D hit) {
            if (gameManager.SelectedPirate != null) {
                DeselectPlayer();
            }

            gameManager.SelectedPirate = hit.collider.gameObject.GetComponent<Pirate>();
            spriteRenderer = gameManager.SelectedPirate.gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.color = Color.red;
        }
    }
}