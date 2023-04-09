using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code_Proyectiles
{
    public class Proyectile : MonoBehaviour
    {
        private float speed;
        private Vector3 direction;
        // Update is called once per frame
        void Update()
        {
            transform.position += speed * direction * Time.deltaTime;
        }

        public void SetMovement(float speed, Vector3 direction) 
        {
            this.speed = speed;
            this.direction = direction;
        }
    }
}
