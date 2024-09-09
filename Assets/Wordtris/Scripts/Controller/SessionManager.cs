using System;
using UnityEngine;

namespace Wordtris.Scripts.Controller
{
    public class SessionManager : MonoBehaviour
    {
        public static event Action<SessionInfo> OnSessionUpdatedEvent;

        bool _isFreshLaunched = true;
        readonly TimeSpan _backgroundThreshHoldTimeSpan = new TimeSpan(0, 0, 120);
        SessionInfo _currentSessionInfo = null;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            if (!PlayerPrefs.HasKey("firstOpenDate"))
            {
                PlayerPrefs.SetString("firstOpenDate", DateTime.Now.ToBinary().ToString());
            }

            Invoke("CheckForSessionStatus", 0.5F);
        }

        void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                _isFreshLaunched = false;
                PlayerPrefs.SetString("lastPauseTime", DateTime.Now.ToBinary().ToString());
                PlayerPrefs.SetString("lastAccessedDate", DateTime.Now.ToBinary().ToString());
            }
            else
            {
                if (!_isFreshLaunched)
                {
                    bool doCheckForSessionChange = true;

                    if (PlayerPrefs.HasKey("lastPauseTime"))
                    {
                        DateTime lastOpenedTime =
                            DateTime.FromBinary(Convert.ToInt64(PlayerPrefs.GetString("lastPauseTime")));
                        TimeSpan pauseDuration = DateTime.Now - lastOpenedTime;

                        if (pauseDuration.TotalSeconds < _backgroundThreshHoldTimeSpan.TotalSeconds)
                        {
                            doCheckForSessionChange = false;
                        }
                    }

                    if (doCheckForSessionChange)
                    {
                        Invoke("CheckForSessionStatus", 0.5F);
                    }
                }
            }
        }

        void CheckForSessionStatus()
        {
            //Calculate Session Count.
            int currentSessionCount = 0;

            if (PlayerPrefs.HasKey("currentSessionCount"))
            {
                currentSessionCount = PlayerPrefs.GetInt("currentSessionCount", 0);
            }

            currentSessionCount++;
            PlayerPrefs.SetInt("currentSessionCount", currentSessionCount);
            PlayerPrefs.DeleteKey("lastPauseTime");

            //SessionOfTheDay
            int currentSessionOfDay =
                PlayerPrefs.GetInt("currentSessionOfDay_" + DateTime.Now.Year + "_" + DateTime.Now.DayOfYear, 0);
            currentSessionOfDay += 1;
            PlayerPrefs.SetInt("currentSessionOfDay_" + DateTime.Now.Year + "_" + DateTime.Now.DayOfYear,
                currentSessionOfDay);

            //Days Since Last Accessed
            DateTime lastAccessedDate =
                DateTime.FromBinary(
                    Convert.ToInt64(PlayerPrefs.GetString("lastAccessedDate", DateTime.Now.ToBinary().ToString())));

            TimeSpan durationSinceLastAccessed = DateTime.Now - lastAccessedDate;

            //Days Since Installed
            DateTime firstOpenDate =
                DateTime.FromBinary(
                    Convert.ToInt64(PlayerPrefs.GetString("firstOpenDate", DateTime.Now.ToBinary().ToString())));

            TimeSpan durationSinceFirstTimeAccessed = DateTime.Now - firstOpenDate;

            _currentSessionInfo = new SessionInfo(currentSessionCount, currentSessionOfDay, durationSinceLastAccessed,
                durationSinceFirstTimeAccessed);

            OnSessionUpdatedEvent?.Invoke(_currentSessionInfo);
        }

        void OnApplicationQuit()
        {
            PlayerPrefs.SetString("lastAccessedDate", DateTime.Now.ToBinary().ToString());
        }

        public SessionInfo getCurrentSessionInfo()
        {
            return _currentSessionInfo;
        }
    }

    public class SessionInfo
    {
        public int currentSessionCount = 0;
        public int currentSessionOfDay = 0;
        public TimeSpan durationSinceLastAccessed;
        public TimeSpan durationSinceFirstTimeAccessed;

        public SessionInfo(
            int currentSessionCount,
            int currentSessionOfDay,
            TimeSpan durationSinceLastAccessed,
            TimeSpan durationSinceFirstTimeAccessed)
        {
            this.currentSessionCount = currentSessionCount;
            this.currentSessionOfDay = currentSessionOfDay;
            this.durationSinceLastAccessed = durationSinceLastAccessed;
            this.durationSinceFirstTimeAccessed = durationSinceFirstTimeAccessed;
        }
    }
}