using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace game_ideas
{
    public class MainMenu_UIHandler : MonoBehaviour
    {
        public GameObject player_UI;
        public GameObject mainmenu_panel;
        public GameObject appName;
        public GameObject play_btn;
        public GameObject highscore_btn;
        public GameObject music_btn;
        public GameObject soundfx_btn;
        public GameObject exit_btn;
        public AudioManager audioManager;
        public PlayerPrefsManager playerPrefsManager;

        private Rigidbody2D player_rigidbody2D;
        private Transform player_transform;
        private float player_rotationSpeed = 200f;
        private float player_move_speed = 25f;
        private Color player_color;
        private bool gameStarted = false;
        private string startGame = "START_GAME";
        private Text text_highestGrade;
        private Toggle toggle_soundFX;

        private void Awake()
        {
            audioManager = FindObjectOfType<AudioManager>();
            player_rigidbody2D = player_UI.GetComponent<Rigidbody2D>();
            player_transform = player_UI.transform;
            player_color = player_transform.GetComponent<SpriteRenderer>().color;
            text_highestGrade = highscore_btn.transform.GetChild(0).GetComponent<Text>();
            toggle_soundFX = soundfx_btn.GetComponent<Toggle>(); 
        }

        private void Start()
        {
            gameStarted = false;
            player_color.a = 0f;
            player_transform.position = new Vector3(-50f, 25f, 0f);
            mainmenu_panel.SetActive(false);
            text_highestGrade.text = playerPrefsManager.GetPlayerTotalScore().ToString();

            if (playerPrefsManager.GetPlayerTotalScore() == 0)
            {
                highscore_btn.SetActive(false);
            }

            if (playerPrefsManager.CheckHello())
            {
                if (playerPrefsManager.CheckSoundFX())
                {
                    toggle_soundFX.isOn = true;
                }
                else
                {
                    toggle_soundFX.isOn = false;
                }
            }
            else
            {
                playerPrefsManager.SetSoundFX(true);
            }
        }

        private void Update()
        {
            PlayerRotation();

            if (gameStarted)
            {
                if (player_transform.position.x >= -50f)
                {
                    player_transform.position = new Vector3(player_transform.position.x - player_move_speed * Time.deltaTime, player_transform.position.y, 0);
                }
                else
                {
                    player_color.a -= 2f * Time.deltaTime;
                    player_transform.GetComponent<SpriteRenderer>().color = player_color;

                    if (player_color.a < 0f)
                    {
                        SceneManager.LoadScene("InGame");
                    }
                }
            }
            else
            {
                if (player_color.a >= 1f) //if player alpha is 255 then proceed
                {
                    if (player_transform.position.x <= 0f)
                    {
                        player_transform.position = new Vector3(player_transform.position.x + player_move_speed * Time.deltaTime, player_transform.position.y, 0);
                    }
                    else
                    {
                        mainmenu_panel.SetActive(true);
                    }
                }
                else //if player alpha is not 255 then add 2 values every frames until to 255 and maximum
                {
                    player_color.a += 2f * Time.deltaTime;
                    player_transform.GetComponent<SpriteRenderer>().color = player_color;
                }
            }
        }

        private void PlayerRotation()
        {
            player_rigidbody2D.rotation += player_rotationSpeed * Time.deltaTime;
            if (player_rigidbody2D.rotation > 360f)
            {
                player_rigidbody2D.rotation = 0f;
            }
        }

        public void StartPlay()
        {
            audioManager.PlayAudio(AudioName.ui_button.ToString());
            appName.GetComponent<Animator>().SetTrigger(startGame);
            play_btn.GetComponent<Animator>().SetTrigger(startGame);
            highscore_btn.GetComponent<Animator>().SetTrigger(startGame);
            //music_btn.GetComponent<Animator>().SetTrigger(startGame);
            soundfx_btn.GetComponent<Animator>().SetTrigger(startGame);
            exit_btn.GetComponent<Animator>().SetTrigger(startGame);
            gameStarted = true;
        }

        public void BestGrade()
        {
            audioManager.PlayAudio(AudioName.ui_button.ToString());
        }

        public void SoundFX()
        {
            if (toggle_soundFX.isOn)
            {
                playerPrefsManager.SetSoundFX(true);
            }
            else
            {
                playerPrefsManager.SetSoundFX(false);
            }

            audioManager.PlayAudio(AudioName.ui_button.ToString());
        }

        public void GameExit()
        {
            audioManager.PlayAudio(AudioName.ui_button.ToString());
            Application.Quit();
        }
    }
}
