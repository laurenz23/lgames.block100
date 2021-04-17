using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game_ideas
{
    public class ParticleHandler : MonoBehaviour
    {
        public float delayToDestroy;

        private void Start()
        {
            Destroy(this.gameObject, delayToDestroy);
        }
    }
}
