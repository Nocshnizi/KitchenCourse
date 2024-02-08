using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectParent kitchenObjectParent;

    public KitchenObjectSO GetKitchenObjectSO() { return kitchenObjectSO; }


    public void SetKitchenObjectParent(IKitchenObjectParent clearCounter) {
        if (this.kitchenObjectParent != null) {
            this.kitchenObjectParent.ClearKitchenObject();
        }
        this.kitchenObjectParent = clearCounter;

        if (clearCounter.HasKitchenObject()) {
            Debug.LogError("Counter already has an object");
        }

        clearCounter.SetKitchenObject(this); 

        transform.parent = clearCounter.GetKitchenFollowTransform();
        transform.localPosition = Vector3.zero;
    }


    public IKitchenObjectParent GetKitchenObjectParent() {
        return kitchenObjectParent;
    }

    public void DestroySelf() {
        kitchenObjectParent.ClearKitchenObject() ;

        Destroy(gameObject);
    }

    public bool TryGetPlate(out PlatesKitchenObject platesKitchenObject) {
        if(this is PlatesKitchenObject) {
            platesKitchenObject = this as PlatesKitchenObject;
            return true;
        }
        else {
            platesKitchenObject = null;
            return false;
        }

    }

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent) {
        Transform kitchenObjTransform = Instantiate(kitchenObjectSO.prefab);
        KitchenObject kitchenObject = kitchenObjTransform.GetComponent<KitchenObject>();
        
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);

        return kitchenObject;
    }
}
