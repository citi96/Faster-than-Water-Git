using System.Collections.Generic;
using Managers.States.Actions.ActionsFactory;
using Managers.States.Actions.Implementations;
using Managers.States.Actions.Interfaces;

namespace Managers.States {
    public class MapState : State {

        public MapState() {
            actions = new List<IAction> {
                ActionFactory.GetAction<OpenCloseMapAction>(),
                ActionFactory.GetAction<UserInputAction>()
            };
        }
    }
}