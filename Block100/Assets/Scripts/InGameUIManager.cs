using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace game_ideas
{
    public class InGameUIManager : MonoBehaviour
    {
        private static InGameUIManager instance;

        public static InGameUIManager GetInstance()
        {
            return instance;
        }

        public Button pause_btn;
        public Button startButton;
        public Animator collectionStar_animator;
        public Text block_text;
        public Text scoreText;
        public Text totalScoreText;
        public Color noStarColor;
        public Color starColor;
        public Image[] starCollected;
        public GameObject levelMessage;
        public GameObject pauseMenu;
        public GameObject gameoverMenu;
        public GameObject gameFinishMenu;
        public GameManager gameManager;
        public GameAssetsManager gameAssetsManager;
        public AudioManager audioManager;
        public PlayerPrefsManager playerPrefsManager;
        
        private Animator scoreText_animator;
        private float delayDurationMenu = 2f;
        private int starValue = 0;

        private void Awake()
        {
            audioManager = FindObjectOfType<AudioManager>();
            instance = this;
            scoreText_animator = scoreText.GetComponent<Animator>();
        }

        public void Start()
        {
            if (playerPrefsManager.GetPlayerTotalScore() == 0)
            {
                totalScoreText.gameObject.SetActive(false);
            }

            SetBlock();
        }

        private void Update()
        {
            switch (gameManager.gameState)
            {
                case GameState.GAME_OVER:
                    levelMessage.SetActive(false);
                    delayDurationMenu -= 1f * Time.deltaTime;
                    if (delayDurationMenu < 0f)
                    {
                        GameOver();
                    }
                    break;
                case GameState.GAME_FINISH:
                    pause_btn.gameObject.SetActive(false);
                    collectionStar_animator.gameObject.SetActive(false);
                    scoreText.gameObject.SetActive(false);
                    delayDurationMenu -= 1f * Time.deltaTime;
                    if (delayDurationMenu < 0f)
                    {
                        AllLevelFinishGame();
                    }
                    break;
            }
        }

        public void StartGame()
        {
            if (playerPrefsManager.IsGameFinish())
            {
                gameManager.gameState = GameState.GAME_FINISH;
                Debug.Log("Game is Finish");
            }
            else
            {
                AdMobManager.instance.RequestInterstitial();
                gameManager.gameState = GameState.IN_GAME;
                pauseMenu.SetActive(false);
                startButton.interactable = false;
                //block_text.gameObject.SetActive(false);
            }
        }

        public void PauseGame()
        {
            gameManager.gameState = GameState.GAME_PAUSE;
            audioManager.PlayAudio(AudioName.ui_button.ToString());
            pauseMenu.SetActive(true);
            pause_btn.gameObject.SetActive(false);
        }

        public void ContinueGame()
        {
            gameManager.gameState = GameState.WAITING_TO_START;
            audioManager.PlayAudio(AudioName.ui_button.ToString());
            startButton.interactable = true;
            pauseMenu.SetActive(false);
            pause_btn.gameObject.SetActive(true);
        }

        public void GameOver()
        {
            AdMobManager.instance.ShowInterstitialAd();
            pause_btn.gameObject.SetActive(false);
            gameoverMenu.SetActive(true);
        }

        public void RetryGame()
        {
            audioManager.PlayAudio(AudioName.ui_button.ToString());
            SceneManager.LoadScene("InGame");
        }

        public void ReplayGame()
        {
            audioManager.PlayAudio(AudioName.ui_button.ToString());
            playerPrefsManager.SetPlayerLevel(0);
            playerPrefsManager.SetGameIsFinish(false);
            SceneManager.LoadScene("InGame");
        }

        public void MainMenu()
        {
            audioManager.PlayAudio(AudioName.ui_button.ToString());
            gameManager.gameState = GameState.TO_MAIN_MENU;
            scoreText_animator.SetTrigger(GameState.TO_MAIN_MENU.ToString());
            GetComponent<Animator>().SetTrigger(GameState.TO_MAIN_MENU.ToString());
            collectionStar_animator.SetTrigger(GameState.TO_MAIN_MENU.ToString());
            levelMessage.GetComponent<Animator>().SetTrigger(GameState.TO_MAIN_MENU.ToString());

            if (gameoverMenu.activeSelf)
            {
                gameoverMenu.GetComponent<Animator>().SetTrigger(GameState.TO_MAIN_MENU.ToString());
            }

            if (gameFinishMenu.activeSelf)
            {
                gameFinishMenu.GetComponent<Animator>().SetTrigger(GameState.TO_MAIN_MENU.ToString());
            }
        }

        public void SetBlock()
        {
            if (playerPrefsManager.GetPlayerLevel() > playerPrefsManager.GetMaxLevel())
            {
                block_text.gameObject.SetActive(false);
            }
            else
            {
                block_text.text = "BLOCK " + playerPrefsManager.GetPlayerLevel().ToString();
            }
        }

        public void SetLevelMessage(string message)
        {
            if (playerPrefsManager.GetPlayerScorePerLevel(playerPrefsManager.GetPlayerLevel()) == 0)
            {
                levelMessage.SetActive(true);
                levelMessage.GetComponentInChildren<Text>().text = message;
                StartCoroutine(DelayLevelMessageDisable(4f));
            }
        }

        public void SetScore(string value)
        {
            scoreText_animator.ResetTrigger("COLLECTING_POINTS");
            scoreText.text = value;
            scoreText_animator.SetTrigger("COLLECTING_POINTS");
        }

        public void SetTotalScore(string value)
        {
            totalScoreText.gameObject.SetActive(true);
            totalScoreText.text = value;
            SetBlock();
        }

        public void SetStar(int value)
        {
            if (value != 0)
            {
                if (value > 3)
                {
                    value = 3;
                }
                starValue = value;
                Debug.Log(starValue);
                starCollected[value - 1].color = starColor;
                switch (value)
                {
                    case 1:
                        collectionStar_animator.SetInteger("COLLECTION", 1);
                        break;
                    case 2:
                        collectionStar_animator.SetInteger("COLLECTION", 2);
                        break;
                    case 3:
                        collectionStar_animator.SetInteger("COLLECTION", 3);
                        break;
                }
            }
            else
            {
                ResetToDefault();
            }
        }

        public void FinishGameLevel()
        {
            collectionStar_animator.SetInteger("FINISH_LEVEL_COLLECTED", starValue);
            foreach (Image i in starCollected)
            {
                GameObject starEffect = Instantiate(gameAssetsManager.starParticleEffect, i.transform.position, Quaternion.identity, i.transform) as GameObject;
            }
        }

        public void AllLevelFinishGame()
        {
            gameFinishMenu.SetActive(true);
        }

        public void ResetToDefault()
        {
            StartCoroutine(DelayReset(.5f));
        }

        private IEnumerator DelayReset (float seconds = 1f)
        {
            yield return new WaitForSeconds(seconds);

            scoreText_animator.ResetTrigger("COLLECTING_POINTS");
            collectionStar_animator.SetInteger("COLLECTION", 0);
            collectionStar_animator.SetInteger("FINISH_LEVEL_COLLECTED", 0);

            foreach (Image i in starCollected)
            {
                i.color = noStarColor;
            }
        }

        private IEnumerator DelayLevelMessageDisable(float seconds = 1)
        {
            yield return new WaitForSeconds(seconds);

            levelMessage.SetActive(false);
        }
    }
}
