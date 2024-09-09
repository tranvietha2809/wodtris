using UnityEngine;

namespace Wordtris.Scripts.GamePlay
{
    /// <summary>
    /// This script component is attached to all block shape containers.
    /// </summary>
    public class GamingButtonContainer : MonoBehaviour
    {
        /// <summary>
        /// Awakes the script instance and initializes block parent to cache it.
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
        }

        /// <summary>
        /// Resets and destroy block shape on game over or game leave.
        /// </summary>
        public void Reset()
        {
        }
    }
}