using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClimbUpPuzzle
{
    [RequireComponent(typeof(Collider))]
    public class Coin : MonoBehaviour
    {
        [SerializeField] private int _value = 1;
        private bool _collected = false;
        private GameManager _gameManager;

        private void Start()
        {
            GetComponentInChildren<Collider>().isTrigger = true;
            _gameManager = GameManager.Current;
        }
        private void OnTriggerEnter(Collider col)
        {
            if (!_collected && !_gameManager.GameFinished &&  (
                col.CompareTag(PlayerController.PLAYER_TAG) || col.CompareTag(ControlDisk.DISK_TAG)
                )
                )
            {
                _collected = true;
                var gm = GameManager.Current;                
                gm.AddCoin(_value);
                gm.CollectEffect(transform.position);
                Audiomaster.PlaySound(SoundClipType.CoinCollected);
                Destroy(gameObject);
            }
        }
    }
}
