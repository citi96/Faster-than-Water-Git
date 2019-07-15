using Managers.States.Actions.Interfaces;
using UnityEngine;

namespace Managers.States.Actions.Implementations {
    public class OpenCloseMapAction : IAction {
        public void Execute() {
            if (Input.GetKeyDown(KeyCode.M)) {
                var map = Map.Map.Instance;
                map.HideShowMapUi(!map.IsMapUiActive());
            }
        }
    }
}