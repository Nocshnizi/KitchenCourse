using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetDataManager : MonoBehaviour
{
    private void Awake() {
        CutCounter.ResetStaticData();
        BaseCounter.ResetStaticData();
        TrashCounter.ResetStaticData();
    }
}
