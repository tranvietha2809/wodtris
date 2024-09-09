using System.Collections;
using System.Collections.Generic;
using Assets.Wordis.Frameworks.UITween.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Wordtris.Scripts.GamePlay
{
    public class InGameMessage : MonoBehaviour
    {
        public AnimationCurve animationCurve;
        public GameObject messageView;
        public Text txtMessageText;

        private readonly Queue<Coroutine> _messagesBeingShown = new Queue<Coroutine>();

        public void ShowMessage(string message)
        {
            var crt = StartCoroutine(ShowMessageCrt(message, _messagesBeingShown.Count));

            _messagesBeingShown.Enqueue(crt);
        }

        IEnumerator ShowMessageCrt(string message, int toWait)
        {
            for (int i = 0; i < toWait; i++)
            {
                yield return _messagesBeingShown.Dequeue();
            }

            messageView.transform.localScale = Vector3.zero;
            txtMessageText.text = message;

            messageView.gameObject.SetActive(true);

            var q = messageView.transform.LocalScale(Vector3.one, 0.2F)
                .SetAnimation(animationCurve)
                .OnComplete(() =>
                {
                    var w = messageView.transform.LocalScale(Vector3.zero, 0.2F)
                        .SetAnimation(animationCurve)
                        .SetDelay(1F);
                });

            yield return new WaitForSeconds(3);
        }
    }
}