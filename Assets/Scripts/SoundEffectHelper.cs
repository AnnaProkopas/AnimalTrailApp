
using System;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class SoundEffectHelper: MonoBehaviour
{
    public static SoundEffectHelper instance;
    
    [SerializeField] private AudioClip eatSound;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("several copies SoundEffectHelper");
        }

        instance = this;
    }

    public void MakeEatSound()
    {
        MakeSound(eatSound);
    }

    private void MakeSound(AudioClip audioClip)
    {
        AudioSource.PlayClipAtPoint(audioClip, transform.position);
    }
}
