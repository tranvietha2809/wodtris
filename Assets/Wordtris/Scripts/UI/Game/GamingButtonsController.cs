using Wordtris.GameCore;
using Wordtris.Scripts.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace Wordtris.Scripts.GamePlay
{
    /// <summary>
    /// This script controls the block shapes that being place/played on board grid.
    /// It controls spawning of block shapes and organizing it.
    /// </summary>
    public class GamingButtonsController : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField] Button btnArrowLeft;
        [SerializeField] Button btnArrowDown;
        [SerializeField] Button btnArrowRight;
        [SerializeField] Button btnRotate;

#pragma warning disable 0649

        public void OnRotateButtonPressed()
        {
            GamePlayUI.Instance.HandleGameEvent(GameEvent.RotateRight);
        }

        public void OnRightButtonPressed()
        {
            GamePlayUI.Instance.HandleGameEvent(GameEvent.Right);
        }

        public void OnDownButtonPressed()
        {
            GamePlayUI.Instance.HandleGameEvent(GameEvent.Down);
        }

        /// <summary>
        /// listener
        /// </summary>
        public void OnLeftButtonPressed()
        {
            GamePlayUI.Instance.HandleGameEvent(GameEvent.Left);
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.LeftArrow) ||
                Input.GetKeyUp(KeyCode.A))
            {
                OnLeftButtonPressed();
            }

            if (Input.GetKeyUp(KeyCode.RightArrow) ||
                Input.GetKeyUp(KeyCode.D))
            {
                OnRightButtonPressed();
            }

            if (Input.GetKeyUp(KeyCode.DownArrow) ||
                Input.GetKeyUp(KeyCode.S) ||
                Input.GetKeyUp(KeyCode.Space))
            {
                OnDownButtonPressed();
            }
        }

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
        }

        /// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
        private void OnEnable()
        {
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable()
        {
        }
    }
}