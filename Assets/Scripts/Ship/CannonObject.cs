using Items.Interfaces.Cannon;
using System.Collections;
using System.Threading;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;
using Timer = System.Timers.Timer;

namespace Ship
{
    public class CannonObject : MonoBehaviour {
        [SerializeField] private Transform piratePosts;
        [SerializeField] private GameObject cannonBall;
        [SerializeField] private Animator cannonFire;
        [SerializeField] private Image progressBar;
        [SerializeField] private Text progressText;

        private ICannon cannon;
        private Coroutine currentCoroutine;
        private bool isBusy;
        private Pirate pirate;
        private static readonly int Fire = Animator.StringToHash("Fire");
        private Timer timer;

        private void Start() {
            timer = new Timer
            {
                Interval = cannon.Cooldown * 1000,
                Enabled = false,
                AutoReset = true
            };

            timer.Elapsed += Shoot;
        }

        public bool IsBusy {
            get => isBusy;
            set {
                isBusy = value;

                if (value) {
                    currentCoroutine = StartCoroutine(FillProgressBar());
                    timer.Enabled = true;
                } else {
                    timer.Enabled = false;
                    StopCoroutine(currentCoroutine);
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

        public bool IsRequestedPirate(Pirate objPirate) {
            return pirate != null && pirate.Equals(objPirate);
        }

        private void Shoot(object source, ElapsedEventArgs e) {
            cannonFire.SetTrigger(Fire);
            var shotBall = Instantiate(cannonBall);
            var objTransform = transform;
            shotBall.transform.position = objTransform.position;
            shotBall.transform.rotation = objTransform.rotation;
            StartCoroutine(shotBall.GetComponent<CannonBall>().Move(cannon.ProjectileSpeed));
            progressBar.fillAmount = 0;
        }

        private IEnumerator FillProgressBar() {
            float milliseconds = cannon.Cooldown * 1000;

            while (milliseconds > 0f) {
                milliseconds -= 1;
                progressBar.fillAmount = (cannon.Cooldown * 1000 - milliseconds) / cannon.Cooldown * 1000;
                progressText.text = (int) (progressBar.fillAmount * 100f) + "%";
                yield return new WaitForSecondsRealtime(0.001f);
            }

            if (IsBusy) {
                currentCoroutine = StartCoroutine(FillProgressBar());
            }
        }
    }
}