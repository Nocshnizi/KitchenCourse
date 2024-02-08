using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CutCounter;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangeEventArgs> OnProgressChange;
    public event EventHandler<OnStateChangeEventArgs> OnStateChange;
    public class OnStateChangeEventArgs: EventArgs {
        public State state;
    }

    public enum State {
        Idle,
        Frying,
        Fried,
        Burned,
    }

    [SerializeField] private FryingRecipeSO[] stoveProductSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;
    private float timer;
    private float burnedTimer;
    private FryingRecipeSO stoveProductSO;
    private State state;
    private BurningRecipeSO burningRecipeSO;


    private void Start() {
        state = State.Idle;        
    }



    private void Update() {

        if(HasKitchenObject()) {


        switch (state) {
            case State.Idle:
                break;
            case State.Frying:  
                timer += Time.deltaTime;
                    OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs {
                        progressNormalized = timer / stoveProductSO.cookingProgressMax
                    });
                    if (timer > stoveProductSO.cookingProgressMax) {
                    //fried
                    GetKitchenObject().DestroySelf();

                    KitchenObject.SpawnKitchenObject(stoveProductSO.output, this);
                    state = State.Fried;
                    burnedTimer = 0f;
                    burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                        OnStateChange?.Invoke(this, new OnStateChangeEventArgs {
                            state = state
                        });
                }
                break;
            case State.Fried:
                burnedTimer += Time.deltaTime;
                    OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs {
                        progressNormalized = burnedTimer / burningRecipeSO.burningProgressMax
                    });
                    if (burnedTimer > burningRecipeSO.burningProgressMax) {
                     //burned
                    GetKitchenObject().DestroySelf();

                    KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);

                        Debug.Log("Burned");
                    state = State.Burned;

                        OnStateChange?.Invoke(this, new OnStateChangeEventArgs {
                            state = state
                        });

                        OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs {
                            progressNormalized = 0f
                        });
                    }
                    break;
            case State.Burned:
                break;
        }

            
            Debug.Log(state);
        }
    }

    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            //No KitchenObject here

            if (player.HasKitchenObject()) {
                //Player is carring smth
                if (HasResipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {
                    //Player carring smth, that can be fried
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    stoveProductSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    state = State.Frying;
                    timer = 0f;

                    OnStateChange?.Invoke(this, new OnStateChangeEventArgs {
                        state = state
                    });

                    OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs {
                        progressNormalized = timer / stoveProductSO.cookingProgressMax
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

                        //renew state
                        state = State.Idle;
                        OnStateChange?.Invoke(this, new OnStateChangeEventArgs {
                            state = state
                        });

                        OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs {
                            progressNormalized = 0f
                        });
                    }
                }
            }
            else {
                //player is not carring anithing
                GetKitchenObject().SetKitchenObjectParent(player);
                state = State.Idle;
                OnStateChange?.Invoke(this, new OnStateChangeEventArgs {
                    state = state
                });

                OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs {
                    progressNormalized = 0f
                });
            }
        }
    }


    private bool HasResipeWithInput(KitchenObjectSO kitchenObjectSO) {
        FryingRecipeSO cuttingRecipeSO = GetFryingRecipeSOWithInput(kitchenObjectSO);
        return cuttingRecipeSO != null;

    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {
        FryingRecipeSO cuttingKitchenObjectSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        if (cuttingKitchenObjectSO != null) {
            return cuttingKitchenObjectSO.output;
        }
        else { return null; }
    }


    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (FryingRecipeSO StoveProductSO in stoveProductSOArray) {
            if (StoveProductSO.input == inputKitchenObjectSO) {
                return StoveProductSO;
            }
        }
        return null;
    }   
    
    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray) {
            if (burningRecipeSO.input == inputKitchenObjectSO) {
                return burningRecipeSO;
            }
        }
        return null;
    }

    public bool IsFried() {
        return state == State.Fried;
    }
}
