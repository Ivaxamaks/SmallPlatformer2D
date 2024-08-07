using System;
using UnityEngine;

namespace Sounds
{
    [Serializable]
    public class SoundSO
    {
        public SoundType Type;
        public AudioClip Clip;
        public float Volume = 1.0f;
    }
}