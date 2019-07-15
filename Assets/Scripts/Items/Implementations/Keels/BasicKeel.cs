using Items.Interfaces.Keel;
using UnityEngine;

namespace Items.Implementations.Keels {
    class BasicKeel : IKeel {
        public Sprite Sprite => Resources.Load<Sprite>("Sprites/Cannons/BasicKeel");
        public string Name => "Basic Keel";
        public int ShopPrice => -1;
        public int Level => -1;
        public int BaseHealth => 20;
        public int DamageResistance => 2;
        public float PerforationResistance => 0.5f;
    }
}