using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.UI;

public class Cannon : MonoBehaviour {
    [SerializeField] private Transform piratePosts;
    [SerializeField] private GameObject cannonBall;
    [SerializeField] private Animator cannonFire;
    [SerializeField] private Image progressBar;
    [SerializeField] private Text progressText;

    private Coroutine currentCoroutine = null;
    private bool isBusy;
    private Pirate pirate;
    private static readonly int Fire = Animator.StringToHash("Fire");

    public bool IsBusy {
        get => isBusy;
        set {
            isBusy = value;

            if (value) {
                currentCoroutine = StartCoroutine(FillProgressBar());
            } else {
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

    private void Shoot() {
        cannonFire.SetTrigger(Fire);
        var shotBall = Instantiate(cannonBall);
        var objTransform = transform;
        shotBall.transform.position = objTransform.position;
        shotBall.transform.rotation = objTransform.rotation;
        StartCoroutine(shotBall.GetComponent<CannonBall>().move());
        progressBar.fillAmount = 0;
    }

    private IEnumerator FillProgressBar() {
        float time = progressBar.fillAmount * 1f;
        while (time < 1f) {
            time += 0.01f;
            progressBar.fillAmount = time;
            progressText.text = (int) (progressBar.fillAmount * 100f) + "%";
            yield return new WaitForSecondsRealtime(0.01f);
        }

        if (IsBusy) {
            Shoot();
            currentCoroutine = StartCoroutine(FillProgressBar());
        }
    }
}