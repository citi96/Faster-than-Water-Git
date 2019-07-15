using Holders;
using Managers;
using Managers.States;
using UnityEngine;

namespace Events {
    /// <inheritdoc />
    /// <summary>
    /// Base class for the Enemy event.
    /// </summary>
    public abstract class EnemyEvent : Event {
        [SerializeField] protected GameObject enemyObject;

        /// <summary>
        /// The Side of the event.
        /// </summary>
        protected readonly Vector2 side = EnemySide.Side;

        private void Start() {
            Hide();
        }

        public override void OnEventAccepted() {
            GameManager.Instance.State = new BattleState();
        }
    }
}