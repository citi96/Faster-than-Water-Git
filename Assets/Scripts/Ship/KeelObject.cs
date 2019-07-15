using Items.Implementations.Keels;
using Items.Interfaces.CannonBall;
using Items.Interfaces.Keel;
using UnityEngine;

namespace Ship {
    public class KeelObject : MonoBehaviour, ITarget {
        private IKeel keel;

        public int Health { get; private set; }

        public IKeel Keel {
            set {
                keel = value;
                GetComponent<SpriteRenderer>().sprite = keel.Sprite;
            }
        }

        private void Start() {
            Keel = new BasicKeel();
            Health = keel.BaseHealth;
        }

        public void ApplyDamage(ICannonBall cannonBall) {
            int damage = cannonBall.Damage - keel.DamageResistance;
            if (damage > 0) {
                Health -= damage;
            }
        }
    }
}