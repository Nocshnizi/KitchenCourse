using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter, IKitchenObjectParent
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
   


    public override void Interact(Player player) {
        Debug.Log("Interact");
        if (!HasKitchenObject()) {
            //No KitchenObject here

            if (player.HasKitchenObject()) {
                //Player is carring smth
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else {
                //player has nothing
            }
        }
        else {
            if (player.HasKitchenObject()) {
                //player is carring smth
                if(player.GetKitchenObject().TryGetPlate(out PlatesKitchenObject platesKitchenObject)) { 
                    //and if he is carring the plate 

                    //if we can add ingridient to the plate
                    if (platesKitchenObject.TryAddIngridient(GetKitchenObject().GetKitchenObjectSO())) {

                        //destroy from clear counter and move to the plate
                        GetKitchenObject().DestroySelf();
                    }
                }else {
                    //player not carring plate

                    if(GetKitchenObject().TryGetPlate(out platesKitchenObject)) {
                        //plate is on claer counter
                        if (platesKitchenObject.TryAddIngridient(player.GetKitchenObject().GetKitchenObjectSO())) {
                            //destroy from clear counter and move to the plate
                            player.GetKitchenObject().DestroySelf();
                        }

                    }
                }

            }
            else {
                //player is not carring anithing
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }


    }
    

    
}
