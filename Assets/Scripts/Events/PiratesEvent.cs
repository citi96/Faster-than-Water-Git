using Assets.Scripts.Map;

namespace Assets.Scripts.Events
{
    public class PiratesEvent : EnemyEvent {
        public override void AssignQuest(IQuest quest) {
            throw new System.NotImplementedException();
        }

        public override bool CanEndQuest(IQuest quest) {
            throw new System.NotImplementedException();
        }

        public override bool CanStartQuest(bool hasMapScreenQuests) {
            throw new System.NotImplementedException();
        }

        public override void InstantiateEventOnScreen(Node node) {
            throw new System.NotImplementedException();
        }

        public override void OnEventAccepted() {
            throw new System.NotImplementedException();
        }

        public override void StartEvent() {
            throw new System.NotImplementedException();
        }

        protected override void ShowText() {
            throw new System.NotImplementedException();
        }
    }
}
