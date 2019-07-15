namespace Items.Interfaces.Keel {
    public interface IKeel : IItem {
        int BaseHealth { get; }
        int DamageResistance { get; }
        float PerforationResistance { get; }
    }
}