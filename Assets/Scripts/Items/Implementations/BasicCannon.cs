using System.Timers;
using Items.Interfaces.Cannon;

namespace Items.Implementations {
    public class BasicCannon : ICannon {
        public int ShopPrice => 100;
        public int Level => 0;
        public int Cooldown => 4;
        public int Angle => 10;
        public float ProjectileSpeed => 25;
    }
}