using Menu.Utility.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Menu.Utility {
    public class ButtonPressed : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
        [SerializeField] private Button button;

        private bool pressed;
        private float holdTime;

        public void OnPointerDown(PointerEventData eventData) {
            if (eventData.button == PointerEventData.InputButton.Left) {
                pressed = true;
            }
        }

        public void OnPointerUp(PointerEventData eventData) {
            if (eventData.button == PointerEventData.InputButton.Left) {
                GetComponentInParent<IHaveButtons>().OnButtonReleased();
            }
        }

        private void Update() {
            if (pressed) {
                holdTime += Time.unscaledDeltaTime;
                if (holdTime > 0.3) {
                    GetComponentInParent<IHaveButtons>().OnButtonPressed(button);
                    pressed = false;
                    holdTime = 0f;
                }
            }
        }
    }
}