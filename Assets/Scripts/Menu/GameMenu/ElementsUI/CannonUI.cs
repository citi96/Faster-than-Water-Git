using Ship;
using System.Collections;
using Menu.Utility.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Menu.GameMenu.ElementsUI {
    public class CannonUI : MonoBehaviour, IHaveButtons {
        [SerializeField] private Text ammoAmount;
        [SerializeField] private Button leftArrow;

        private Coroutine currentCoroutine;

        public CannonObject Cannon { get; set; }

        public void OnButtonPressed(Button button) {
            if (button == leftArrow) {
                OnRemoveAmmo(false);
            } else {
                OnAddAmmo();
            }
        }

        public void OnButtonReleased() {
            if (currentCoroutine != null) {
                StopCoroutine(currentCoroutine);
                currentCoroutine = null;
            }
        }

        private void OnAddAmmo() {
            if (currentCoroutine == null) {
                currentCoroutine = StartCoroutine(AddAmmoCoroutine());
            }
        }

        private IEnumerator AddAmmoCoroutine() {
            do {
                Cannon.AmmoAmount += CannonsUIManager.Instance.TakeCannonBall();
                ammoAmount.text = Cannon.AmmoAmount.ToString();
                yield return new WaitForSecondsRealtime(0.15f);
            } while (Input.GetMouseButton(0));

            currentCoroutine = null;
        }

        public void OnRemoveAmmo(bool consume) {
            if (currentCoroutine == null) {
                currentCoroutine = StartCoroutine(RemoveAmmoCoroutine(consume));
            }
        }

        private IEnumerator RemoveAmmoCoroutine(bool consume) {
            if (Cannon.AmmoAmount > 0) {
                do {
                    Cannon.AmmoAmount--;
                    ammoAmount.text = Cannon.AmmoAmount.ToString();
                    CannonsUIManager.Instance.ReturnCannonBall(consume);
                    yield return new WaitForSecondsRealtime(0.15f);
                } while (Cannon.AmmoAmount > 0 && Input.GetMouseButton(0));
            }

            currentCoroutine = null;
        }
    }
}