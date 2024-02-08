using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private const string PLAYER_PREFS_BINDINGS = "InputBindings";

    public static GameInput Instance { get; private set; }

    public enum Binding {
        Move_Up,
        Move_Down, 
        Move_Left,
        Move_Right,
        Interact,
        InteractAlt,
        Pause
    }

    public event EventHandler OnIneractAction;
    public event EventHandler OnIneractAlternateAction;
    public event EventHandler OnPauseAction;

    public event EventHandler OnBindingRebind;

    private PlayerInputSystem playerInputSystem;

    private void Awake() {
        Instance = this;

        playerInputSystem = new PlayerInputSystem();
        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS)) {
                   playerInputSystem.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
                }

        playerInputSystem.Player.Enable();

        playerInputSystem.Player.Interact.performed += Interact_performed;
        playerInputSystem.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInputSystem.Player.Pause.performed += Pause_performed;

        
    }

    private void OnDestroy() {
        playerInputSystem.Player.Interact.performed -= Interact_performed;
        playerInputSystem.Player.InteractAlternate.performed -= InteractAlternate_performed;
        playerInputSystem.Player.Pause.performed -= Pause_performed;

        playerInputSystem.Dispose();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnIneractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        /*
         * if(OnIneractAction != null){
         *      OnIneractAction(this, EventArgs.Empty);
         * }
         * same as 
         */
        OnIneractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormolized() {
        Vector2 inputVector = playerInputSystem.Player.Move.ReadValue<Vector2>();
        
        inputVector = inputVector.normalized;

       return inputVector;
    }

    public string GetBindingText(Binding binding) {
        switch(binding) {
            default:
            case Binding.Interact:
                return playerInputSystem.Player.Interact.bindings[0].ToDisplayString();
            case Binding.InteractAlt:
                return playerInputSystem.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return playerInputSystem.Player.Pause.bindings[0].ToDisplayString();
            case Binding.Move_Up:
                return playerInputSystem.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return playerInputSystem.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_Left:
                return playerInputSystem.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return playerInputSystem.Player.Move.bindings[4].ToDisplayString();

        }
    }

    public void RebindBinding(Binding binding, Action onActionRebind) {
        playerInputSystem.Player.Disable();

        InputAction inputAction;
        int bindingIndex;

        switch (binding) {
            default:
            case Binding.Move_Up:
                inputAction = playerInputSystem.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.Move_Down:
                inputAction = playerInputSystem.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.Move_Left:
                inputAction = playerInputSystem.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.Move_Right:
                inputAction = playerInputSystem.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = playerInputSystem.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.InteractAlt:
                inputAction = playerInputSystem.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = playerInputSystem.Player.Pause;
                bindingIndex = 0;
                break;

        }

        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback => {
                callback.Dispose();
                playerInputSystem.Player.Enable();
                onActionRebind();

                //

                PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputSystem.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();

                OnBindingRebind?.Invoke(this, EventArgs.Empty);
            })
            .Start();
    }

}
