using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesKitchenObject : KitchenObject
{
    public event EventHandler<OnIngridientAddedEventArgs> OnIngridientAdded;
    public class OnIngridientAddedEventArgs: EventArgs {
        public KitchenObjectSO kitchenObjectSO;
    }

    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSO;

    private List<KitchenObjectSO> kitchenObjectSOList;

    private void Awake() {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngridient(KitchenObjectSO kitchenObjectSO) {
        if (!validKitchenObjectSO.Contains(kitchenObjectSO)) {
            //not a valid ingridient
            return false;
        }
        if (kitchenObjectSOList.Contains(kitchenObjectSO)) {
            //already has an object, do not to duplicate it
            return false;
        }
        else {
            //list hasnt this object, add to list
            kitchenObjectSOList.Add(kitchenObjectSO);

            OnIngridientAdded?.Invoke(this, new OnIngridientAddedEventArgs {
                kitchenObjectSO = kitchenObjectSO
            });

            return true;
        }
        
    }

    public List<KitchenObjectSO> GetKitchenObjestSOList() {
        return kitchenObjectSOList;
    }
}
