using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour
{

    private const string PLAYER_PREFS_SOUND_EFFECT = "SounEffectVolume";
    public static SoundManager Instance { get; private set; }

    [SerializeField] private SoundManagerSO soundManagerSO;

    private float volume = 1f;
    private void Awake() {
        Instance = this;

        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECT, 1f);
    }

    public void Start() {
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        DeliveryManager.Instance.OnRecipeSucceeded += DeliveryManager_OnRecipeSucceeded;

        CutCounter.OnAnyCut += CutCounter_OnAnyCut;

        Player.Instance.OnPickupSound += Player_OnPickupSound;

        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;

        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, System.EventArgs e) {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(soundManagerSO.trash, trashCounter.transform.position);
    }

    private void BaseCounter_OnAnyObjectPlacedHere(object sender, System.EventArgs e) {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(soundManagerSO.objectDrop, baseCounter.transform.position);
    }

    private void Player_OnPickupSound(object sender, System.EventArgs e) {
        PlaySound(soundManagerSO.objectPickup, Player.Instance.transform.position);
    }

    private void CutCounter_OnAnyCut(object sender, System.EventArgs e) {
        CutCounter cuttingCounter = sender as CutCounter;
        PlaySound(soundManagerSO.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeSucceeded(object sender, System.EventArgs e) {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(soundManagerSO.deliverySuccess, deliveryCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e) {
        DeliveryCounter deliveryCounter =  DeliveryCounter.Instance;
        PlaySound(soundManagerSO.deliveryFailed, deliveryCounter.transform.position);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f) {
        PlaySound(audioClipArray[Random.Range(0,audioClipArray.Length)], position, volume);
        
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f) {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }

    public void PlayFootStepSound(Vector3 position, float volumeMultiplayer = 1f) {
        PlaySound(soundManagerSO.footsteps, position, volumeMultiplayer * volume);
    }
    public void PlayCountdownSound() {
        PlaySound(soundManagerSO.warning, Vector3.zero);
    } 
    public void PlayWarningSound(Vector3 position) {
        PlaySound(soundManagerSO.warning, position);
    }

    

    public  void ChangeVolume() {
        volume += .1f;
        if(volume > 1f) {
            volume = 0f;
        }

        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECT, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume() {
        return volume;
    }

}
