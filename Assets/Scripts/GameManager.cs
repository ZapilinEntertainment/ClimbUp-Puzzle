using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClimbUpPuzzle
{
    public sealed class GameManager : MonoBehaviour
    {
        [SerializeField] private GameSettings _gameSettings;
        [SerializeField] private ControlDisk _leftControlDisk, _rightControlDisk;
        [SerializeField] private ParticleSystem _coinCollectPE;

        public static GameManager Current { get; private set; }
        public PlayerController PlayerController { get; private set; }
        public CameraController CameraController { get; private set; }

        public InputZone InputZone { get; private set; }
        public bool GameFinished { get; private set; }
        public int TotalCoinsCount { get; private set; }
        public System.Action LevelChangeEvent;
        private UIManager _uiManager;
        private Vector3 _savedLeftDiskPosition, _savedRightDiskPosition;
       
        public GameSettings GameSettings => _gameSettings;

        private void Awake()
        {
            Current = this;
            //
            if (_gameSettings == null)
            {
                _gameSettings = new GameSettings();
                Debug.LogWarning("game settings not defined, switching to default");
            }
            //
            PlayerController = FindObjectOfType<PlayerController>();
            if (PlayerController == null)
            {
                Debug.LogError("no player controller found");
                return;
            }
            PlayerController.Prepare(this, _gameSettings, _leftControlDisk.PinPoint, _rightControlDisk.PinPoint);
            //
            CameraController = FindObjectOfType<CameraController>();
            if (CameraController == null)
            {
                Debug.LogError("no camera controller found");
                return;
            }
            CameraController.Prepare(_leftControlDisk.transform, _rightControlDisk.transform);
            //
            InputZone = FindObjectOfType<InputZone>();
            if (InputZone == null)
            {
                Debug.LogError("no input field found");
                return;
            }
            InputZone.Prepare(CameraController);
            InputZone.gameObject.SetActive(false);
            //
            _savedLeftDiskPosition = _leftControlDisk.transform.position;
            _leftControlDisk.Prepare(this, true);
            _savedRightDiskPosition = _rightControlDisk.transform.position;
            _rightControlDisk.Prepare(this, false);
            //
            _uiManager = FindObjectOfType<UIManager>();
            if (_uiManager == null) { Debug.LogError("no ui manager found"); }
            _uiManager.Prepare(this);
            //
            _uiManager.SetState(UIState.GameStart);
        }

        public void StartGame()
        {
            GameFinished = false;
            InputZone.gameObject.SetActive(true);
        }
        private void StopGame()
        {
            InputZone.gameObject.SetActive(false);
            _leftControlDisk.Deactivate();
            _rightControlDisk.Deactivate();
        }
        public void BreakBond()
        {
            StopGame();
            PlayerController.BreakBond();            
            _uiManager.SetState(UIState.RestartPanel);
            GameFinished = true;
        }
        public void Restart()
        {
            _leftControlDisk.Deactivate();
            _leftControlDisk.transform.position = _savedLeftDiskPosition;
            _rightControlDisk.Deactivate();
            _rightControlDisk.transform.position = _savedRightDiskPosition;
            PlayerController.Restart();
            LevelChangeEvent?.Invoke();
            StartGame();
        }

        public void CheckVictoryCondition()
        {
            if (!GameFinished && _leftControlDisk.AtTheEndPosition && _rightControlDisk.AtTheEndPosition)
            {
                GameFinished = true;
                _uiManager.SetState(UIState.Victory);
                StopGame();
                Audiomaster.PlaySound(SoundClipType.Victory);
            }
        }

        public float GetStretchValue()
        {
            float a = 0f,b =0f;
            if (_leftControlDisk != null)
            {
                a = GetVisualStretchVal(_leftControlDisk);
            }
            if (_rightControlDisk != null)
            {
                b = GetVisualStretchVal(_rightControlDisk);
            }
            float GetVisualStretchVal(ControlDisk cd)
            {
                return 0.5f * (Mathf.Clamp01(cd.StretchValue - 0.5f)) + 0.5f * cd.OverstretchVal;
            }

            if (a > b) return a;
            else return b;
        }

        public void AddCoin(int x)
        {
            GameConstants.AddCoins(x);
            TotalCoinsCount = GameConstants.GetTotalCoinsCount();            
        }

        public void CollectEffect(Vector3 pos)
        {
            _uiManager.PlayCoinEffect(CameraController.Camera.WorldToScreenPoint(pos));
            _coinCollectPE.transform.position = pos;
            _coinCollectPE.Play();
        }
    }
}
