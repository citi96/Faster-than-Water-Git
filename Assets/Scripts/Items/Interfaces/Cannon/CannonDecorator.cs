using System.Timers;
using Items.Interfaces.CannonBall;
using Ship;
using UnityEngine;

namespace Items.Interfaces.Cannon
{
    public abstract class CannonDecorator : ICannon {
        public abstract Sprite Sprite { get; }
        public abstract string Name { get; }
        public abstract int ShopPrice { get; }
        public abstract int Level { get; }
        public abstract int Cooldown { get; }
        public abstract float Angle { get; }
        public abstract float ProjectileSpeed { get; }

        protected ICannon cannon;

        public CannonDecorator(ICannon cannon) {
            this.cannon = cannon;
        }

        public float CheckAccuracy(ICannonBall cannonBall) {
            return cannon.CheckAccuracy(cannonBall);
        }

        public void Shoot(CannonBallObject cannonBall, Transform shootTransform, ITarget target, ICannonBall ammo) {
            cannon.Shoot(cannonBall, shootTransform, target, ammo);
        }
    }
}