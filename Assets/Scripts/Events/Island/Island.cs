using Holders;
using Map;
using UnityEngine;

namespace Events.Island {
    /// <summary>
    /// The Script attached to the Island prefabs. It is responsible of the handling of the event.
    /// </summary>
    public class Island : Event {
        [SerializeField] private GameObject shopUi;
        [SerializeField] private GameObject shopWindow;
        [SerializeField] private GameObject questUi;
        private Shop shop;
        private GameObject activeScreen;

        private void Start() {
            shopWindow.SetActive(false);
            Hide();
        }

        public override void InstantiateEventOnScreen(Node node) {
            Node = node;
            SetupQuestUi();
            shop = new Shop(node.Height.GetValueOrDefault());
        }

        private void SetupQuestUi() {
            if (Quest == null) questUi.SetActive(false);
        }

        protected override void ShowText() {
            text.text = "You hear the lookout call, an island is in sight.\r\nDo you want to land?";
        }

        public override bool CanStartQuest(bool hasMapScreenQuests) {
            return Quest != null;
        }

        public override bool CanEndQuest(IQuest quest) {
            return quest.GetQuestType() == QuestType.Island;
        }

        public override void AssignQuest(IQuest quest) {
            Quest = quest;
        }

        public override void StartEvent() {
            textContainer.SetActive(true);
            ShowText();
            Show();
            Map.Map.Instance.HideShowMapUi(false);
        }

        #region Buttons action

        /// <summary>
        /// Used by buttons to refuse an Event.
        /// </summary>
        public void OnEventRefused() {
            Map.Map.Instance.HideShowMapUi(true);
            Hide();
        }

        public override void OnEventAccepted() {
            textContainer.SetActive(false);
        }

        /// <summary>
        /// Used by buttons to open the quest window, if available, on Islands.
        /// </summary>
        public void OnQuestButtonPressed() { }

        /// <summary>
        /// Used by buttons to open the shop window on Islands.
        /// </summary>
        public void OnShopButtonPressed() {
            activeScreen = shopWindow;
            shopWindow.SetActive(true);
        }

        /// <summary>
        /// Used by buttons to close the most recent opened menu tab on Islands.
        /// </summary>
        public void OnExitButtonPressed() {
            activeScreen.SetActive(false);
        }


        /// <summary>
        /// Used by buttons to leave the Island.
        /// </summary>
        public void LeaveIsland() {
            activeScreen = canvas;
            OnExitButtonPressed();
        }

        #endregion
    }
}