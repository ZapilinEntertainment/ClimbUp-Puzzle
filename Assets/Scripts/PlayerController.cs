using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClimbUpPuzzle
{
    public sealed class PlayerController : MonoBehaviour
    {
        [SerializeField] private Transform _leftPinPoint, _rightPinPoint, _leftHand, _rightHand;
        [SerializeField] private Vector3 _correctionVector = new Vector3(0f, -0.2f, 0f);
        private bool _pinned = true;
        private GameManager _gameManager;
        private GameSettings _gameSettings;

        public void Prepare(GameManager i_gm, GameSettings i_gs)
        {
            _gameManager = i_gm;
            _gameSettings = i_gs;
        }


        private void Update()
        {
            if (_pinned)
            {
                _leftHand.position = _leftPinPoint.position + _correctionVector;
                _leftHand.rotation = Quaternion.LookRotation(Vector3.left, Vector3.forward);
                _rightHand.position = _rightPinPoint.position + _correctionVector;
                _rightHand.rotation = Quaternion.LookRotation(Vector3.right, Vector3.forward);
            }
        }

        public void BreakBond()
        {
            _pinned = false;
            _leftHand.GetComponent<Rigidbody>().isKinematic = false;
            _rightHand.GetComponent<Rigidbody>().isKinematic = false;
        }

    }
}
