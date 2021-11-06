using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClimbUpPuzzle
{
    public sealed class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerModel _model;
        [SerializeField] private GameObject _modelPref;
        private Transform _leftPinPoint, _rightPinPoint;
 
         [SerializeField] private Vector3 _correctionVector = new Vector3(0f, -0.2f, 0f);
        [SerializeField] private float _diskRadius = 0.4f;
        public bool IsPinned { get; private set; }
        private bool _modelExist = false;
        private GameManager _gameManager;
        private GameSettings _gameSettings;
        private InputZone _inputZone;
        public Transform LeftShoulderPoint => _model.LeftShoulderPoint;
        public Transform RightShoulderPoint => _model.RightShoulderPoint;
        public System.Action ModelChangingEvent;
        public const string PLAYER_TAG = "Player";

        public void Prepare(GameManager i_gm, GameSettings i_gs, Transform i_leftPinPoint, Transform i_rightPinPoint)
        {
            _gameManager = i_gm;
            _gameSettings = i_gs;
            _leftPinPoint = i_leftPinPoint;
            _rightPinPoint = i_rightPinPoint;
            IsPinned = true;
            _modelExist = true;
        }

        private void Start()
        {
            _inputZone = _gameManager.InputZone;   
        }

        private void FixedUpdate()
        {
            if (IsPinned)
            {
                Vector3 cpos = _leftPinPoint.position + _correctionVector, mpos = transform.position,
                    d = mpos - cpos;
                    d.z = 0f;
                _model.LeftHandPoint.position = cpos  + d.normalized * _diskRadius;
                //_model.LeftHandPoint.rotation = Quaternion.LookRotation(Vector3.left, Vector3.forward);
                cpos = _rightPinPoint.position + _correctionVector;
                d = mpos - cpos; d.z = 0f;
                _model.RightHandPoint.position = cpos + d.normalized * _diskRadius;
                //_model.RightHandPoint.rotation = Quaternion.LookRotation(Vector3.right, Vector3.forward);

                if (_inputZone.FollowTouchPoint)
                {
                    _model.Headbone.transform.LookAt(_inputZone.TargetTouchPosition, Vector3.up);
                }
                else _model.Headbone.rotation = Quaternion.RotateTowards(_model.Headbone.rotation, Quaternion.LookRotation(transform.forward, Vector3.up), 30f * Time.fixedDeltaTime);
            }
            else
            {
                if (_modelExist && _model.transform.position.y < -25f)
                {
                    Destroy(_model.gameObject);
                    _modelExist = false;
                }
            }
        }

        public void BreakBond()
        {
            IsPinned = false;
            _model.LeftHandPoint.GetComponent<Rigidbody>().isKinematic = false;
            _model.RightHandPoint.GetComponent<Rigidbody>().isKinematic = false;
        }

        public void Restart()
        {
            IsPinned = false;
            if (_modelExist) Destroy(_model.gameObject);
            _model = Instantiate(_modelPref, transform.position, transform.rotation, transform).GetComponent<PlayerModel>();
            _modelExist = true;
            ModelChangingEvent?.Invoke();
            IsPinned = true;
        }

    }
}
