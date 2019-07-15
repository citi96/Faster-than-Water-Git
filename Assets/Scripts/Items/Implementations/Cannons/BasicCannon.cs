using Items.Interfaces.Cannon;
using Items.Interfaces.CannonBall;
using Managers;
using Ship;
using UnityEngine;

namespace Items.Implementations.Cannons {
    public class BasicCannon : ICannon {
        private const float Accuracy = 40;
        private readonly Random random = new Random();

        public Sprite Sprite => Resources.Load<Sprite>("Sprites/Cannons/BasicCannon");
        public string Name => "Basic Cannon";
        public int ShopPrice => 100;
        public int Level => 0;
        public int Cooldown => 4;
        public float Angle => 10;
        public float ProjectileSpeed => 25;

        public float CheckAccuracy(ICannonBall cannonBall) {
            return Accuracy * cannonBall.AccuracyModifier;
        }

        public void Shoot(CannonBallObject cannonBall, Transform shootTransform, ITarget target, ICannonBall ammo) {
            var objTransform = shootTransform;
            var cannonBallTransform = cannonBall.transform;
            cannonBallTransform.position = objTransform.position;
            cannonBallTransform.rotation = objTransform.rotation;

            
            GameManager.Instance.StartCoroutine(cannonBall.GetComponent<CannonBallObject>().Move(ProjectileSpeed, target, ammo));
        }
    }
}