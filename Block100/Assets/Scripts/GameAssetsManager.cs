using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game_ideas
{
    public class GameAssetsManager : MonoBehaviour
    {
        private static GameAssetsManager instance;

        public static GameAssetsManager GetInstance()
        {
            return instance;
        }

        private void Awake()
        {
            instance = this;
        }

        public GameObject gameCamera;
        public GameObject playerExplosionEffect;
        public GameObject playerJumpEffect;
        public GameObject triangleParticleEffect;
        public GameObject squareParticleEffect;
        public GameObject starParticleEffect;
        public GameObject popupTextEffect;
        public GameObject newHighestScoreEffect;
    }
}
