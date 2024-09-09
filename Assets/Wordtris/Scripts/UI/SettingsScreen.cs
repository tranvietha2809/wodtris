
using System.Collections;
using Wordtris.Scripts.Controller;
using Wordtris.Scripts.UI.Extensions;
using Wordtris.Scripts.Utils.InputManager;
using UnityEngine;
using UnityEngine.UI;

namespace Wordtris.Scripts.UI
{
    /// <summary>
    /// Settings screen controls different user selection like sound, music, language etc.
    /// </summary>
    public class SettingsScreen : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField] Text txtVersion;
#pragma warning restore 0649

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            txtVersion.text = "Version : " + Application.version;
        }

        /// <summary>
        /// Close button click listener.
        /// </summary>
        public void OnCloseButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                gameObject.Deactivate();
            }
        }

        /// <summary>
        /// Navigate to given URL.
        /// </summary>
        private IEnumerator NavigateToUrl(string url)
        {
            yield return new WaitForSeconds(0.2F);
            Application.OpenURL(url);
        }
    }
}