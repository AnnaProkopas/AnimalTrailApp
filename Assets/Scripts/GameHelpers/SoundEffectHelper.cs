using UnityEngine;

public class SoundEffectHelper: MonoBehaviour
{
    private const string EnableSoundsKey = "enableSounds";
    private const string EnableMusicKey = "enableMusic";
    
    public static SoundEffectHelper instance;
    
    [SerializeField] private AudioClip eatSound;
    [SerializeField] private AudioClip fallSound;
    [SerializeField] private AudioClip achieveSound;
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

    public void MakeEatSound(Vector3 position)
    {
        MakeSound(eatSound, position);
    }

    public void MakeFallSound(Vector3 position)
    {
        MakeSound(fallSound, position);
    }
    
    public void MakeAchieveSound()
    {
        MakeSound(achieveSound, transform.position);
    }

    private void MakeSound(AudioClip audioClip, Vector3 position)
    {
        if (enableSounds)
            AudioSource.PlayClipAtPoint(audioClip, position);
    }
}
