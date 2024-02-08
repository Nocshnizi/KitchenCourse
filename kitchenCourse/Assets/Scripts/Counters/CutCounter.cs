using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutCounter : BaseCounter, IHasProgress
{

    [SerializeField] private CuttingRecipeSO[] cutKitchenObjectSOArray;
    public event EventHandler<IHasProgress.OnProgressChangeEventArgs> OnProgressChange;
    public int cuttingProgress;

    public event EventHandler OnCut;

    public static event EventHandler OnAnyCut;

    public static void ResetStaticData() {
        OnAnyCut = null;
    }

    //private KitchenObject kitchenObject;
    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            //No KitchenObject here

            if (player.HasKitchenObject()) {
                //Player is carring smth
                if (HasResipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {
                    //Player carring smth, that can be cut
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;
                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                    OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs {
                        progressNormalized = (float)cuttingProgress/cuttingRecipeSO.cuttingProgressMax,
                    });
                }
                
                
            }
            else {
                //player has nothing
            }
        }
        else {
            if (player.HasKitchenObject()) {
                //player is carring smth
                if (player.GetKitchenObject().TryGetPlate(out PlatesKitchenObject platesKitchenObject)) {
                    //and if he is carring 

                    //if we can add ingridient to the plate
                    if (platesKitchenObject.TryAddIngridient(GetKitchenObject().GetKitchenObjectSO())) {

                        //destroy from clear counter and move to the plate
                        GetKitchenObject().DestroySelf();
                    }
                }
            }
            else {
                //player is not carring anithing
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }


    public override void InteractAlternate(Player player) {
        if(HasKitchenObject() && HasResipeWithInput(GetKitchenObject().GetKitchenObjectSO())) {
            //There is a KitchenObject here
            cuttingProgress += 1;

            OnCut?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);

            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

            OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax,
            });
            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax) {
                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());

                GetKitchenObject().DestroySelf();
          
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }
    
           

        }
    }

    private bool HasResipeWithInput(KitchenObjectSO kitchenObjectSO) {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(kitchenObjectSO);
        return cuttingRecipeSO != null;

    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) { 
        CuttingRecipeSO cuttingKitchenObjectSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        if(cuttingKitchenObjectSO != null) {
            return cuttingKitchenObjectSO.output;
        }
        else { return null; }
    }


    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (CuttingRecipeSO cuttingRecipeSO in cutKitchenObjectSOArray) {
            if (cuttingRecipeSO.input == inputKitchenObjectSO) {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}
