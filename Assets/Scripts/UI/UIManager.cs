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
        [SerializeField] private Image _stretchBar, _coinCollectEffect, _upgradeButtonBack, _upgradeEffect;
        [SerializeField] private TMPro.TMP_Text _totalCoinsLabel, _upgradeCostLabel, _levelIndexLabel;
        private bool _coinEffectInProgress = false, _upgradeEffectInProgress = false;
        private float _coinEffectProgress = 0f, _upgradeEffectProgress = 0f;
        private UIState _uistate;
        private GameManager _gameManager;
        private readonly Color MIN_STRETCH_COLOR = Color.green, MAX_STRETCH_COLOR = Color.red, 
            GAME_BLUE = new Color(0.1921569f, 0.6117647f, 0.9529412f);
        private const float COIN_EFFECT_TIME = 2f, UPGRADE_EFFECT_TIME = 2f;

        public void Prepare(GameManager i_gm)
        {
            _gameManager = i_gm;
            _uistate = UIState.Disabled;
            _startGamePanel.SetActive(false);
            _restartPanel.SetActive(false);
            _victoryPanel.SetActive(false);
            _stretchPanel.SetActive(false);
            _coinCollectEffect.gameObject.SetActive(false);
            _upgradeEffect.gameObject.SetActive(false);
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
                    if (_upgradeEffectInProgress)
                    {
                        _upgradeEffect.gameObject.SetActive(false);
                        _upgradeEffectInProgress = false;
                    }
                    break;
                case UIState.Victory: _victoryPanel.SetActive(false); break;
                case UIState.GameStart: _startGamePanel.SetActive(false); break;
            }
            _uistate = i_state;
            if (_uistate != UIState.Victory) _levelIndexLabel.text = "Level " + GameConstants.GetLastLevelIndex().ToString();
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
                    _startGamePanel.SetActive(true);
                    RefreshUpgradeButton();
                    break;
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
            else
            {
                if (_uistate == UIState.GameStart)
                {
                    if (_upgradeEffectInProgress)
                    {
                        _upgradeEffectProgress = Mathf.MoveTowards(_upgradeEffectProgress, 1f, Time.deltaTime / UPGRADE_EFFECT_TIME);
                        if (_upgradeEffectProgress == 1f)
                        {
                            _upgradeEffect.gameObject.SetActive(false);
                            _upgradeEffectInProgress = false;
                        }
                        else
                        {
                            _upgradeEffect.transform.localScale = Vector3.one * _upgradeEffectProgress * 3f;
                            _upgradeEffect.color = Color.Lerp(Color.white, Color.clear, _coinEffectProgress);
                        }
                    }
                }
            }
        }

        public void PlayCoinEffect(Vector3 pos)
        {
            if (_coinEffectInProgress) RefreshTotalCoinsCount();
            _coinCollectEffect.gameObject.SetActive(true);
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

        public void Upgrade()
        {
            if (_gameManager.TryUpgrade())
            {
                RefreshUpgradeButton();
                RefreshTotalCoinsCount();
                _upgradeEffect.gameObject.SetActive(true);
                _upgradeEffectInProgress = true;
                _upgradeEffectProgress = 0f;
            }
        }
        private void RefreshUpgradeButton()
        {
            int cost = GameConstants.GetUpgradeCost();
            _upgradeCostLabel.text = "Upgrade \n(" + cost.ToString() + " coins)";
            if (cost > _gameManager.TotalCoinsCount)
            {
                _upgradeButtonBack.color = Color.gray;
            }
            else
            {
                _upgradeButtonBack.color = GAME_BLUE;
            }
        }
        #endregion
    }
}
