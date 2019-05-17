using UnityEngine;

public class Stairs : MonoBehaviour {
    [SerializeField] private Transform stairsSpot;
    [SerializeField] private Ship.Floor floor;
         
    public Transform StairsSpot => stairsSpot;

    public Ship.Floor Floor => floor;
}
