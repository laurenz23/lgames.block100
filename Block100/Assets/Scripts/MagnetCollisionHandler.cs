using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game_ideas
{
    public class MagnetCollisionHandler : MonoBehaviour
    {
        public PlayerManager playerManager;
        public GameManager gameManager;
        private float speed = 50f;
        private float magnetDuration = 5f;
        private float currentMagnetDuration;

        private void Awake()
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        private void Start()
        {
            currentMagnetDuration = magnetDuration;
        }

        private void Update()
        {
            if (gameManager.gameState == GameState.IN_GAME)
            {
                if (gameManager.gamePowerups == GamePowerups.Magnet)
                {
                    currentMagnetDuration -= 1f * Time.deltaTime;
                    if (currentMagnetDuration < 0)
                    {
                        currentMagnetDuration = magnetDuration;
                        gameManager.gamePowerups = GamePowerups.None;
                    }
                }
                else
                {
                    currentMagnetDuration = magnetDuration;
                }
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (gameManager.gamePowerups == GamePowerups.Magnet)
            {
                if (collision.GetComponent<ObjectivesHandler>())
                {
                    collision.transform.position = Vector2.MoveTowards(collision.transform.position, playerManager.PlayerTransform.position, speed * Time.deltaTime);
                }
            }
        }
    }
}
