using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClimbUpPuzzle
{
    public enum UIState : byte { Disabled, GameStart, RestartPanel, Ingame, Victory}
    public sealed class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject _startGamePanel, _restartPanel, _stretchPanel, _victoryPanel;
        [SerializeField] private Image _stretchBar, _coinCollectEffect;
        [SerializeField] private TMPro.TMP_Text _totalCoinsLabel;
        private bool _coinEffectInProgress = false;
        private float _coinEffectProgress = 0f;
        private UIState _uistate;
        private GameManager _gameManager;
        private readonly Color MIN_STRETCH_COLOR = Color.green, MAX_STRETCH_COLOR = Color.red;
        private const float COIN_EFFECT_TIME = 1f;

        public void Prepare(GameManager i_gm)
        {
            _gameManager = i_gm;
            _uistate = UIState.Disabled;
            _startGamePanel.SetActive(false);
            _restartPanel.SetActive(false);
            _victoryPanel.SetActive(false);
            _stretchPanel.SetActive(false);
            _coinCollectEffect.gameObject.SetActive(false);
        }
        public void SetState(UIState i_state)
        {
            switch (_uistate)
            {
                case UIState.RestartPanel: _restartPanel.SetActive(false);break;
                case UIState.Ingame: 
                    _stretchPanel.SetActive(false); 
                    if (_coinEffectInProgress)
                    {
                        _coinCollectEffect.gameObject.SetActive(false);
                        _coinEffectInProgress = false;
                    }
                    break;
                case UIState.Victory: _victoryPanel.SetActive(false); break;
                case UIState.GameStart: _startGamePanel.SetActive(false); break;
            }
            _uistate = i_state;
            switch(_uistate)
            {
                case UIState.RestartPanel: 
                    _restartPanel.SetActive(true);
                    RefreshTotalCoinsCount();
                    break;
                case UIState.Ingame: _stretchPanel.SetActive(true); break;
                case UIState.Victory:
                    RefreshTotalCoinsCount();
                    _victoryPanel.SetActive(true); break;
                case UIState.GameStart:
                    RefreshTotalCoinsCount();
                    _startGamePanel.SetActive(true);break;
            }
        }

        private void Update()
        {
            if (_uistate == UIState.Ingame)
            {
                float x = _gameManager.GetStretchValue();
                _stretchBar.fillAmount = x;
                _stretchBar.color = Color.Lerp(MIN_STRETCH_COLOR, MAX_STRETCH_COLOR, x);

                if (_coinEffectInProgress)
                {
                    _coinEffectProgress = Mathf.MoveTowards(_coinEffectProgress, 1f, Time.deltaTime / COIN_EFFECT_TIME);                    
                    if (_coinEffectProgress == 1f)
                    {
                        _coinCollectEffect.gameObject.SetActive(false);
                        _coinEffectInProgress = false;
                        RefreshTotalCoinsCount();
                    }
                    else
                    {
                        _coinCollectEffect.transform.position = Vector3.Lerp(_coinCollectEffect.transform.position, _totalCoinsLabel.transform.position, _coinEffectProgress);
                        _coinCollectEffect.color = Color.Lerp(Color.white, Color.clear, _coinEffectProgress);
                    }
                }
            }
        }

        public void PlayCoinEffect(Vector3 pos)
        {
            if (_coinEffectInProgress) RefreshTotalCoinsCount();
            else _coinCollectEffect.gameObject.SetActive(true);
            _coinEffectProgress = 0f;
            _coinCollectEffect.transform.position = pos;
            _coinCollectEffect.color = Color.white;
            _coinEffectInProgress = true;
        }

        private void RefreshTotalCoinsCount()
        {
            _totalCoinsLabel.text = _gameManager.TotalCoinsCount.ToString();
        }

        #region buttons
        public void RestartButton()
        {
            _gameManager.Restart();
            SetState(UIState.Ingame);
            Audiomaster.PlaySound(SoundClipType.ButtonSound);
        }
        public void StartGameButton()
        {
            _gameManager.StartGame();
            SetState(UIState.Ingame);
            Audiomaster.PlaySound(SoundClipType.ButtonSound);
        }
        public void ContinueButton()
        {
            RestartButton();
        }
        #endregion
    }
}
