using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code_Proyectiles
{
    public class Proyectile : MonoBehaviour
    {
        private float _speed;
        private Vector3 _direction;
        private bool _bounce;
        private int _nBounces;
        // Update is called once per frame
        void Update()
        {
            transform.position += _speed * _direction * Time.deltaTime;
        }

        public void SetMovement(float speed, Vector3 direction, bool bounce) 
        {
            this._speed = speed;
            this._direction = direction;
            this._bounce = bounce;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("xd");
            if (_bounce)
            {
                Debug.Log("SuperXD");
                _direction = Vector3.Reflect(_direction, collision.contacts[0].normal);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }
}
