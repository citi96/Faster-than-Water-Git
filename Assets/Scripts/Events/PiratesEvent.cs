using Holders;
using Managers;
using Map;
using Ship;

namespace Events {
    public class PiratesEvent : EnemyEvent {
        public override void AssignQuest(IQuest quest) {
            Quest = quest;
        }

        public override bool CanEndQuest(IQuest quest) {
            return Quest.GetQuestType() == QuestType.Sea;
        }

        public override bool CanStartQuest(bool hasMapScreenQuests) {
            return false;
        }

        public override void InstantiateEventOnScreen(Node node) {
            Node = node;
        }

        public override void StartEvent() {
            textContainer.SetActive(true);
            ShowText();
            Show();
            Map.Map.Instance.HideShowMapUi(false);

            var ship = Instantiate(enemyObject).GetComponent<ShipObject>();
            ship.Side = side;
            GameManager.Instance.EnemyShip = ship;
        }

        protected override void ShowText() {
            text.text = "An enemy ship appears by the reef! \nThe fight is inevitable!";
        }

        #region Buttons action

        public override void OnEventAccepted() {
            base.OnEventAccepted();
            textContainer.SetActive(false);
        }

        #endregion
    }
}