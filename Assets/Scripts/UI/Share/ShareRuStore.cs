using System.Collections;
using UI.Localization;
using UnityEngine;

namespace UI.Share
{
    public class ShareRuStore : MonoBehaviour
    {
        private const string ShareSubjectKey = "Share_subject_key";
        private const string ShareMessageKey = "Share_message_key";

        private bool isFocus;
        private bool isProcessing;
    
        void OnApplicationFocus (bool focus) {
            isFocus = focus;
        }

        public void ShareText () {
            if (!isProcessing) {
                StartCoroutine (Share());
            }
        }
    
        private IEnumerator Share()
        {
            isProcessing = true;

            if (!Application.isEditor)
            {
                AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
                AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
                intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
                AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");

                intentObject.Call<AndroidJavaObject>("setType", "text/plain");
                intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), "SUBJECT");
                intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), LangManager.GetTranslate(ShareMessageKey));

                AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
                currentActivity.Call("startActivity", intentObject);

                yield return new WaitUntil(() => isFocus);
            }

            isProcessing = false;
        }
    }
}
