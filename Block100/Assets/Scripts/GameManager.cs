using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// handles the game settings
/// </summary>

namespace game_ideas
{
    public enum GameDifficulty
    {
        EASY,
        MEDIUM,
        HARD,
        EXPERT,
        IMPOSSIBLE
    }

    public enum GameState
    {
        WAITING_TO_START,
        IN_GAME,
        GAME_PAUSE,
        GAME_OVER,
        GAME_FINISH,
        TO_MAIN_MENU
    }

    public enum GameTag
    {
        Respawn,
        Finish,
        Player,
        Obstacle,
        Objective,
        Powerups
    }

    public enum GamePowerups
    {
        None,
        Dash,
        Magnet,
        Flaw
    }

    public enum GameObjectiveType
    {
        None,
        Triangle, //+3 points - 15
        Square, //+5 points - 5
        Star //+15 points - 3
    }

    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;

        public static GameManager GetInstane()
        {
            return instance;
        }

        public GameState gameState;
        public GamePowerups gamePowerups;

        private float gameTime = 0f;
        private float delayTimeToMainMenu = 2f;

        private void Awake()
        {
            gameState = GameState.WAITING_TO_START;
            instance = this;
        }

        private void Update()
        {
            switch (gameState)
            {
                case GameState.WAITING_TO_START:
                    Time.timeScale = 1f;
                    break;
                case GameState.IN_GAME:
                    gameTime += 1f * Time.deltaTime;
                    break;
                case GameState.GAME_PAUSE:
                    Time.timeScale = 0f;
                    break;
                case GameState.GAME_OVER:
                    break;
                case GameState.GAME_FINISH:

                    break;
                case GameState.TO_MAIN_MENU:
                    Time.timeScale = 1f;
                    if (delayTimeToMainMenu <= 0f)
                    {
                        SceneManager.LoadScene("MainMenu");
                    }
                    else
                    {
                        delayTimeToMainMenu -= 1f * Time.deltaTime;
                    }
                    break;
            }
        }

        public float GetGameTime()
        {
            return gameTime;
        }
    }
}
