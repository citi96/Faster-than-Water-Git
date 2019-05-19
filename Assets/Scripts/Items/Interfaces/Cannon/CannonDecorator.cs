using System.Timers;

namespace Items.Interfaces.Cannon
{
    public abstract class CannonDecorator : ICannon {
        public abstract int ShopPrice { get; }
        public abstract int Level { get; }
        public abstract int Cooldown { get; }
        public abstract int Angle { get; }
        public abstract float ProjectileSpeed { get; }

        protected ICannon cannon;

        public CannonDecorator(ICannon cannon) {
            this.cannon = cannon;
        }
    }
}