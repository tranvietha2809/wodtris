using System.Collections;
using System.Linq;
using Wordtris.GameCore.Levels.Base;
using Wordtris.Scripts.Controller;
using Wordtris.Scripts.GamePlay;
using Wordtris.Scripts.UI.Extensions;
using Wordtris.Scripts.Utils.InputManager;
using UnityEngine;
using UnityEngine.UI;

namespace Wordtris.Scripts.UI
{
    /// <summary>
    /// Code behind for Game Over and Level Completion
    /// </summary>
    public class ItemPanel : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField] GameObject dynamite;
        [SerializeField] GameObject bomb;
        [SerializeField] GameObject hammer;
        [SerializeField] GameObject missile;
#pragma warning restore 0649

        void Start()
        {
            OnRefresh();
        }

        /// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
        private void OnEnable()
        {
            OnRefresh();
        }

        public void OnRefresh()
        {
            ItemManager.Instance.LoadItems();
            if (ItemManager.Instance.GetBombCount() > 0)
            {
                bomb.SetActive(true);
            }
            if (ItemManager.Instance.GetDynamiteCount() > 0)
            {
                dynamite.SetActive(true);
            }
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable()
        {
        }

        public void OnDynamitePress()
        {
            ItemManager.Instance.UseItem("Dynamite");
            OnRefresh();
        }

        public void OnBombPress()
        {
            ItemManager.Instance.UseItem("Bomb");
            OnRefresh();
        }
    }
}