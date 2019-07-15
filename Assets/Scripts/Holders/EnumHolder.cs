using UnityEngine;
using Random = System.Random;

namespace Holders {
    /// <summary>
    /// Represents starting and ending point of a quest.
    /// </summary>
    public enum QuestType {
        Island,
        Sea
    }

    /// <summary>
    /// The Side of the enemy inside a battle.
    /// </summary>
    public static class EnemySide {
        /// <summary>
        /// Enemy left, Player Right.
        /// </summary>
        public static readonly Vector2 Left = new Vector2(-15, 5);

        /// <summary>
        /// Enemy right, Player Left.
        /// </summary>
        public static readonly Vector2 Right = new Vector2(15, 5);

        /// <summary>
        /// Return the Side randomly chose between the two possibilities.
        /// </summary>
        public static Vector2 Side {
            get {
                int random = new Random().Next(2);
                return random == 0 ? Left : Right;
            }
        }
    }
}