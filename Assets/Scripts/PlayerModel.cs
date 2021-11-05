using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClimbUpPuzzle
{
    public sealed class PlayerModel : MonoBehaviour
    {
        [SerializeField]
        private Transform _leftHandPoint, _rightHandPoint,
           _leftShoulderPoint, _rightShoulderPoint, _headBone;
        public Transform LeftShoulderPoint => _leftShoulderPoint;
        public Transform RightShoulderPoint => _rightShoulderPoint;
        public Transform LeftHandPoint => _leftHandPoint;
        public Transform RightHandPoint => _rightHandPoint;
        public Transform Headbone => _headBone;
    }
}
