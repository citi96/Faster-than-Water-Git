namespace Items.Interfaces.Cannon
{
    public interface ICannon : IItem {
        int Cooldown { get; }
        int Angle { get; }
        float ProjectileSpeed { get; }
    }
}