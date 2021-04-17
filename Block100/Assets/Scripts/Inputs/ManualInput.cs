using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game_ideas
{
    public class ManualInput : MonoBehaviour
    {
        private PlayerManager playerManager;

        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
        }

        private void Update()
        {
            if (VirtualInput.Instance.jump)
            {
                playerManager.jump = true;
            }
            else
            {
                playerManager.jump = false;
            }
        }
    }
}
