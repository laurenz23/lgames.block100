using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game_ideas
{
    public class PlayerManager : MonoBehaviour
    {
        private static PlayerManager instance;

        public static PlayerManager GetInstance()
        {
            return instance;
        }

        public Transform PlayerTransform;
        public Transform PlayerMagnet;
        public Transform PlayerDash;
        public Transform PlayerFlaw;
        public Animator PlayerAnimator;
        public Rigidbody2D RIGIDBODY2D;
        public BoxCollider2D BoxCollider2D;
        public LevelManager levelManager;
        public GameManager gameManager;
        public GameAssetsManager gameAssetsManager;
        public AudioManager audioManager;
        public InGameUIManager inGameUIManager;

        //movements
        public bool jump;
        public const float JUMP_FORCE = 70f;
        public const float ROTATION_SPEED = 200f;
        public const float GRAVITY_FORCE = 30f;
        public const float MOVE_SPEED = 25f;

        //buff
        public float flawJump = 40f;
        public float flawGravity = 10.5f;
        private float flawDuration = 10f;
        private float currentFlawDuration;

        public float dashJump = 10f;
        public float dashGravity = 5f;

        private void Awake()
        {
            instance = this;
            audioManager = FindObjectOfType<AudioManager>();
            gameManager = FindObjectOfType<GameManager>();
            gameAssetsManager = GameAssetsManager.GetInstance();
        }

        private void Start()
        {
            RIGIDBODY2D.gravityScale = GRAVITY_FORCE;
            currentFlawDuration = flawDuration;
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.X))
            {
                PlayerPrefs.DeleteAll();
            }

            switch (gameManager.gameState)
            {
                case GameState.WAITING_TO_START:
                    RIGIDBODY2D.bodyType = RigidbodyType2D.Static;
                    CharacterRotation();
                    break;
                case GameState.IN_GAME:
                    //PlayerTransform.localPosition = new Vector2(PlayerTransform.localPosition.x + 30f * Time.deltaTime, PlayerTransform.localPosition.y);
                    RIGIDBODY2D.bodyType = RigidbodyType2D.Dynamic;

                    switch (gameManager.gamePowerups)
                    {
                        case GamePowerups.Dash:
                            CharacterRotation(400f);

                            PlayerMagnet.GetChild(0).gameObject.SetActive(false); //magnet effect
                            PlayerFlaw.gameObject.SetActive(false); //flaw effect
                            currentFlawDuration = flawDuration; //flaw

                            if (jump)
                            {
                                CharacterJump(dashJump);
                                audioManager.PlayAudio(AudioName.player_jump.ToString(), true);
                            }

                            CharacterGravity(dashGravity);

                            break;
                        case GamePowerups.Magnet:

                            CharacterRotation();

                            PlayerMagnet.position = PlayerTransform.position;
                            PlayerDash.gameObject.SetActive(false); //dash effect
                            PlayerMagnet.GetChild(0).gameObject.SetActive(true); //magnet effect
                            PlayerFlaw.gameObject.SetActive(false); //flaw effect
                            currentFlawDuration = flawDuration; //flaw
                            
                            if (jump)
                            {
                                CharacterJump();
                                audioManager.PlayAudio(AudioName.player_jump.ToString(), true);
                            }

                            CharacterGravity();

                            break;
                        case GamePowerups.Flaw:

                            CharacterRotation();

                            PlayerDash.gameObject.SetActive(false); //dash effect
                            PlayerMagnet.GetChild(0).gameObject.SetActive(false); //magnet effect
                            PlayerFlaw.gameObject.SetActive(true); //flaw effect
                            PlayerFlaw.position = PlayerTransform.position;
                            currentFlawDuration -= 1f * Time.deltaTime;

                            if (currentFlawDuration > 0f)
                            {
                                if (jump)
                                {
                                    CharacterJump(flawJump);
                                    audioManager.PlayAudio(AudioName.player_jump.ToString(), true);
                                }
                            }
                            else
                            {
                                gameManager.gamePowerups = GamePowerups.None;
                            }

                            CharacterGravity(flawGravity);

                            break;
                        default:
                            CharacterRotation();
                            BuffEffectReset();
                            if (jump)
                            {
                                CharacterJump();
                                audioManager.PlayAudio(AudioName.player_jump.ToString(), true);
                            }

                            CharacterGravity();

                            break;
                    }
                    break;
                case GameState.GAME_OVER:
                    RIGIDBODY2D.bodyType = RigidbodyType2D.Static;
                    BuffEffectReset();
                    break;
                case GameState.GAME_FINISH:
                    RIGIDBODY2D.bodyType = RigidbodyType2D.Static;
                    CharacterRotation();
                    BuffEffectReset();

                    if (PlayerTransform.transform.localPosition.y > 0.9)
                    {
                        PlayerTransform.transform.localPosition = new Vector3(0f, PlayerTransform.localPosition.y - MOVE_SPEED * Time.deltaTime, 0f);
                    }

                    if (PlayerTransform.transform.localPosition.y < -0.9)
                    {
                        PlayerTransform.transform.localPosition = new Vector3(0f, PlayerTransform.localPosition.y + MOVE_SPEED * Time.deltaTime, 0f);
                    }

                    break;
                case GameState.TO_MAIN_MENU:
                    RIGIDBODY2D.bodyType = RigidbodyType2D.Static;
                    PlayerDash.gameObject.SetActive(false); //dash effect
                    PlayerMagnet.GetChild(0).gameObject.SetActive(false); //magnet effect
                    PlayerFlaw.gameObject.SetActive(false); //flaw effect
                    CharacterRotation();
                    PlayerAnimator.SetTrigger(GameState.TO_MAIN_MENU.ToString());
                    break;
            }
        }

        public void CharacterJump(float jumpForce = JUMP_FORCE)
        {
            RIGIDBODY2D.velocity = Vector2.up * jumpForce;
            //gameManager.audioManager.PlayAudio(AudioName.player_jump.ToString());
        }

        public void CharacterGravity(float gravityForce = GRAVITY_FORCE)
        {
            // gravity force for flaw 10.5f
            // gravity force for dash 5f
            RIGIDBODY2D.gravityScale = gravityForce;
        }

        public void CharacterRotation(float rotationSpeed = ROTATION_SPEED)
        {
            RIGIDBODY2D.rotation += rotationSpeed * Time.deltaTime;
        }

        public void CharacterDeath()
        {
            DeathEffect();
            PlayerTransform.gameObject.SetActive(false);
            gameManager.gameState = GameState.GAME_OVER;
        }

        public void BuffEffectReset()
        {
            PlayerDash.gameObject.SetActive(false); //dash effect
            PlayerMagnet.GetChild(0).gameObject.SetActive(false); //magnet effect
            PlayerFlaw.gameObject.SetActive(false);
            currentFlawDuration = flawDuration; //flaw
        }

        public void JumpEffect()
        {
            GameObject playerJumpEffect = Instantiate(gameAssetsManager.playerJumpEffect) as GameObject;
            playerJumpEffect.transform.parent = FindObjectOfType<ObstacleHandler>().transform;
            playerJumpEffect.transform.position = PlayerTransform.position;
        }

        public void DeathEffect()
        {
            GameObject playerDeathEffect = Instantiate(gameAssetsManager.playerExplosionEffect) as GameObject;
            playerDeathEffect.transform.parent = this.transform;
            playerDeathEffect.transform.localPosition = PlayerTransform.localPosition;
            gameAssetsManager.gameCamera.GetComponent<Animator>().SetTrigger("Shake");
        }

        public void CollectionEffect(Transform _transform, GameObjectiveType objectiveType)
        {
            GameObject playerCollectionEffect = null;

            switch (objectiveType)
            {
                case GameObjectiveType.Triangle:
                    playerCollectionEffect = Instantiate(gameAssetsManager.triangleParticleEffect) as GameObject;
                    break;
                case GameObjectiveType.Square:
                    playerCollectionEffect = Instantiate(gameAssetsManager.squareParticleEffect) as GameObject;
                    break;
                case GameObjectiveType.Star:
                    playerCollectionEffect = Instantiate(gameAssetsManager.starParticleEffect) as GameObject;
                    break;
            }
            
            if (playerCollectionEffect != null)
            {
                playerCollectionEffect.transform.parent = levelManager.GetCurrentLevel().transform;
                playerCollectionEffect.transform.localPosition = _transform.localPosition;
            }
        }
    }
}
