using UnityEngine;

namespace Ship {
    public class Stairs : MonoBehaviour {
        [SerializeField] private Transform stairsSpot;
        [SerializeField] private ShipObject.Floor floor;
         
        public Transform StairsSpot => stairsSpot;

        public ShipObject.Floor Floor => floor;
    }
}
