using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClimbUpPuzzle
{
    public enum SoundClipType : byte { CoinCollected, Victory, BallSound, ButtonSound, BreakSound}
    public sealed class Audiomaster : MonoBehaviour
    {
        [SerializeField] private AudioClip _collectSound, _victorySound, _ballSound, _buttonSound, _breakSound;
        [SerializeField] private AudioSource _audioSource;
        private static Audiomaster Current;
        public static void PlaySound(SoundClipType type)
        {
            Current.a_PlaySound(type);
        }

        private void Awake()
        {
            Current = this;
        }
        private void a_PlaySound(SoundClipType type)
        {
            AudioClip ac = null;
            switch (type)
            {
                case SoundClipType.CoinCollected: ac = _collectSound; break;
                case SoundClipType.Victory: ac = _victorySound; break;
                case SoundClipType.BallSound: ac = _ballSound; break;
                case SoundClipType.ButtonSound: ac = _buttonSound; break;
                case SoundClipType.BreakSound: ac = _breakSound; break;
            }
            if (ac != null)
            {
                _audioSource.clip = ac;
                _audioSource.Play();
            }
        }
    }
}
