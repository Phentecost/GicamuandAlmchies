using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code
{
    public class Pilar : MonoBehaviour
    {
        private float _velocity;
        private float _distance;
        private float _speed;
        private float _timer;

        public enum States { Up, Hold, Down }
        public States _states = States.Up;

        
        public void SetUp(float distance, float speed, float timer) 
        {
            _distance= distance;
            _speed= speed;
            _timer= timer;
            _velocity= 0;
        }

        void Update()
        {
            switch (_states) 
            {
                case States.Up:

                    float newScale = Mathf.SmoothDamp(transform.localScale.y, _distance, ref _velocity, _speed * Time.deltaTime);
                    transform.localScale = new Vector3(1, newScale, 1);

                    if (transform.localScale.y > _distance-0.5)
                    {
                        _states= States.Hold;
                    }

                    break;

                case States.Hold:

                    if (_timer <= 0)
                    {
                        _states = States.Down;
                        _velocity = 0;
                    }
                    else
                    {
                        _timer -= Time.deltaTime;
                    }

                    break;

                case States.Down:

                    float newScale2 = Mathf.SmoothDamp(transform.localScale.y, 0, ref _velocity, _speed * Time.deltaTime);
                    transform.localScale = new Vector3(1, newScale2 , 1);

                    if (transform.localScale.y < 0+.1)
                    {
                        Destroy(gameObject);
                    }

                    break;
                
            }
        }
    }
}
