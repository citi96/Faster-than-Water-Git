using System.Collections.Generic;
using Managers.States.Actions.ActionsFactory;
using Managers.States.Actions.Implementations;
using Managers.States.Actions.Interfaces;

namespace Managers.States {
    public class BattleState : State {
        public BattleState() {
            actions = new List<IAction> {
                ActionFactory.GetAction<MovePirateDuringEventAction>(),
                ActionFactory.GetAction<UserInputAction>()
            };
        }
    }
}