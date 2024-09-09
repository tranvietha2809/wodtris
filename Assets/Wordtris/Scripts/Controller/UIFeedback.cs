using Assets.Wordis.Frameworks.Utils;

namespace Wordtris.Scripts.Controller
{
    public class UIFeedback : Singleton<UIFeedback>
    {
        /// Plays Button Click Sound and Haptic Feedback.
        public void PlayButtonPressEffect()
        {
            AudioController.Instance.PlayButtonClickSound();
        }

        /// Plays Block Shape Pick Effect.
        public void PlayBlockShapePickEffect()
        {
            AudioController.Instance.PlayBlockShapePickSound();
        }

        /// Plays Block Shape Pick Effect.
        public void PlayBlockShapePlaceEffect()
        {
            AudioController.Instance.PlayBlockShapePlaceSound();
        }

        /// Plays Block Shape Pick Effect.
        public void PlayBlockShapeResetEffect()
        {
            AudioController.Instance.PlayBlockShapeResetSound();
        }
    }
}