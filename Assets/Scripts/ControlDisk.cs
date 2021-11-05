using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClimbUpPuzzle
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class ControlDisk : MonoBehaviour
    {
        [SerializeField] private Transform _pinPoint;
        public Transform PinPoint => _pinPoint;
        public bool AtTheEndPosition { get; private set; }
        public float StretchValue { get; private set; }
        public float OverstretchVal { get; private set; }

        private PlayerController _playerController;
        private InputZone _inputZone;
        private GameSettings _gameSettings;
        private Rigidbody _rigidbody;
        private Transform  _shoulderPoint;
        private bool _active = false, _isLeft = false;
        
        private GameManager _gameManager;
        public const string DISK_TAG = "GameController", DISK_LAYER = "Disk";

        public void Prepare(GameManager i_gm, bool i_isLeft)
        {
            _gameManager = i_gm;
            _playerController = _gameManager.PlayerController;
            _inputZone = _gameManager.InputZone;
            _gameSettings = _gameManager.GameSettings;
            _isLeft = i_isLeft;
            _shoulderPoint = _isLeft ? _playerController.LeftShoulderPoint : _playerController.RightShoulderPoint;
            _playerController.ModelChangingEvent += PlayerModelChanged;
            _rigidbody = GetComponent<Rigidbody>();            
        }

        private void PlayerModelChanged()
        {
            _shoulderPoint = _isLeft ? _playerController.LeftShoulderPoint : _playerController.RightShoulderPoint;
            OverstretchVal = 0f;
            _rigidbody.isKinematic = false;
        }
        public void Activate()
        {
            _active = true;
            _rigidbody.useGravity = false;
            Audiomaster.PlaySound(SoundClipType.BallSound);
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
            float t = Time.deltaTime;
            bool overstretch = false;
            if (_active)
            {
                Vector3 pos0 = transform.position, pos1 = _inputZone.TargetTouchPosition;
                pos1.z = pos0.z;
                Vector3 npos = Vector3.MoveTowards(pos0, pos1, _gameSettings.DiskMoveSpeed * t),
                    shoulderPos = _shoulderPoint.position;
                float d = Vector3.SqrMagnitude(npos - shoulderPos), maxVal = _gameSettings.MaxDiskDistance;
                maxVal *= maxVal;
                if (d < maxVal)
                {
                    StretchValue = d / maxVal;
                    if (StretchValue > 1f) StretchValue = 1f;
                    _rigidbody.MovePosition(pos0 + (npos - pos0) * (1f - _gameSettings.DiskResistanceCurve.Evaluate(StretchValue)));
                }
                else
                {
                    overstretch = true;
                    StretchValue = 1f;
                }
            }
            else
            {
                if (_playerController.IsPinned)
                {
                    float d = Vector3.SqrMagnitude(transform.position - _shoulderPoint.position),
                        maxVal = _gameSettings.MaxDiskDistance;
                    maxVal *= maxVal;
                    StretchValue = d / maxVal;
                    if (StretchValue > 1f)
                    {
                        StretchValue = 1f;
                        overstretch = true;
                    }
                    else overstretch = false;
                }
            }

            if (overstretch) {
                OverstretchVal = Mathf.MoveTowards(OverstretchVal, 1f, t / _gameSettings.MaxOverstretchDelay);
                if (OverstretchVal >= 1f)
                {
                    Audiomaster.PlaySound(SoundClipType.BreakSound);
                    _gameManager.BreakBond();
                }
            }
            else Mathf.MoveTowards(OverstretchVal, 0f, (t / _gameSettings.MaxOverstretchDelay) * 4f);

            //else _rigidbody.MovePosition(transform.position + 5f * Vector3.down * Time.deltaTime);
        }

        public void ChangeTriggeredStatus(bool x)
        {
            AtTheEndPosition = x;
            Deactivate();
            _rigidbody.isKinematic = true;
            GameManager.Current.CheckVictoryCondition();
        }

    }
}
