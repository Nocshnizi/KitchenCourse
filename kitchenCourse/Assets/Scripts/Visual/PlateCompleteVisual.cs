using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSO_GameObject {
        public GameObject gameObject;
        public KitchenObjectSO kitchenObjectSO;
    }

    [SerializeField] private List<KitchenObjectSO_GameObject> kitchenObjectSOGameObjectList;

    [SerializeField] private PlatesKitchenObject platesKitchenObject;

    private void Start() {
        platesKitchenObject.OnIngridientAdded += PlatesKitchenObject_OnIngridientAdded;

        foreach (KitchenObjectSO_GameObject KitchenObjectSO_GameObject in kitchenObjectSOGameObjectList) {
                KitchenObjectSO_GameObject.gameObject.SetActive(false);
        }

    }

    private void PlatesKitchenObject_OnIngridientAdded(object sender, PlatesKitchenObject.OnIngridientAddedEventArgs e) {
        foreach(KitchenObjectSO_GameObject KitchenObjectSO_GameObject in kitchenObjectSOGameObjectList) {
            if(KitchenObjectSO_GameObject.kitchenObjectSO == e.kitchenObjectSO) {
                KitchenObjectSO_GameObject.gameObject.SetActive(true);
            }
        }
        //e.kitchenObjectSO
    }
}
