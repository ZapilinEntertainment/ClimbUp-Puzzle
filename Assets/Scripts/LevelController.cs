using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClimbUpPuzzle
{
    public sealed class LevelController : MonoBehaviour
    {
        [SerializeField] private Transform _collectiblesHost;
        void Start()
        {
            GameManager.Current.LevelChangeEvent += Restart;
        }

        private void Restart() {
            for (int i = 0; i< _collectiblesHost.childCount; i++)
            {
                _collectiblesHost.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
}
