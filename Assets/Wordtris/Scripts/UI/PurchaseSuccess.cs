using Wordtris.Scripts.Controller;
using Wordtris.Scripts.UI.Extensions;
using Wordtris.Scripts.Utils.InputManager;
using UnityEngine;

namespace Wordtris.Scripts.UI
{
    /// <summary>
    /// This script is attached to purchase success popup.
    /// </summary>
    public class PurchaseSuccess : MonoBehaviour
    {
        public RectTransform rewardAnimPosition;

        /// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
        private void OnEnable()
        {
            UIController.Instance.PlayAddGemsAnimationAtPosition(Vector3.zero, 0.2F);
        }

        /// <summary>
        /// Close button click listener.
        /// </summary>
        public void OnCloseButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                UIFeedback.Instance.PlayButtonPressEffect();
                gameObject.Deactivate();
            }
        }

        /// <summary>
        /// Ok button click listener.
        /// </summary>
        public void OnOkButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                UIFeedback.Instance.PlayButtonPressEffect();
                gameObject.Deactivate();
            }
        }
    }
}