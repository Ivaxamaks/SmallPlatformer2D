using System.Collections.Generic;
using UnityEngine;

namespace Sounds
{
    [CreateAssetMenu(menuName = "Game/SoundListSO")]
    public class SoundListSO : ScriptableObject
    {
        public List<SoundSO> Sounds;
    }
}