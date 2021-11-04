using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClimbUpPuzzle
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettingsObject", order = 1)]
    public sealed class GameSettings : ScriptableObject
    {
        [SerializeField] private float _diskMoveSpeed = 1f, _maxDiskDistance = 8f;
        [SerializeField] private AnimationCurve _disksResistanceCurve;

        public float DiskMoveSpeed => _diskMoveSpeed;
        public float MaxDiskDistance => _maxDiskDistance;
        public AnimationCurve DiskResistanceCurve => _disksResistanceCurve;
    }
}
