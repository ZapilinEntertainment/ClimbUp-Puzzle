using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClimbUpPuzzle
{
    public sealed class GameManager : MonoBehaviour
    {
        [SerializeField] private GameSettings _gameSettings;
        [SerializeField] private ControlDisk _leftControlDisk, _rightControlDisk;
        public PlayerController PlayerController { get; private set; }
        public CameraController CameraController { get; private set; }

        public InputZone InputZone { get; private set; }
        public GameSettings GameSettings => _gameSettings;

        private void Awake()
        {
            if (_gameSettings == null)
            {
                _gameSettings = new GameSettings();
                Debug.LogWarning("warning -  game settings not defined, switching to default");
            }
            //
            PlayerController = FindObjectOfType<PlayerController>();
            if (PlayerController == null)
            {
                Debug.LogError("error - no player controller found");
                return;
            }
            PlayerController.Prepare(this, _gameSettings);
            //
            CameraController = FindObjectOfType<CameraController>();
            if (CameraController == null)
            {
                Debug.LogError("error - no camera controller found");
                return;
            }
            CameraController.Prepare(_leftControlDisk.transform, _rightControlDisk.transform);
            //
            InputZone = FindObjectOfType<InputZone>();
            if (InputZone == null)
            {
                Debug.LogError("error - no input field found");
                return;
            }
            InputZone.Prepare(CameraController);
            //
            _leftControlDisk.Prepare(this, _rightControlDisk.transform);
            _rightControlDisk.Prepare(this, _leftControlDisk.transform);
        }

        public void BreakBond()
        {
            InputZone.gameObject.SetActive(false);
            PlayerController.BreakBond();
            _leftControlDisk.Deactivate();
            _rightControlDisk.Deactivate();
        }
    }
}
