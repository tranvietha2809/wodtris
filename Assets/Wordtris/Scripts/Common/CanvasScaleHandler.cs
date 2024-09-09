using UnityEngine;
using UnityEngine.UI;

namespace Assets.Wordis.BlockPuzzle.Scripts.Common
{
    public class CanvasScaleHandler : MonoBehaviour
    {
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            float screenAspect = 0.0F;

            if (Screen.height > Screen.width)
            {
                screenAspect = (float)Screen.height / (float)Screen.width;
            }
            else
            {
                screenAspect = (float)Screen.width / (float)Screen.height;
            }

            GetComponent<CanvasScaler>().matchWidthOrHeight = screenAspect < 1.75F
                ? 1.0F
                : 0.5F;
        }
    }
}