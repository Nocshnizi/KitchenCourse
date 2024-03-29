using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    private const string NUMBER_POPUP = "NumberPopup";

    [SerializeField] private TextMeshProUGUI countdownText;

    private Animator animator;
    private int previousCountdownNumber;

    private void Start() {
        GameManager.Instance.OnStaeChange += GameManager_OnStaeChange; ;


        Hide();
    }

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void GameManager_OnStaeChange(object sender, System.EventArgs e) {
        if(GameManager.Instance.IsCountdownStartActive()) {
            Show();
        }
        else {
            Hide();
        }
    }

    private void Update() {
        int countdownNumber = Mathf.CeilToInt(GameManager.Instance.GetCountdownToStartTimer());
        countdownText.text = countdownNumber.ToString();

        if(previousCountdownNumber !=  countdownNumber ) {
            previousCountdownNumber = countdownNumber;
            animator.SetTrigger(NUMBER_POPUP);

            SoundManager.Instance.PlayCountdownSound();
        }
    }


    private void Hide() {
        gameObject.SetActive(false);
    }

    private void Show() {
        gameObject.SetActive(true);
    }


}
