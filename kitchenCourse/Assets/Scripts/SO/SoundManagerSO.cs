using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]

public class SoundManagerSO : ScriptableObject
{
    public AudioClip[] chop;
    public AudioClip[] deliveryFailed;
    public AudioClip[] deliverySuccess;
    public AudioClip[] footsteps;    
    public AudioClip[] objectDrop; 
    public AudioClip[] objectPickup;    
    public AudioClip stoveSizzle;
    public AudioClip[] trash;
    public AudioClip[] warning;
}
