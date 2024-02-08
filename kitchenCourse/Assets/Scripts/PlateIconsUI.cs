using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconsUI : MonoBehaviour
{
    [SerializeField] private PlatesKitchenObject platesKitchenObject;
    [SerializeField] private Transform iconTemplate;


    private void Awake() {
        iconTemplate.gameObject.SetActive(false);
    }

    private void Start() {
        platesKitchenObject.OnIngridientAdded += PlatesKitchenObject_OnIngridientAdded;
    }

    private void PlatesKitchenObject_OnIngridientAdded(object sender, PlatesKitchenObject.OnIngridientAddedEventArgs e) {
        UpdateVisual();
    }


    private void UpdateVisual() {   
        foreach (Transform child in transform) {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach(KitchenObjectSO kitchenObjectSO in platesKitchenObject.GetKitchenObjestSOList()) {
            Transform iconTransform = Instantiate(iconTemplate, transform);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<PlateIconSingleUI>().SetKitchenObjectSO(kitchenObjectSO);
        }
    }
}
