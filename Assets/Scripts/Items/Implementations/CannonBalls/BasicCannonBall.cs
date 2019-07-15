using Items.Interfaces.CannonBall;
using Ship;
using UnityEngine;

namespace Items.Implementations.CannonBalls {
    public class BasicCannonBall : ICannonBall {
        public Sprite Sprite => Resources.Load<Sprite>("Sprites/Cannons/BasicCannonBall");
        public string Name => "Basic Cannon Ball";
        public int ShopPrice => 10;
        public int Level => 0;
        public int Damage => 5;
        public float AccuracyModifier => 0;

        public void Bonus(ITarget hitTarget) { }
    }
}