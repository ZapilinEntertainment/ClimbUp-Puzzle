using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClimbUpPuzzle
{
    public static class GameConstants
    {
        private const string TOTAL_COINS_KEY = "totalCoins", UPGRADE_COST_KEY = "upgradeCost", LAST_LEVEL_INDEX_KEY = "lastLevelIndex";

        public static int GetTotalCoinsCount()
        {
            return PlayerPrefs.GetInt(TOTAL_COINS_KEY, 0);
        }
        public static void ChangeCoins(int x)
        {
            int s = GetTotalCoinsCount() + x;
            if (s < 0) s = 0;
            PlayerPrefs.SetInt(TOTAL_COINS_KEY, s);
        }

        public static int GetUpgradeCost()
        {
            return PlayerPrefs.GetInt(UPGRADE_COST_KEY, 1);
        }
        public static void IncreaseUpgradeCost()
        {
            PlayerPrefs.SetInt(UPGRADE_COST_KEY, GetUpgradeCost() + 1);
        }

        public static int GetLastLevelIndex()
        {
            return PlayerPrefs.GetInt(LAST_LEVEL_INDEX_KEY, 1);
        }
        public static void IncreaseLastLevelIndex()
        {
            PlayerPrefs.SetInt(LAST_LEVEL_INDEX_KEY, GetLastLevelIndex() + 1);
        }
    }
}
