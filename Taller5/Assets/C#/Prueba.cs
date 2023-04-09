using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code
{
    public class Prueba : MonoBehaviour
    {

        public float speed;
        public float velocity = 0;
        public float distance;

        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            float newScale = Mathf.SmoothDamp(transform.localScale.x,distance,ref velocity,speed*Time.deltaTime);
            transform.localScale = new Vector3(newScale, 1, 1);
        }
    }
}
