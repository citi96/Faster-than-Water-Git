using Items.Interfaces.CannonBall;
using Ship;
using UnityEngine;

namespace Items.Interfaces.Cannon
{
    public interface ICannon : IItem {
        int Cooldown { get; }
        float Angle { get; }
        float ProjectileSpeed { get; }

        float CheckAccuracy(ICannonBall cannonBall);
        void Shoot(CannonBallObject cannonBall, Transform shootTransform, ITarget target, ICannonBall ball);
    }
}