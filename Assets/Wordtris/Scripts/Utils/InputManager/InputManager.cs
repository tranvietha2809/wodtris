using System.Collections;
using Assets.Wordis.Frameworks.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Wordtris.Scripts.Utils.InputManager
{
    public class InputManager : Singleton<InputManager>
    {
        private static bool _isTouchAvailable = true;
        public EventSystem eventSystem;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            if (eventSystem == null)
            {
                eventSystem = FindObjectOfType<EventSystem>() as EventSystem;
            }
        }

        public bool canInput(float delay = 0.25F, bool disableOnAvailable = true)
        {
            bool status = _isTouchAvailable;
            if (status && disableOnAvailable)
            {
                _isTouchAvailable = false;
                eventSystem.enabled = false;

                StopCoroutine(nameof(EnableTouchAfterDelay));
                StartCoroutine(nameof(EnableTouchAfterDelay), delay);
            }

            return status;
        }

        public void DisableTouch()
        {
            _isTouchAvailable = false;
            eventSystem.enabled = false;
        }

        public void DisableTouchForDelay(float delay = 0.25F)
        {
            _isTouchAvailable = false;
            eventSystem.enabled = false;

            StopCoroutine(nameof(EnableTouchAfterDelay));
            StartCoroutine(nameof(EnableTouchAfterDelay), delay);
        }

        public void EnableTouch()
        {
            _isTouchAvailable = true;
            eventSystem.enabled = true;
        }

        public IEnumerator EnableTouchAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            EnableTouch();
        }
    }
}