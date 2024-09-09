using Assets.Wordis.Frameworks.Utils;
using UnityEngine;

namespace Wordtris.Scripts.Controller
{
    public class AudioController : Singleton<AudioController>
    {
        [Header("Audio Soureces")] public AudioSource audioSource;
        public AudioSource lowSoundSource;

        float lowAudioDefaultVolume = 0.1F;

        [Header("Audio Clips")] public AudioClip btnPressSound;
        public AudioClip addGemsSound;
        public AudioClip addScoreSoundChord;
        public AudioClip addGemsSoundChord;

        #region GamePlay Sounds

        public AudioClip blockPickSound;
        public AudioClip blockPlaceSound;
        public AudioClip blockResetSound;

        public AudioClip lineBreakSound1;
        public AudioClip lineBreakSound2;
        public AudioClip lineBreakSound3;
        public AudioClip lineBreakSound4;

        #endregion

        public void PlayClip(AudioClip clip)
        {
            if (ProfileManager.Instance.IsSoundEnabled)
            {
                audioSource.PlayOneShot(clip);
            }
        }

        public void PlayClipLow(AudioClip clip)
        {
            if (ProfileManager.Instance.IsSoundEnabled)
            {
                lowSoundSource.volume = lowAudioDefaultVolume;
                lowSoundSource.PlayOneShot(clip);
            }
        }

        public void PlayClipLow(AudioClip clip, float volume)
        {
            if (ProfileManager.Instance.IsSoundEnabled)
            {
                lowSoundSource.volume = volume;
                lowSoundSource.PlayOneShot(clip);
            }
        }

        public void PlayButtonClickSound()
        {
            if (ProfileManager.Instance.IsSoundEnabled)
            {
                audioSource.PlayOneShot(btnPressSound);
            }
        }

        public void PlayBlockShapePickSound()
        {
            if (ProfileManager.Instance.IsSoundEnabled)
            {
                audioSource.PlayOneShot(blockPickSound);
            }
        }

        public void PlayBlockShapePlaceSound()
        {
            if (ProfileManager.Instance.IsSoundEnabled)
            {
                audioSource.PlayOneShot(blockPlaceSound);
            }
        }

        public void PlayBlockShapeResetSound()
        {
            if (ProfileManager.Instance.IsSoundEnabled)
            {
                audioSource.PlayOneShot(blockResetSound);
            }
        }

        public void PlayLineBreakSound(int lines)
        {
            if (ProfileManager.Instance.IsSoundEnabled)
            {
                switch (lines)
                {
                    case 1:
                        audioSource.PlayOneShot(lineBreakSound1);
                        break;

                    case 2:
                        audioSource.PlayOneShot(lineBreakSound2);
                        break;

                    case 3:
                        audioSource.PlayOneShot(lineBreakSound3);
                        break;

                    case 4:
                        audioSource.PlayOneShot(lineBreakSound4);
                        break;

                    default:
                        audioSource.PlayOneShot(lineBreakSound4);
                        break;
                }
            }
        }
    }
}