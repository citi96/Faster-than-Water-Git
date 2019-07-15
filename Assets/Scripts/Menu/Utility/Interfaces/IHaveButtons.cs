using UnityEngine.UI;

namespace Menu.Utility.Interfaces {
    public interface IHaveButtons {
        void OnButtonPressed(Button button);
        void OnButtonReleased();
    }
}