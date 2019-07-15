using Ship;

namespace Items.Interfaces.CannonBall {
    public interface ICannonBall : IItem {
        int Damage { get; }
        float AccuracyModifier { get; }

        void Bonus(ITarget hitTarget);
    }
}