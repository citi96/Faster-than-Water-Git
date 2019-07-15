using Items.Implementations.Cannons;
using Items.Interfaces.Cannon;
using Items.Interfaces.CannonBall;
using Managers;
using System.Collections;
using Menu.GameMenu;
using UnityEngine;
using UnityEngine.UI;

namespace Ship
{
    public class CannonObject : MonoBehaviour, ITarget {
        [SerializeField] private Transform piratePosts;
        [SerializeField] private GameObject cannonBall;
        [SerializeField] private Animator cannonFire;
        [SerializeField] private Image progressBar;
        [SerializeField] private Text progressText;

        private ITarget target;

        private Coroutine currentCoroutine;
        private bool isBusy;
        private Pirate pirate;
        private ICannon cannon;
        private static readonly int Fire = Animator.StringToHash("Fire");
        private ICannonBall ammoType;
        private int ammoAmount;

        private ITarget Target {
            get => target ?? GameManager.Instance.EnemyShip.GetComponentInChildren<KeelObject>();
            set => target = value;
        }

        public int AmmoAmount {
            get => ammoAmount;
            set {
                ammoAmount = value;
                if (ammoAmount == 0 && currentCoroutine != null) {
                    StopCoroutine(currentCoroutine);
                    currentCoroutine = null;
                }

                if (isBusy && ammoAmount - 1 == 0 && currentCoroutine == null) {
                    currentCoroutine = StartCoroutine(FillProgressBar());
                }
            }
        }

        public bool IsActive { get; set; } = true;

        public ICannon Cannon {
            get => cannon;
            set {
                cannon = value;
                GetComponent<SpriteRenderer>().sprite = cannon.Sprite;
            }
        }

        public bool IsBusy {
            get => isBusy;
            set {
                isBusy = value;

                if (GameManager.Instance.EnemyShip != null) {
                    if (value && ammoAmount > 0) {
                        currentCoroutine = StartCoroutine(FillProgressBar());
                    } else {
                        if (currentCoroutine != null) {
                            StopCoroutine(currentCoroutine);
                            currentCoroutine = null;
                        }
                    }
                }
            }
        }

        public Pirate Pirate {
            set {
                if (pirate != null && value != null && !IsRequestedPirate(value)) {
                    pirate.Move(pirate.transform.position, pirate.CurrentGraph);
                }

                pirate = value;
            }
        }

        public Transform PiratePosts => piratePosts;

        private void Start() {
            Cannon = new BasicCannon();
        }

        public bool IsRequestedPirate(Pirate objPirate) {
            return pirate != null && pirate.Equals(objPirate);
        }

        private void Shoot() {
            cannonFire.SetTrigger(Fire);
            var shotBall = Instantiate(cannonBall);
            cannon.Shoot(shotBall.GetComponent<CannonBallObject>(), transform, Target, ammoType);
            CannonsUIManager.Instance.UseAmmo(this);
            progressBar.fillAmount = 0;
        }

        private IEnumerator FillProgressBar() {
            float elapsedCooldown = cannon.Cooldown;

            while (elapsedCooldown > 0f) {
                elapsedCooldown -= Time.unscaledDeltaTime;
                progressBar.fillAmount = (cannon.Cooldown - elapsedCooldown) / (cannon.Cooldown);
                progressText.text = (int) (progressBar.fillAmount * 100f) + "%";
                yield return null;
            }

            if (IsBusy) {
                currentCoroutine = StartCoroutine(FillProgressBar());
                Shoot();
            }
        }

        public void ApplyDamage(ICannonBall cannonBall1) {
            throw new System.NotImplementedException();
        }

        public void ChangeAmmo(ICannonBall ball) {
            ammoType = ball;
        }
    }
}