using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code
{
    public class Barrier : MonoBehaviour
    {
        private float life;

        void Update()
        {
            if (life <= 0) 
            {
                Destroy(gameObject);
            }
        }

        public void SetUp(float life) 
        {
            this.life = life;
        }
    }
}
