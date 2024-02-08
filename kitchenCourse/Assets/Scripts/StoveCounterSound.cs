using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{

    [SerializeField] private StoveCounter stoveCounter;
    private AudioSource audioSource;
    private float warningSoundTimer;
    private bool playWarningSound;
    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        stoveCounter.OnStateChange += StoveCounter_OnStateChange;
        stoveCounter.OnProgressChange += StoveCounter_OnProgressChange;

    }

    private void StoveCounter_OnProgressChange(object sender, IHasProgress.OnProgressChangeEventArgs e) {
        
        float burnShowProgressAmount = .5f;

        playWarningSound = e.progressNormalized >= burnShowProgressAmount && stoveCounter.IsFried();
    }

    private void StoveCounter_OnStateChange(object sender, StoveCounter.OnStateChangeEventArgs e) {
        bool playSound = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;

        if(playSound) {
            audioSource.Play();
        }
        else {
            audioSource.Pause();
        }

    }

    private void Update() {
        if (playWarningSound) {
            warningSoundTimer -= Time.deltaTime;

            if(warningSoundTimer <= 0f) {
                float warningSoundTimerMax = .2f;
                warningSoundTimer = warningSoundTimerMax;

                SoundManager.Instance.PlayWarningSound(stoveCounter.transform.position);
            }
        }
        

    }
}
