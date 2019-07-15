using Menu.GameMenu.ElementsUI;
using Ship;
using UnityEngine;
using UnityEngine.UI;

namespace Menu.GameMenu {
    public class CannonsUIManager : MonoBehaviour {
        [SerializeField] private GameObject[] cannonUiPrefab = new GameObject[6];
        [SerializeField] private Text ammoStorageText;

        private ShipObject ship;
        private int freeCannonBalls;

        public static CannonsUIManager Instance { get; private set; }

        private void Start() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(this);
            }

            ship = GameObject.FindGameObjectWithTag("Ship").GetComponent<ShipObject>();
            SetupPrefabs();
            SetupCannonBalls();
        }

        private void SetupCannonBalls() {
            // TODO: TEMPORARY.
            foreach (var ball in ship.CannonBalls) {
                freeCannonBalls += ball.Value;
            }

            UpdateAmmoStorage();
        }

        private void SetupPrefabs() {
            for (int i = 0; i < ship.Cannons.Length; i++) {
                cannonUiPrefab[i].GetComponent<CannonUI>().Cannon = ship.Cannons[i];
                if (!ship.Cannons[i].IsActive) {
                    cannonUiPrefab[i].SetActive(false);
                }
            }
        }

        public int TakeCannonBall() {
            if (freeCannonBalls > 0) {
                freeCannonBalls--;
                UpdateAmmoStorage();
                return 1;
            }

            return 0;
        }

        public void ReturnCannonBall(bool consume) {
            if (!consume) {
                freeCannonBalls++;
                UpdateAmmoStorage();
            }
        }

        public void UseAmmo(CannonObject cannon) {
            foreach (var cannonObject in cannonUiPrefab) {
                if (cannonObject.GetComponent<CannonUI>().Cannon.Equals(cannon)) {
                    cannonObject.GetComponent<CannonUI>().OnRemoveAmmo(true);
                    break;
                }
            }
        }

        private void UpdateAmmoStorage() {
            ammoStorageText.text = freeCannonBalls.ToString();
        }
    }
}