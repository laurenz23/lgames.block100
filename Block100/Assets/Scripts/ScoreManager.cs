using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game_ideas
{
    public class ScoreManager : MonoBehaviour
    {
        public AudioManager audioManager;
        public PlayerPrefsManager playerPrefsManager;
        private static ScoreManager instance;

        public static ScoreManager GetInstance()
        {
            return instance;
        }

        public int scorePerLevel = 0;
        public int starCollected = 0;
        public int trianglePoints = 3;
        public int squarePoints = 5;
        public int starPoints = 15;
        public bool alreadySetNewHighestScore = false;

        public InGameUIManager inGameUIManager;

        private int totalScore = 0;
        private int currentLevel;

        private bool setNewHighestScore = false;

        private void Awake()
        {
            instance = this;
            audioManager = FindObjectOfType<AudioManager>();

            if (playerPrefsManager == null)
            {
                Debug.LogError("Score Manager: Player Prefs Manager is missing");
            }
        }

        private void Start()
        {
            inGameUIManager.scoreText.text = scorePerLevel.ToString();
            ResetScore();
        }

        public void AddPoints(int additionScore)
        {
            totalScore += additionScore;
            scorePerLevel += additionScore;
            inGameUIManager.SetScore(scorePerLevel.ToString());

            if (scorePerLevel > playerPrefsManager.GetPlayerScorePerLevel(currentLevel))
            {
                setNewHighestScore = true;
            }
        }

        public void AddStar(int addStar)
        {
            starCollected += addStar;

            if (starCollected > 3)
            {
                audioManager.PlayAudio(AudioName.all_star_collected.ToString());
                starCollected = 3;
            }
            else
            {
                audioManager.PlayAudio(AudioName.star_collected.ToString());
            }

            inGameUIManager.SetStar(starCollected);
        }

        public void SetScore()
        {
            if (scorePerLevel > playerPrefsManager.GetPlayerScorePerLevel(currentLevel))
            {
                playerPrefsManager.SetPlayerScorePerLevel(currentLevel, scorePerLevel);

                int total = 0;
                for (int i = 0; i < currentLevel; i++)
                {
                    total += playerPrefsManager.GetPlayerScorePerLevel(i + 1);
                }

                playerPrefsManager.SetPlayerTotalScore(total);
            }
        }

        public bool NewHighestScore()
        {
            if (!alreadySetNewHighestScore)
            {
                return setNewHighestScore;
            }

            return false;
        }

        public void ResetScore()
        {
            SetScore();
            totalScore = playerPrefsManager.GetPlayerTotalScore();
            currentLevel = playerPrefsManager.GetPlayerLevel();
            inGameUIManager.SetStar(starCollected);
            inGameUIManager.scoreText.text = "0";
            inGameUIManager.SetTotalScore("HIGHEST SCORE : " + totalScore.ToString());
            inGameUIManager.ResetToDefault();
            scorePerLevel = 0;
            starCollected = 0;
        }
        
    }
}
