using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game_ideas
{
    public class ObjectivesHandler : MonoBehaviour
    {
        public GameObjectiveType objectiveType;
        public float rotationSpeed;
        
        private Rigidbody2D RIGIDBODY2D;

        private void Awake()
        {
            RIGIDBODY2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            RIGIDBODY2D.rotation += rotationSpeed * Time.deltaTime;
        }
    }
}
