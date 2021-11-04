using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClimbUpPuzzle
{
    public sealed class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        private Transform _leftDiscTransform, _rightDiscTransform;
        private Vector3 _targetPos;
        public Camera Camera => _camera;
        private const float _cameraSpeed = 5f;

        public void Prepare(Transform i_leftDisc, Transform i_rightDisc)
        {
            _leftDiscTransform = i_leftDisc;
            _rightDiscTransform = i_rightDisc;
             Vector3 pos0 = _rightDiscTransform.position,
             pos1 = (_leftDiscTransform.position - pos0) * 0.5f + pos0;
            pos1.z = transform.position.z;
            transform.position = pos1;
        }
        private void Update()
        {
            var pos0 = _rightDiscTransform.position;
            _targetPos = (_leftDiscTransform.position - pos0) * 0.5f + pos0;
            Vector3 mpos = transform.position;
            _targetPos.z = mpos.z;

            if (mpos != _targetPos)
            {
                transform.position = Vector3.MoveTowards(mpos, _targetPos, _cameraSpeed * Time.deltaTime);
            }
        }
    }
}
