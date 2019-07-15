using Items.Implementations.CannonBalls;
using Items.Interfaces.CannonBall;
using System;
using System.Collections.Generic;
using System.Linq;
using Managers;
using UnityEngine;

namespace Ship {
    public class ShipObject : MonoBehaviour {
        public enum Floor {
            Upper,
            Lower
        }

        [SerializeField] private Transform upperSpawn;
        [SerializeField] private Transform lowerSpawn;

        private GameObject upperFloor;
        private GameObject lowerFloor;
        private Floor currentFloor;
        private int cannonAvailable = 2;
        private Vector2 side;

        public Dictionary<ICannonBall, int> CannonBalls { get; private set; }
        public IEnumerable<CannonObject> Cannons { get; private set; } = new CannonObject[6];
        public KeelObject Keel { get; private set; }

        public Floor CurrentFloor {
            get => currentFloor;
            set {
                currentFloor = value;
                SwitchFloor(value);
            }
        }

        public Stairs[] Stairs { get; private set; }

        private void Awake() {
            upperFloor = GameObject.FindGameObjectWithTag("Upper Floor");
            lowerFloor = GameObject.FindGameObjectWithTag("Lower Floor");
            Stairs = GetComponentsInChildren<Stairs>();
        }

        public void InitShip(int cannonBalls) {
            CannonBalls = new Dictionary<ICannonBall, int> {{new BasicCannonBall(), cannonBalls}};
            Cannons = GetComponentsInChildren<CannonObject>();
            foreach (var cannon in Cannons) {
                cannon.ChangeAmmo(CannonBalls.FirstOrDefault().Key);
            }

            Keel = GetComponentInChildren<KeelObject>();
        }

        public Vector2 Side {
            get => side;
            set {
                side = value;
                gameObject.transform.position = new Vector3(side.x, side.y, 1);
            }
        }

        private void SwitchFloor(Floor floor) {
            switch (floor) {
                case Floor.Upper:
                    SetupSwitch(upperFloor, lowerFloor);
                    break;
                case Floor.Lower:
                    SetupSwitch(lowerFloor, upperFloor);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(floor), floor, null);
            }
        }

        private static void SetupSwitch(GameObject roseFloor, GameObject loweredFloor) {
            roseFloor.GetComponent<SpriteRenderer>().sortingOrder = 0;
            loweredFloor.GetComponent<SpriteRenderer>().sortingOrder = -3;
            ChangeFloorChildren(roseFloor, loweredFloor);
        }

        /// <summary>
        /// Scan all the floors' children to change their sorting layer
        /// </summary>
        /// <param name="roseFloor"></param>
        /// <param name="loweredFloor"></param>
        private static void ChangeFloorChildren(GameObject roseFloor, GameObject loweredFloor) {
            foreach (var child in roseFloor.transform.GetComponentsInChildren<SpriteRenderer>()) {
                ChangeFloor(child, 1);
                child.gameObject.transform.position -= Vector3.forward;
            }

            foreach (var child in loweredFloor.transform.GetComponentsInChildren<SpriteRenderer>()) {
                ChangeFloor(child, -2);
                child.gameObject.transform.position += Vector3.forward;
            }
        }

        /// <summary>
        /// Change the floor's children sorting layer
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="floor"></param>
        private static void ChangeFloor(Renderer sprite, int? floor = null) {
            if (floor.HasValue) {
                sprite.sortingOrder = floor.Value;
            } else {
                int sortingOrder = sprite.sortingOrder;
                sortingOrder = sortingOrder == 1 ? -2 : 1;
                sprite.sortingOrder = sortingOrder;
            }
        }

        public void PirateChangeFloor(Pirate pirate, Floor floor) {
            var piratePosition = pirate.transform.localPosition;
            switch (floor) {
                case Floor.Upper:
                    pirate.gameObject.transform.SetParent(lowerFloor.transform);
                    pirate.CurrentGraph = GameManager.Instance.Graphs[1];
                    var lowerSpawnPosition = lowerSpawn.position;
                    pirate.transform.position =
                        new Vector3(lowerSpawnPosition.x, lowerSpawnPosition.y, piratePosition.z);
                    break;
                case Floor.Lower:
                    pirate.gameObject.transform.SetParent(upperFloor.transform);
                    pirate.CurrentGraph = GameManager.Instance.Graphs[0];
                    var upperSpawnPosition = upperSpawn.position;
                    pirate.transform.position =
                        new Vector3(upperSpawnPosition.x, upperSpawnPosition.y, piratePosition.z);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(floor), floor, null);
            }

            pirate.Move(pirate.position, pirate.CurrentGraph);
            ChangeFloor(pirate.gameObject.GetComponent<SpriteRenderer>());
        }
    }
}