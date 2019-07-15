using UnityEngine;

namespace Items {
    public interface IItem {
        Sprite Sprite { get; }
        string Name { get; }
        int ShopPrice { get; }
        int Level { get; }
    }
}