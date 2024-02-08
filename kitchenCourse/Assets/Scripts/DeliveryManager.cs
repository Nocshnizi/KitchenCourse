using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{

    public event EventHandler OnRecipeSpawn;
    public event EventHandler OnRecipeCompleted;

    public event EventHandler OnRecipeFailed;
    public event EventHandler OnRecipeSucceeded;

    public static DeliveryManager Instance { get; private set; }
    [SerializeField] private RecipeListSO recipeListSO;
    private List<RecipeSO> waitingRecipeList;

    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipeMax = 4;
    private int successfulRecipe = 0;

    private void Awake() {
        Instance = this;
        waitingRecipeList = new List<RecipeSO>();
    }

    private void Update() {
        spawnRecipeTimer -= Time.deltaTime;
        if(spawnRecipeTimer <= 0f) {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if(GameManager.Instance.IsGamePlaying() && waitingRecipeList.Count < waitingRecipeMax) {
                RecipeSO waitnigRecipeSO =  recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                
                waitingRecipeList.Add(waitnigRecipeSO);

                OnRecipeSpawn?.Invoke(this, EventArgs.Empty);
            }

        }
    }

    public void DeliverRecipe(PlatesKitchenObject platesKitchenObject) {
        for(int i = 0; i < waitingRecipeList.Count; i++) {
            RecipeSO waitingRecipeSO = waitingRecipeList[i];
                
            if(waitingRecipeSO.kitchenObjectSO.Count == platesKitchenObject.GetKitchenObjestSOList().Count) {
                //has the same number of ingridient
                bool plateContentsMatchesRecipe = true;
                foreach(KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSO) {
                    //cycling all ingridient in the recipe
                    bool ingridientMatch = false;
                    foreach (KitchenObjectSO plateKitchenObjectSO in platesKitchenObject.GetKitchenObjestSOList()) {
                        //cycling all ingridient in the plate
                        if(plateKitchenObjectSO == recipeKitchenObjectSO) {
                            //Ingridient does match
                            ingridientMatch = true;
                            break;
                        }


                    }
                    if(!ingridientMatch) {
                        //this recipe was not found on the plate
                        plateContentsMatchesRecipe = false;
                    }

                }
                if(plateContentsMatchesRecipe) {
                    // player deliver correct recipe
                    
                    waitingRecipeList.RemoveAt(i);
                   
                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSucceeded?.Invoke(this, EventArgs.Empty);

                    successfulRecipe++;
                    return;
                }
            }
            
            

        }
        //No mathes found
            //Player did not make right delivery
        
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);

       
    }

    public List<RecipeSO> GetWaitingRecipeList() {
        return waitingRecipeList;
    }

    public int GetSuccessfulRecipies() {
        return successfulRecipe;
    }

    

}
