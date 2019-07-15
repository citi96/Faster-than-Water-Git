using Managers.States.Actions.Interfaces;
using Ship;

namespace Managers.States.Actions.Implementations {
    /// <inheritdoc cref="MovePirateAction" />
    public class MovePirateDuringEventAction : MovePirateAction {
        /// <inheritdoc />
        /// <summary>
        /// Actually move the selected pirate to the clicked cannon.
        /// </summary>
        /// <param name="cannon"></param>
        protected override void MoveToCannon(CannonObject cannon) {
            if (!cannon.IsBusy) {
                cannon.Pirate = gameManager.SelectedPirate;
                gameManager.SelectedPirate.Move(cannon.PiratePosts.position, gameManager.SelectedPirate.CurrentGraph);
            }
        }
    }
}