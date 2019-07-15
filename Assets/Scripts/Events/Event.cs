using Map;
using UnityEngine;
using UnityEngine.UI;

namespace Events {
    /// <inheritdoc />
    /// <summary>
    /// Base class for the in-game events.
    /// </summary>
    public abstract class Event : MonoBehaviour {
        [SerializeField] protected Text text;
        [SerializeField] protected GameObject textContainer;
        [SerializeField] protected GameObject canvas;

        protected IQuest Quest { get; set; }
        protected Node Node { get; set; }

        /// <summary>
        /// Setup the introductory text of the Event.
        /// </summary>
        protected abstract void ShowText();

        /// <summary>
        /// Method responsible of the initialization of the event.
        /// </summary>
        /// <param name="node">The node linked to the event.</param>
        public abstract void InstantiateEventOnScreen(Node node);

        /// <summary>
        /// Starts the event on screen.
        /// </summary>
        public abstract void StartEvent();

        public abstract bool CanStartQuest(bool hasMapScreenQuests);
        public abstract bool CanEndQuest(IQuest quest);

        /// <summary>
        /// Method used by buttons to accept the Event request or to continue to the event if no choice are given to the player.
        /// </summary>
        public abstract void OnEventAccepted();

        public abstract void AssignQuest(IQuest quest);

        /// <summary>
        /// Shows the GameObject linked to the Event on screen.
        /// </summary>
        protected void Show() {
            canvas.gameObject.SetActive(true);
        }

        /// <summary>
        /// Hides the GameObject linked to the Event on screen.
        /// </summary>
        protected void Hide() {
            canvas.gameObject.SetActive(false);
        }
    }
}