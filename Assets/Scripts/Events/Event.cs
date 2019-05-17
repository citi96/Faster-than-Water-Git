using Assets.Scripts.Map;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Events
{
    public abstract class Event : MonoBehaviour {
        [SerializeField] protected Text text;
        [SerializeField] protected GameObject textContainer;
        [SerializeField] protected GameObject canvas;

        protected Node node;

        protected abstract void  ShowText();
        public abstract void InstantiateEventOnScreen(Node node);
        public abstract void StartEvent();
        public abstract bool CanStartQuest(bool hasMapScreenQuests);
        public abstract bool CanEndQuest(IQuest quest);
        public abstract void OnEventAccepted();
        public abstract void AssignQuest(IQuest quest);

        protected void Show() {
            canvas.gameObject.SetActive(true);
        }

        protected void Hide() {
            canvas.gameObject.SetActive(false);
        }
    }
}