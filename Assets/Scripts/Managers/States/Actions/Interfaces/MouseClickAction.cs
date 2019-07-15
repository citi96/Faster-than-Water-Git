using UnityEngine;

namespace Managers.States.Actions.Interfaces {
    public abstract class MouseClickAction : IAction {
        protected readonly GameManager gameManager = GameManager.Instance;

        public virtual void Execute() {
            if (Input.GetMouseButtonDown(0)) {
                FindClickedObject();
            }
        }

        /// <summary>
        /// Template method to handle clicked in game objects.
        /// </summary>
        protected abstract void FindClickedObject();
    }
}