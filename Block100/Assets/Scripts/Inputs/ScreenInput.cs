using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game_ideas
{
    public class ScreenInput : MonoBehaviour
    {
        private InputManager inputManager;

        private void Awake()
        {
            inputManager = FindObjectOfType<InputManager>();
        }

        private void Update()
        {
            if (inputManager.GetInputType() == InputType.ONSCREEN)
            {
                if (Input.GetMouseButton(0))
                {
                    VirtualInput.Instance.jump = true;
                }
                else
                {
                    VirtualInput.Instance.jump = false;
                }
            }
        }
    }
}
