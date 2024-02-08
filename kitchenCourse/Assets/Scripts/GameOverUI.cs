using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipiesDeliveredText;
    [SerializeField] private Button playAgainButton;


    private void Awake() {
        playAgainButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.GameScene);
        });
    }

    private void Start() {
        GameManager.Instance.OnStaeChange += GameManager_OnStaeChange; 

        Hide();
    }

    private void GameManager_OnStaeChange(object sender, System.EventArgs e) {
        if (GameManager.Instance.IsGameOver()) {
            Show();
            recipiesDeliveredText.text = DeliveryManager.Instance.GetSuccessfulRecipies().ToString();
        }
        else {
            Hide();
        }
    }



    private void Hide() {
        gameObject.SetActive(false);
    }

    private void Show() {
        gameObject.SetActive(true);
    }
}
