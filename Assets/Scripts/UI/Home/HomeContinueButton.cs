using LevelLoaderModule;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Home
{
    public class HomeContinueButton : MonoBehaviour
    {
        private void Start()
        {
            if (!CanLoadOldLevel())
            {
                GetComponent<Button>().interactable = false;
            }
        }

        public bool CanLoadOldLevel()
        {
            return LevelLoader.HasSavedLevel();
        }
    }
}