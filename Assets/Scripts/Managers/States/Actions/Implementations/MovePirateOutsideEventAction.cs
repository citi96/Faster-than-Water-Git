using Managers.States.Actions.Interfaces;
using Ship;

namespace Managers.States.Actions.Implementations {
    /// <inheritdoc cref="MovePirateAction" />
    public class MovePirateOutsideEventAction : MovePirateAction {
        /// <inheritdoc />
        /// <summary>
        /// Do nothing since cannons are unusable outside an EnemyEvent.
        /// </summary>
        /// <param name="cannon"></param>
        protected override void MoveToCannon(CannonObject cannon) { }
    }
}