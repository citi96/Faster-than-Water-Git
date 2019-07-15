using Managers.States.Actions.Interfaces;
using System.Collections.Generic;

namespace Managers.States
{
    /// <summary>
    /// Class representing the current state of the game. Each state holds actions that can be performed inside the state.
    /// </summary>
    public abstract class State {
        /// <summary>
        /// Actions defined for the state. Must be implemented in subclasses.
        /// </summary>
        protected IEnumerable<IAction> actions;

        /// <summary>
        /// Call each actions strategy inside the state. Must be called inside an update function.
        /// </summary>
        public void Tick() {
            foreach (var t in actions) {
                t.Execute();
            }
        }
    }
}