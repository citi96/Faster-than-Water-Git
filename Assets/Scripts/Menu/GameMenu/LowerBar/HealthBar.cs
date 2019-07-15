using Managers;
using Ship;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Menu.GameMenu.LowerBar
{
    public class HealthBar : MonoBehaviour {
        [SerializeField] private Image healthBar;
        [SerializeField] private ShipObject ship;

        private int maxHealth;

        private void Awake() {
            StartCoroutine(Init());
        }

        private IEnumerator Init() {
            while (ship.Keel == null) {
                yield return new WaitForSeconds(0.1f);
            }

            if (ship == null || ship != GameManager.Instance.Ship) {
                ship = GameManager.Instance.EnemyShip;
            }

            maxHealth = ship.Keel.Health;
        }

        private void Update() {
            if (ship.Keel != null) {
                //healthBar.fillAmount = maxHealth / (float)ship.Keel.Health;
                healthBar.color = Color.Lerp(Color.red, Color.green, healthBar.fillAmount);
            }
        }
    }
}