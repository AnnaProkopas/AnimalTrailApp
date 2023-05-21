using UnityEngine;

public class SoundEffectHelper: MonoBehaviour
{
    private const string EnableSoundsKey = "enableSounds";
    private const string EnableMusicKey = "enableMusic";
    
    public static SoundEffectHelper instance;
    
    [SerializeField] private AudioClip eatSound;
    public bool enableSounds = true;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("several copies SoundEffectHelper");
        }
        enableSounds = PlayerPrefs.GetInt(EnableSoundsKey, 0) == 1;
        // enableMusic = PlayerPrefs.GetInt(EnableMusicKey, 0) == 1;

        instance = this;
    }

    public void MakeEatSound()
    {
        MakeSound(eatSound);
    }

    private void MakeSound(AudioClip audioClip)
    {
        if (enableSounds)
            AudioSource.PlayClipAtPoint(audioClip, transform.position);
    }
}
