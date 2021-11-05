using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClimbUpPuzzle
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettingsObject", order = 1)]
    public sealed class GameSettings : ScriptableObject
    {
        [SerializeField] private float _diskMoveSpeed = 1f, _maxDiskDistance = 8f, _maxOverstretchDelay = 0.3f;
        [SerializeField] private AnimationCurve _disksResistanceCurve;

        public float DiskMoveSpeed => _diskMoveSpeed;
        public float MaxDiskDistance => _maxDiskDistance;
        public float MaxOverstretchDelay => _maxOverstretchDelay;
        public AnimationCurve DiskResistanceCurve => _disksResistanceCurve;
    }
}
