using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game_ideas
{
    public class PlatformHandler : MonoBehaviour
    {
        public GameManager gameManager;

        private void Update()
        {
            if (gameManager.gameState == GameState.TO_MAIN_MENU)
            {
                GetComponent<Animator>().SetTrigger(GameState.TO_MAIN_MENU.ToString());
            }
        }
    }
}
