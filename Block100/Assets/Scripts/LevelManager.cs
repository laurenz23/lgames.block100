using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace game_ideas
{
    /*[CustomEditor(typeof(LevelManager))]
    public class LevelManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            LevelManager levelManager = (LevelManager)target;
            if (GUILayout.Button("Setup Level Render"))
            {
                levelManager.SetupLevel();
            }
        }
    }*/

    public class LevelManager : MonoBehaviour
    {
        public GameObject[] levelObjects;
        public PlayerPrefsManager playerPrefsManager;
        public InGameUIManager inGameUIManager;

        private ObstacleHandler obstacle;
        private GameManager gameManager;
        private PlayerManager playerManager;


        private int currentLevel;
        private float dashDuration = 1f;
        private float dashCurrentDuration;

        private SpriteRenderer[] levelAllSpriteRenderer;
        private TextMesh[] levelAllTextMesh;
        private Color colorAlpha;

        private Dictionary<int, string> DictionaryLevelMessage = new Dictionary<int, string>();

        private void Awake()
        {
            obstacle = FindObjectOfType<ObstacleHandler>();
            playerManager = FindObjectOfType<PlayerManager>();
            gameManager = GameManager.GetInstane();

            if (playerPrefsManager.GetPlayerLevel() == 0)
            {
                playerPrefsManager.SetPlayerLevel(1);
            }

            currentLevel = playerPrefsManager.GetPlayerLevel();
        }

        private void Start()
        {
            dashCurrentDuration = dashDuration;
            SetupDictionary();
            SetupLevel();

            //PlayerPrefs.DeleteKey("IsGameFinish");
        }

        private void Update()
        {
            switch (gameManager.gameState)
            {
                case GameState.WAITING_TO_START:
                    break;
                case GameState.IN_GAME:
                    if (gameManager.gamePowerups == GamePowerups.Dash)
                    {
                        dashCurrentDuration -= 1f * Time.deltaTime;
                        if (dashCurrentDuration > 0f)
                        {
                            LevelMove(currentLevel - 1, 100f);
                            playerManager.PlayerDash.gameObject.SetActive(true);
                        }
                        else
                        {
                            dashCurrentDuration = dashDuration;
                            gameManager.gamePowerups = GamePowerups.None;
                            playerManager.PlayerDash.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        dashCurrentDuration = dashDuration;
                        LevelMove(currentLevel - 1);
                    }
                    break;
                case GameState.GAME_OVER:
                    break;
                case GameState.GAME_FINISH:
                    LevelMove(levelObjects.Length - 1);
                    break;
                case GameState.TO_MAIN_MENU:
                    FadeOut(currentLevel - 1);
                    break;
            }
        }

        public void LevelMove(int level, float moveSpeed = 30f)
        {
            levelObjects[level].transform.Translate(-Vector2.right * moveSpeed * Time.deltaTime);
        }

        public void FadeOut(int level)
        {
            if (colorAlpha.a >= 0f)
            {
                colorAlpha.a -= 1f * Time.deltaTime;
            }
            else
            {
                levelAllSpriteRenderer = levelObjects[level].GetComponentsInChildren<SpriteRenderer>();
                levelAllTextMesh = levelObjects[level].GetComponentsInChildren<TextMesh>();

                foreach (SpriteRenderer s in levelAllSpriteRenderer)
                {
                    s.color = colorAlpha;
                }

                foreach (TextMesh t in levelAllTextMesh)
                {
                    t.color = colorAlpha;
                }
            }
        }

        public Transform GetCurrentLevel()
        {
            return levelObjects[currentLevel - 1].transform;
        }

        public void SetupLevel()
        {
            if (!playerPrefsManager.IsGameFinish())
            {
                currentLevel = playerPrefsManager.GetPlayerLevel();

                foreach (GameObject o in levelObjects)
                {
                    o.SetActive(false);
                }

                levelObjects[currentLevel - 1].SetActive(true);
                inGameUIManager.SetLevelMessage(DictionaryLevelMessage[currentLevel]);
            }
        }

        public bool CheckIfAllLevelCompleted()
        {
            if (playerPrefsManager.GetPlayerLevel() > levelObjects.Length)
            {
                StartCoroutine(GameIsFinishDelay(2f));
                return true;
            }

            return false;
        }

        public IEnumerator GameIsFinishDelay(float delayTime = 1f)
        {
            yield return new WaitForSeconds(delayTime);
            gameManager.gameState = GameState.GAME_FINISH;
        }

        private void SetupDictionary()
        {
            DictionaryLevelMessage.Add(1, "BLOCK 1");
            DictionaryLevelMessage.Add(2, "BLOCK 2");
            DictionaryLevelMessage.Add(3, "BLOCK 3");
            DictionaryLevelMessage.Add(4, "BLOCK 4");
            DictionaryLevelMessage.Add(5, "BLOCK 5");
        }
    }
}
