using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game_ideas
{
    public class PlayerCollisionHandler : MonoBehaviour
    {
        public AudioManager audioManager;
        public ScoreManager scoreManager;
        public PlayerPrefsManager playerPrefsManager;
        public LevelManager levelManager;

        private PlayerManager playerManager;

        private int starCollected = 0;

        private void Awake()
        {
            audioManager = FindObjectOfType<AudioManager>();

            if (scoreManager == null)
            {
                Debug.LogError("Player Collision Handler: scoreManager is missing");
            }

            if (playerPrefsManager == null)
            {
                Debug.LogError("Player Collision Handler: playerPrefsManager is missing");
            }

            if (levelManager == null)
            {
                Debug.LogError("Player Collision Handler: levelManager is missing");
            }

            if (playerManager == null)
            {
                playerManager = GetComponentInParent<PlayerManager>();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(GameTag.Objective.ToString()))
            {
                if (collision.GetComponent<ObjectivesHandler>())
                {
                    switch (collision.GetComponent<ObjectivesHandler>().objectiveType)
                    {
                        case GameObjectiveType.Triangle:
                            audioManager.PlayAudio(AudioName.triangle_collected.ToString());
                            playerManager.CollectionEffect(collision.transform, GameObjectiveType.Triangle);
                            scoreManager.AddPoints(scoreManager.trianglePoints);
                            CreatePointsEffect(collision.gameObject, collision.transform.parent, scoreManager.trianglePoints);
                            break;
                        case GameObjectiveType.Square:
                            audioManager.PlayAudio(AudioName.square_collected.ToString());
                            playerManager.CollectionEffect(collision.transform, GameObjectiveType.Square);
                            scoreManager.AddPoints(scoreManager.squarePoints);
                            CreatePointsEffect(collision.gameObject, collision.transform.parent, scoreManager.squarePoints);
                            break;
                        case GameObjectiveType.Star:
                            playerManager.CollectionEffect(collision.transform, GameObjectiveType.Star);
                            scoreManager.AddPoints(scoreManager.starPoints);
                            CreatePointsEffect(collision.gameObject, collision.transform.parent, scoreManager.starPoints);
                            scoreManager.AddStar(1);
                            starCollected++;
                            break;
                    }

                    if (scoreManager.NewHighestScore())
                    {
                        GameObject newHighestScoreEffect = Instantiate(playerManager.gameAssetsManager.newHighestScoreEffect, new Vector2(collision.transform.position.x + 10f, collision.transform.position.y + 15f), Quaternion.identity, collision.transform.parent) as GameObject;
                        Destroy(newHighestScoreEffect, 2f);
                        scoreManager.alreadySetNewHighestScore = true;
                    }

                    Destroy(collision.gameObject);
                }
            }
            else if (collision.CompareTag(GameTag.Obstacle.ToString()))
            {
                playerManager.CharacterDeath();
                scoreManager.ResetScore();
                audioManager.PlayAudio(AudioName.death.ToString());
            }

            if (collision.CompareTag(GameTag.Powerups.ToString()))
            {
                if (collision.GetComponent<PowerupsHandler>())
                {
                    //audioManager.PlayAudio(AudioName.buff_collected.ToString());
                    switch (collision.GetComponent<PowerupsHandler>().gamePowerups)
                    {
                        case GamePowerups.Dash:
                            audioManager.PlayAudio(AudioName.dash.ToString());
                            playerManager.CharacterJump(playerManager.dashJump);
                            playerManager.CharacterGravity(playerManager.dashGravity);
                            playerManager.gameManager.gamePowerups = GamePowerups.Dash;
                            playerManager.PlayerDash.position = new Vector3(25f, playerManager.PlayerTransform.transform.position.y, 0f);
                            break;
                        case GamePowerups.Magnet:
                            audioManager.PlayAudio(AudioName.buff_collected.ToString());
                            playerManager.gameManager.gamePowerups = GamePowerups.Magnet;
                            break;
                        case GamePowerups.Flaw:
                            audioManager.PlayAudio(AudioName.buff_collected.ToString());
                            playerManager.CharacterJump(20f);
                            playerManager.CharacterGravity(playerManager.flawGravity);
                            playerManager.gameManager.gamePowerups = GamePowerups.Flaw;
                            break;
                    }
                }

                Destroy(collision.gameObject);
            }

            if (collision.CompareTag(GameTag.Finish.ToString()))
            {
                playerManager.inGameUIManager.FinishGameLevel();
                audioManager.PlayAudio(AudioName.finish.ToString());
                playerPrefsManager.SetPlayerLevel(playerPrefsManager.GetPlayerLevel() + 1);

                switch (starCollected)
                {
                    case 1:
                        collision.transform.GetChild(1).GetComponent<SpriteRenderer>().color = playerManager.inGameUIManager.starColor;
                        collision.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
                        break;
                    case 2:
                        collision.transform.GetChild(2).GetComponent<SpriteRenderer>().color = playerManager.inGameUIManager.starColor;
                        collision.transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
                        collision.transform.GetChild(1).GetComponent<SpriteRenderer>().color = playerManager.inGameUIManager.starColor;
                        collision.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
                        break;
                    case 3:
                        collision.transform.GetChild(3).GetComponent<SpriteRenderer>().color = playerManager.inGameUIManager.starColor;
                        collision.transform.GetChild(3).GetChild(0).gameObject.SetActive(true);
                        collision.transform.GetChild(2).GetComponent<SpriteRenderer>().color = playerManager.inGameUIManager.starColor;
                        collision.transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
                        collision.transform.GetChild(1).GetComponent<SpriteRenderer>().color = playerManager.inGameUIManager.starColor;
                        collision.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
                        break;
                }

                ResetToDefault();
                scoreManager.ResetScore();
                
                if (levelManager.CheckIfAllLevelCompleted())
                {
                    playerPrefsManager.SetGameIsFinish(true);

                    if (playerPrefsManager.GetPlayerTotalScore() == 100 * 5)
                    {
                        collision.transform.GetChild(0).GetComponent<TextMesh>().text = "PERFECT Score: " + playerPrefsManager.GetPlayerTotalScore() + "     Arrivederci";
                    }
                    else
                    {
                        collision.transform.GetChild(0).GetComponent<TextMesh>().text = "Score: " + playerPrefsManager.GetPlayerTotalScore() + "     Arrivederci";
                    }

                    return;
                }

                if (collision.transform.GetChild(0).GetComponent<TextMesh>())
                {
                    if (playerPrefsManager.GetPlayerScorePerLevel(playerPrefsManager.GetPlayerLevel() - 1) == 100)
                    {
                        collision.transform.GetChild(0).GetComponent<TextMesh>().text = "BLOCK " + (playerPrefsManager.GetPlayerLevel() - 1) + "      PERFECT Score: " + playerPrefsManager.GetPlayerTotalScore();
                    }
                    else
                    {
                        collision.transform.GetChild(0).GetComponent<TextMesh>().text = "BLOCK " + (playerPrefsManager.GetPlayerLevel() - 1) + "      Score: " + playerPrefsManager.GetPlayerTotalScore();
                    }
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(GameTag.Finish.ToString()))
            {
                levelManager.SetupLevel();
            }
        }

        private void CreatePointsEffect(GameObject collision, Transform parent, int score, float delayForDestroy = 1.5f)
        {
            GameObject poinstEffect = Instantiate(playerManager.gameAssetsManager.popupTextEffect, new Vector2(collision.transform.position.x + 10f, collision.transform.position.y + 7.5f), Quaternion.identity, parent) as GameObject;
            poinstEffect.transform.GetChild(0).GetComponent<TextMesh>().text = "+" + score;
            Destroy(poinstEffect, delayForDestroy);
        }

        private void ResetToDefault()
        {
            starCollected = 0;
        }
    }
}
