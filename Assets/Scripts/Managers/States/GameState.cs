using System.Collections.Generic;
using Managers.States.Actions.ActionsFactory;
using Managers.States.Actions.Implementations;
using Managers.States.Actions.Interfaces;

namespace Managers.States {
    public class GameState : State {
        public GameState() {
            actions = new List<IAction> {
                ActionFactory.GetAction<MovePirateDuringEventAction>(),
                ActionFactory.GetAction<OpenCloseMapAction>(),
                ActionFactory.GetAction<UserInputAction>()
            };
        }
    }
}