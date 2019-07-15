using Holders;
using Managers.States;
using Pathfinding;
using Ship;
using System.Collections;
using UnityEngine;

namespace Managers {
    /// <inheritdoc />
    /// <summary>
    /// The active manager during the game play.
    /// </summary>
    public class GameManager : MonoBehaviour {
        [SerializeField] private GameObject battleUI;

        private ShipObject enemyShip;
        private GameState gameState;
        private State state;


        public State State {
            get => state;
            set {
                if (value == null) {
                    value = FindCurrentState();
                }

                state = value;
            }
        }

        public ShipObject Ship { get; private set; }

        /// <summary>
        /// Getter for the only active instance of the GameManager.
        /// </summary>
        public static GameManager Instance { get; private set; }

        /// <summary>
        /// Return all the GraphMasks in the game scene inside an array.
        /// Graphs[0] = UpperFloor
        /// Graphs[1] = LowerFloor
        /// </summary>
        public GraphMask[] Graphs { get; private set; }

        /// <summary>
        /// Currently selected player
        /// </summary>
        public Pirate SelectedPirate { get; set; }

        /// <summary>
        /// The enemy ship. Null when not in combat. It also set the ship side according
        /// to the EnemyEvent side before the begin of the event.
        /// Eventually it shows and hides the battle ui.
        /// </summary>
        public ShipObject EnemyShip {
            get => enemyShip;
            set {
                enemyShip = value;
                Ship.Side = enemyShip.Side == EnemySide.Left ? EnemySide.Right : EnemySide.Left;
                StartCoroutine(ScanGraph());

                ShowBattleUI(value != null);
            }
        }

        private void ShowBattleUI(bool active) {
            battleUI.SetActive(active);
        }

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(this);
            }

            gameState = new GameState();
            State = gameState;
            Ship = GameObject.FindGameObjectWithTag("Ship").GetComponent<ShipObject>();
            Ship.InitShip(20);
            Graphs = new[] {
                GraphMask.FromGraphName("Upper Floor"),
                GraphMask.FromGraphName("Lower Floor")
            };
        }

        private void Update() {
            State.Tick();

            if (!Map.Map.Instance.IsMapUiActive()) { }
        }

        private static IEnumerator ScanGraph() {
            yield return new WaitForSecondsRealtime(0.1f);
            AstarPath.active.Scan();
        }

        private State FindCurrentState() {
            return gameState;
        }
    }
}