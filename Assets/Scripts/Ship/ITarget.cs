using Items.Interfaces.CannonBall;

namespace Ship {
    public interface ITarget {
        void ApplyDamage(ICannonBall cannonBall);
    }
}