﻿using Assets.Wordis.Frameworks.Utils;
using UnityEngine;

namespace Wordtris.Scripts.Controller
{
    [RequireComponent(typeof(AudioSource))]
    public class BackgroundMusic : Singleton<BackgroundMusic>
    {
#pragma warning disable 0649
        [SerializeField] private AudioClip BGMusicMain;
#pragma warning restore 0649

        private AudioSource CurrentAudioSource;

        void Awake()
        {
            //Init audio source to play background music.
            CurrentAudioSource = GetComponent<AudioSource>();
        }

        void Start()
        {
            Invoke("StartBGMusic", 1F);
        }

        void OnEnable()
        {
            //Register Audio status event
            ProfileManager.OnMusicStatusChangedEvent += OnMusicStatusChangedEvent;
        }

        void OnDisable()
        {
            //Unregister Audio status event
            ProfileManager.OnMusicStatusChangedEvent -= OnMusicStatusChangedEvent;
        }

        void StartBGMusic()
        {
            ///Start playing music is music setting is enabled.
            if (ProfileManager.Instance.IsMusicEnabled && !CurrentAudioSource.isPlaying)
            {
                CurrentAudioSource.clip = BGMusicMain;
                CurrentAudioSource.loop = true;
                CurrentAudioSource.Play();
            }
        }

        /// <summary>
        /// Pauses the background music.
        /// </summary>
        public void PauseBGMusic()
        {
            if (CurrentAudioSource.isPlaying)
            {
                CurrentAudioSource.Pause();
            }
        }

        /// <summary>
        /// Resumes the background music.
        /// </summary>
        public void ResumeBGMusic()
        {
            if (!CurrentAudioSource.isPlaying)
            {
                CurrentAudioSource.Play();
            }
        }

        /// <summary>
        /// Raises the music status changed event event.
        /// </summary>
        /// <param name="isMusicEnabled">If set to <c>true</c> is music enabled.</param>
        void OnMusicStatusChangedEvent(bool isMusicEnabled)
        {
            if (!isMusicEnabled)
            {
                PauseBGMusic();
            }
            else
            {
                ResumeBGMusic();
            }
        }
    }
}