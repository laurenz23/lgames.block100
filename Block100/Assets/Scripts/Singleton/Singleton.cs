﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game_ideas
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    instance = obj.AddComponent<T>();
                    obj.name = typeof(T).ToString();
                }

                return instance;
            }
        }
    }
}
