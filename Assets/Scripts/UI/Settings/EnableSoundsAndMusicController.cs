using GameHelpers;
using UnityEngine;

namespace UI.Settings
{
    public class EnableSoundsAndMusicController: MonoBehaviour
    {
        private const string EnableSoundsKey = "enableSounds";
        private const string EnableMusicKey = "enableMusic";
    
        private bool enableSounds;
        private bool enableMusic;

        [SerializeField] private LangSwitchedText soundsButtonText;
        [SerializeField] private LangSwitchedText musicButtonText;
    
        private void Start()
        {
            enableSounds = PlayerPrefs.GetInt(EnableSoundsKey, 0) == 1;
            enableMusic = PlayerPrefs.GetInt(EnableMusicKey, 0) == 1;
        
            soundsButtonText.ChangeText(enableSounds);
            // musicButtonText.ChangeText(enableMusic);
        }

        public void OnSetEnableSounds()
        {
            enableSounds = !enableSounds;
            SoundEffectHelper.instance.enableSounds = enableSounds;
            PlayerPrefs.SetInt(EnableSoundsKey, enableSounds ? 1 : 0);
            soundsButtonText.ChangeText(enableSounds);
            // SoundEffectHelper.instance.MakeEatSound();
        }

        public void OnSetEnableMusic()
        {
            enableMusic = !enableMusic;
            // SoundEffectHelper.instance.enableMusic = enableMusic;
            PlayerPrefs.SetInt(EnableMusicKey, enableMusic ? 1 : 0);
            musicButtonText.ChangeText(enableMusic);
        }
    }
}