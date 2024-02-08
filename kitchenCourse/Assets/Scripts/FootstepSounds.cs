using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSounds : MonoBehaviour
{
    private Player player;
    private float footstepTimer;
    private float footstepTimerMax = 0.1f;


    private void Start() {
        player = GetComponent<Player>();
    }

    private void Update() {
        footstepTimer -= Time.deltaTime;
        if (footstepTimer < 0f) {
            footstepTimer = footstepTimerMax;
            if(player.IsWalking()) {
                float volume = 1f;
                SoundManager.Instance.PlayFootStepSound(player.transform.position, volume);
            }
        }
    }
}
