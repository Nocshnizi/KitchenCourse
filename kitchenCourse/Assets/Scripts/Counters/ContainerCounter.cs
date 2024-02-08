using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter, IKitchenObjectParent {

    public event EventHandler OnPlayerGrabObject;

    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    //private KitchenObject kitchenObject;

    public override void Interact(Player player) {
        Debug.Log("Interact");

        //Instantiate = create object on scene
        if (!player.HasKitchenObject()) {
            //Player is not carring any object

            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);

            OnPlayerGrabObject?.Invoke(this, EventArgs.Empty);
        }
        else {

        }

    }

}
