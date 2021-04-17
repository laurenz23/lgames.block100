using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace game_ideas
{
    public enum AudioName
    {
        ui_button,
        player_jump,
        triangle_collected,
        square_collected,
        star_collected,
        buff_collected,
        all_star_collected,
        dash,
        finish,
        death
    }

    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;

        private PlayerPrefsManager playerPrefsManager;

        [SerializeField] private Audio[] audios = null;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);

            playerPrefsManager = FindObjectOfType<PlayerPrefsManager>();

            if (playerPrefsManager == null)
            {
                Debug.LogError("AudioManager : PlayerPrefsManager reference is missing");
            }

            foreach (Audio a in audios)
            {
                a.source = gameObject.AddComponent<AudioSource>();
                a.source.clip = a.clip;
                a.source.loop = a.loop;
                a.source.volume = a.volume;
                a.source.pitch = a.pitch;
            }
        }

        public void PlayAudio(string name, bool checkIfAudioPlaying = false)
        {
            if (playerPrefsManager.CheckSoundFX())
            {
                Audio aud = Array.Find(audios, a => a.name == name);
                if (aud == null)
                {
                    Debug.LogWarning("Audio: " + name + " not found");
                    return;
                }

                if (checkIfAudioPlaying)
                {
                    if (!aud.source.isPlaying)
                    {
                        aud.source.Play();
                    }
                }
                else
                {
                    aud.source.Play();
                }
            }
        }
    }
}
