using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Wordis.Frameworks.Utils;
using UnityEngine;

namespace Wordtris.Scripts.Controller
{
    /// <summary>
    /// This script component manages and saves the basic status
    /// if user control including sound, music, haptic feedback, notification, ad status etc.
    /// </summary>
    public class ProfileManager : Singleton<ProfileManager>
    {

        // Sound status change event action.
        public static event Action<bool> OnSoundStatusChangedEvent;

        // Music status change event action.
        public static event Action<bool> OnMusicStatusChangedEvent;

        // Notification status change event action.
        public static event Action<bool> OnNotificationStatusChangedEvent;

        // Returns current status of sound.
        public bool IsSoundEnabled { get; private set; } = true;

        // Returns current status of music.
        public bool IsMusicEnabled { get; private set; } = true;

        // Returns current status of Notifications.
        public bool IsNotificationEnabled { get; private set; } = true;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            Initialise();
        }

        /// <summary>
        /// Initializes the profilemanager.
        /// </summary>
        void Initialise()
        {
            InitProfileStatus();
        }


        /// <summary>
        /// Initialize the audio status.
        /// </summary>
        public void InitProfileStatus()
        {
            // Fetches the status of all user setting and invokes event callbacks for each settings.

            IsMusicEnabled = PlayerPrefs.GetInt("isMusicEnabled", 0) == 0 ? true : false;
            IsNotificationEnabled = PlayerPrefs.GetInt("isNotificationEnabled", 0) == 0 ? true : false;
            IsSoundEnabled = PlayerPrefs.GetInt("isSoundEnabled", 0) == 0 ? true : false;

            if (!IsSoundEnabled)
            {
                OnSoundStatusChangedEvent?.Invoke(IsSoundEnabled);
            }

            if (!IsMusicEnabled)
            {
                OnMusicStatusChangedEvent?.Invoke(IsMusicEnabled);
            }

        }

        /// <summary>
        /// Toggles the sound status.
        /// </summary>
        public void ToggleSoundStatus()
        {
            IsSoundEnabled = !IsSoundEnabled;
            PlayerPrefs.SetInt("isSoundEnabled", IsSoundEnabled ? 0 : 1);

            OnSoundStatusChangedEvent?.Invoke(IsSoundEnabled);
        }

        /// <summary>
        /// Toggles the music status.
        /// </summary>
        public void ToggleMusicStatus()
        {
            IsMusicEnabled = !IsMusicEnabled;
            PlayerPrefs.SetInt("isMusicEnabled", IsMusicEnabled ? 0 : 1);

            OnMusicStatusChangedEvent?.Invoke(IsMusicEnabled);
        }

        /// <summary>
        /// Toggles the notification status.
        /// </summary>
        public void ToggleNotificationStatus()
        {
            IsNotificationEnabled = !IsNotificationEnabled;
            PlayerPrefs.SetInt("isNotificationEnabled", IsNotificationEnabled ? 0 : 1);

            OnNotificationStatusChangedEvent?.Invoke(IsNotificationEnabled);
        }


        public bool IsAppAdFree()
        {
            return true;
        }

    }
}