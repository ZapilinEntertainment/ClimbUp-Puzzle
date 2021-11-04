using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClimbUpPuzzle
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class ControlDisk : MonoBehaviour
    {
        private PlayerController _playerController;
        private InputZone _inputZone;
        private GameSettings _gameSettings;
        private Rigidbody _rigidbody;
        private Transform _anotherDiskTransform;
        private bool _active = false;
        private GameManager _gameManager;

        public void Prepare(GameManager i_gm, Transform i_diskTransform)
        {
            _gameManager = i_gm;
            _playerController = _gameManager.PlayerController;
            _inputZone = _gameManager.InputZone;
            _gameSettings = _gameManager.GameSettings;
            _anotherDiskTransform = i_diskTransform;
            _rigidbody = GetComponent<Rigidbody>();            
        }

        public void Activate()
        {
            _active = true;
            _rigidbody.useGravity = false;
        }
        public void Deactivate()
        {
            _active = false;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.ResetCenterOfMass();
            _rigidbody.ResetInertiaTensor();
            _rigidbody.useGravity = true;
        }

        private void Update()
        {
            if (_active)
            {
                Vector3 pos0 = transform.position, pos1 = _inputZone.TargetTouchPosition;
                pos1.z = pos0.z;
                Vector3 npos = Vector3.MoveTowards(pos0, pos1, _gameSettings.DiskMoveSpeed * Time.deltaTime),
                    siblingPos = _anotherDiskTransform.position;
                float d = Vector3.SqrMagnitude(npos - siblingPos), maxVal = _gameSettings.MaxDiskDistance;
                if (d < maxVal)
                {
                    _rigidbody.MovePosition(pos0 + (npos - pos0) * (1f - _gameSettings.DiskResistanceCurve.Evaluate(d / maxVal)));
                }
                else _gameManager.BreakBond();
            }
            //else _rigidbody.MovePosition(transform.position + 5f * Vector3.down * Time.deltaTime);
        }
    }
}
