using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;

namespace Sounds
{
    public class SoundController : MonoBehaviour
    {
        [SerializeField, NotNull] private AudioSource _audioSource;
        
        private Dictionary<SoundType, SoundSO> _sounds;
    
        [Inject]
        public void Construct(SoundListSO soundSOs)
        {
            _sounds = soundSOs.Sounds.ToDictionary(so => so.Type);
        }

        public void PlaySound(SoundType type)
        {
            if (_sounds.TryGetValue(type, out var sound))
            {
                _audioSource.PlayOneShot(sound.Clip, sound.Volume);
            }
        }
    }
}