using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClimbUpPuzzle
{
    public sealed class EndTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject _completeMark;
        [SerializeField] private bool _forLeftHand = false;
        private GameManager _gameManager;
        private bool _activated = false;

        private void Start()
        {
            _completeMark.SetActive(false);
            _activated = false;
            _gameManager = GameManager.Current;
            _gameManager.LevelChangeEvent += Restart;
            GetComponentInChildren<Collider>().isTrigger = true;
        }

        private void Restart()
        {
            _activated = false;
            _completeMark.SetActive(false);
        }

        private void OnTriggerEnter(Collider col)
        {
            if (!_activated && !_gameManager.GameFinished && col.CompareTag(ControlDisk.DISK_TAG))
            {
                if (col.GetComponent<ControlDisk>().IsLeft == _forLeftHand)
                {
                    _activated = true;
                    _completeMark.SetActive(true);
                    col.GetComponent<ControlDisk>().ChangeTriggeredStatus(true);
                }
            }
        }

        private void OnTriggerExit(Collider col)
        {
            if (_activated && !_gameManager.GameFinished && col.CompareTag(ControlDisk.DISK_TAG))
            {
                if (col.GetComponent<ControlDisk>().IsLeft == _forLeftHand)
                {
                    _activated = false;
                    _completeMark.SetActive(false);
                    col.GetComponent<ControlDisk>().ChangeTriggeredStatus(false);
                }
            }
        }
    }
}
