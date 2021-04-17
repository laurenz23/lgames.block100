using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game_ideas
{
    public enum InputType
    {
        KEYBOARD,
        ONSCREEN
    }

    public class InputManager : MonoBehaviour
    {
        public InputType inputType;

        public InputType GetInputType()
        {
            if (inputType == InputType.KEYBOARD)
            {
                return InputType.KEYBOARD;
            }
            else
            {
                return InputType.ONSCREEN;
            }
        }
    }
}
