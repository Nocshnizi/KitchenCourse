using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnWarningUI : MonoBehaviour
{
    [SerializeField] StoveCounter stoveCounter;


    private void Start() {
        stoveCounter.OnProgressChange += StoveCounter_OnProgressChange;

        Hide();
    }

    private void StoveCounter_OnProgressChange(object sender, IHasProgress.OnProgressChangeEventArgs e) {
        float burnShowProgressAmount = .5f;

        bool show = e.progressNormalized >= burnShowProgressAmount && stoveCounter.IsFried();

        if (show) {
            Show();
        }
        else {
            Hide();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
