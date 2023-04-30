using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Code;
using Unity.VisualScripting;
using Code_Proyectiles;

namespace Code_EnemiesAndAI
{
    public class Homunculo : Enemy
    {
        [SerializeField] protected float jumpThreshold = 5;

        #region Behaviour
        protected override void Behaviour()
        {
           
            _target = GetClosestPlayer();
            distance = Vector2.Distance(_target.transform.position, transform.position);
            CalculateCollisions();
            CalculateGravity();

            switch (base._currentState) 
            {
                case State.Idle:

                    if (_waitForTimer <= 0)
                    {
                        _currentState = State.Walking;
                        _waitForTimer = 1;
                    }
                    else
                    {
                        _waitForTimer -= Time.deltaTime;
                    }
                    MoveCharacter();
                    break;

                case State.Walking:

                    if (distance < attackTheshold) 
                    {
                        _currentState = State.Shooting;
                    }
                    else
                    {
                        CalculateWalk(_target);
                        if (distance < jumpThreshold && waitForTimerJMP <= 0)
                        {
                            CalculateJump(_target);
                            waitForTimerJMP = 3;    
                        }
                        else
                        {
                            waitForTimerJMP -= Time.deltaTime;
                        }
                        
                    }

                    MoveCharacter();
                    break;

                case State.Shooting:

                    if (waitForTimerATK <= 0)
                    {
                        Attack();

                        if (distance > attackTheshold)
                        {
                            _currentState = State.Walking;
                        }
                    }
                    else
                    {
                        waitForTimerATK -= Time.deltaTime;
                    }

                    _currentHorizontalSpeed= 0;
                    MoveCharacter();

                    break;

            }
        }

        #endregion

        #region Attack

        [SerializeField] protected GameObject bulletPrefab;
        [SerializeField] protected float waitForTimerATK = 3;
        [SerializeField] protected float waitForTimerJMP = 0;
        [SerializeField] protected float ATKspeed;

        protected override void Attack()
        {
            Vector3 vec2tar = _target.transform.position - transform.position;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.transform.parent = null;
            bullet.GetComponent<Proyectile>().SetMovement(ATKspeed, vec2tar,false);
            waitForTimerATK = 3;

        }
        #endregion

    }
}
