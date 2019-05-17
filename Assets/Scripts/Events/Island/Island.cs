using Assets.Scripts.Holders;
using Assets.Scripts.Map;
using UnityEngine;

namespace Assets.Scripts.Events.Island
{
    public class Island : Event {
        [SerializeField] private GameObject shopUi;
        [SerializeField] private GameObject shopWindow;
        [SerializeField] private GameObject questUi;
        private IQuest quest;
        private Shop shop;
        private GameObject activeScreen;

        private void Start() {
            shopWindow.SetActive(false);
            Hide();
        }

        public override void InstantiateEventOnScreen(Node node) {
            this.node = node;
            SetupQuestUi();
            shop = new Shop(node.GetHeight().GetValueOrDefault());        
        }

        private void SetupQuestUi() {
            if (quest == null) questUi.SetActive(false);
        }

        protected override void ShowText() {
            text.text = "You hear the lookout call, an island is in sight.\r\nDo you want to land?";
        }

        public override bool CanStartQuest(bool hasMapScreenQuests) {
            return quest != null;
        }

        public override bool CanEndQuest(IQuest quest) {
            return quest.GetQuestType() == QuestType.Island;                
            
        }

        public override void AssignQuest(IQuest quest) {
            this.quest = quest;
        }

        public override void StartEvent() {
            Map.Map.Instance.isMapOpenable = false;
            textContainer.SetActive(true);
            ShowText();
            Show();
            Map.Map.Instance.hideShowCanvas(false);
        }

        #region Buttons action

        public void OnEventRefused() {
            Map.Map.Instance.hideShowCanvas(true);
            Hide();
        }

        public override void OnEventAccepted() {
            textContainer.SetActive(false);
        }

        public void OnQuestButtonPressed() {

        }

        public void OnShopButtonPressed() {
            activeScreen = shopWindow;
            shopWindow.SetActive(true);
        }

        public void OnExitButtonPressed() {
            activeScreen.SetActive(false);
        }

        public void LeaveIsland() {
            Map.Map.Instance.isMapOpenable = true;
            activeScreen = canvas;
            OnExitButtonPressed();
        }

        #endregion
    }
}
