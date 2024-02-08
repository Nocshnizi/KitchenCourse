using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBar : MonoBehaviour
{
    private enum Mode {
        LookAt,
        LookAtInverted,
        CameraForward,
        CameraFowardInverted,
    }

    [SerializeField] private Mode mode;

    private void LateUpdate() {
        switch (mode) {
            case Mode.LookAt:
                transform.LookAt(Camera.main.transform);
                break;
            case Mode.LookAtInverted:
                Vector3 dirFromVamera = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + dirFromVamera);
                break;
            case Mode.CameraForward:
                transform.forward = Camera.main.transform.forward;
                break;
            case Mode.CameraFowardInverted:
                transform.forward = -Camera.main.transform.forward;
                break;
        }

    }

}
