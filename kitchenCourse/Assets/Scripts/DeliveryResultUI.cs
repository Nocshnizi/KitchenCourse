using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour
{
    private const string POPUP = "PopUp";

    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI deliveryText;
    [SerializeField] private Color successColor;
    [SerializeField] private Color failureColor;
    [SerializeField] private Sprite successSprite;
    [SerializeField] protected Sprite failureSprite;

    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }


    private void Start() {
        DeliveryManager.Instance.OnRecipeSucceeded += DeliveryManager_OnRecipeSucceeded;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;

       Hide();
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e) {
        animator.SetTrigger(POPUP);
        iconImage.sprite = failureSprite;
        backgroundImage.color = failureColor;
        deliveryText.text = "DELIVERY\nFAILED";
        Show();
    }

    private void DeliveryManager_OnRecipeSucceeded(object sender, System.EventArgs e) {
        animator.SetTrigger(POPUP);
        iconImage.sprite = successSprite;
        backgroundImage.color = successColor;
        deliveryText.text = "DELIVERY\nSUCCESS";
        Show();
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
