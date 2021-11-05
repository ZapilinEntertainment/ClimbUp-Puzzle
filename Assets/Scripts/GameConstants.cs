using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClimbUpPuzzle
{
    public static class GameConstants
    {
        private const string TOTAL_COINS_KEY = "totalCoins";

        public static int GetTotalCoinsCount()
        {
            return PlayerPrefs.GetInt(TOTAL_COINS_KEY, 0);
        }
        public static void AddCoins(int x)
        {
            PlayerPrefs.SetInt(TOTAL_COINS_KEY, GetTotalCoinsCount() + x);
        }
    }
}
