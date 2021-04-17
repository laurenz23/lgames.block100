using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace game_ideas
{
    public class PlayerPrefsManager : MonoBehaviour
    {
        private string hello = "Hello";
        
        public bool CheckHello()
        {
            if (PlayerPrefs.HasKey(hello))
            {
                return true;
            }

            PlayerPrefs.SetInt(hello, 1);
            return false;
        }

        private string playerName = null;

        public void SetPlayerName(string value)
        {
            PlayerPrefs.SetString(playerName, value);
        }

        public string GetPlayerName()
        {
            return PlayerPrefs.GetString(playerName);
        }

        private string playerLevel = "PlayerLevel";

        public void SetPlayerLevel(int value)
        {
            PlayerPrefs.SetInt(playerLevel, value);
        }

        public int GetPlayerLevel()
        {
            return PlayerPrefs.GetInt(playerLevel);
        }

        public int setMaxLevel;
        private string maxLevel = "MaxLevel";

        public int GetMaxLevel()
        {
            return PlayerPrefs.GetInt(maxLevel);
        }

        private string isGameFinish = "IsGameFinish";

        public void SetGameIsFinish(bool isFinish)
        {
            if (isFinish)
            {
                PlayerPrefs.SetInt(isGameFinish, 1);
            }
            else
            {
                PlayerPrefs.SetInt(isGameFinish, 0);
            }
        }

        public bool IsGameFinish()
        {
            if (PlayerPrefs.GetInt(isGameFinish) == 1)
            {
                return true;
            }

            return false;
        }

        public void SetPlayerScorePerLevel(int level, int value)
        {
            PlayerPrefs.SetInt(level.ToString(), value);
        }

        public int GetPlayerScorePerLevel(int level)
        {
            return PlayerPrefs.GetInt(level.ToString());
        }

        private string playerTotalScore = "PlayerTotalScore";

        public void SetPlayerTotalScore(int value)
        {
            PlayerPrefs.SetInt(playerTotalScore, value);
        }

        public int GetPlayerTotalScore()
        {
            return PlayerPrefs.GetInt(playerTotalScore);
        }

        private string settingSoundFX = "SettingSoundFX";

        public void SetSoundFX(bool value)
        {
            if (value)
            {
                PlayerPrefs.SetInt(settingSoundFX, 1);
                return;
            }

            PlayerPrefs.SetInt(settingSoundFX, 0);
        }

        public bool CheckSoundFX()
        {
            if (PlayerPrefs.GetInt(settingSoundFX) == 1)
            {
                return true;
            }

            return false;
        }

        public void Start()
        {
            PlayerPrefs.SetInt(maxLevel, setMaxLevel);
        }
    }
}
