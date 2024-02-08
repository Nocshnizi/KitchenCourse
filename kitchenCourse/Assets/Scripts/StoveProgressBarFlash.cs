using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveProgressBarFlash : MonoBehaviour
{
    private const string IS_FLASHING = "IsFlashing";
    [SerializeField] StoveCounter stoveCounter;

    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        
        stoveCounter.OnProgressChange += StoveCounter_OnProgressChange;

        animator.SetBool(IS_FLASHING, false);
    }

    private void StoveCounter_OnProgressChange(object sender, IHasProgress.OnProgressChangeEventArgs e) {
        float burnShowProgressAmount = .5f;

        bool show = e.progressNormalized >= burnShowProgressAmount && stoveCounter.IsFried();

        animator.SetBool(IS_FLASHING, show);
    }

}
