using Managers;
using UnityEngine;

namespace Menu.GameMenu.LowerBar.CannonsManagement
{
    public class CannonsManagementUI : MonoBehaviour {
        [SerializeField] private GameObject cannonManagementSlotPrefab;
        [SerializeField] private Transform container;

        private void Awake() {
            foreach (var cannon in GameManager.Instance.Ship.Cannons) {
                var cannonSlot = Instantiate(cannonManagementSlotPrefab, container).GetComponent<CannonSlot>();
                cannonSlot.Init(cannon.Cannon);
            }
        }
    }
}