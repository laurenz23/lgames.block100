using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game_ideas
{
    public class KeyboardInput : MonoBehaviour
    {
        private InputManager inputManager;

        private void Awake()
        {
            inputManager = FindObjectOfType<InputManager>();
        }

        private void Update()
        {
            if (inputManager.GetInputType() == InputType.KEYBOARD)
            {
                if (Input.GetKey(KeyCode.Space))
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
