using Code;
using Code_Proyectiles;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

namespace Code_EnemiesAndAI
{
    public class SapoQuimera : Enemy
    {

        #region Behaviour
        protected override void Behaviour()
        {
            /*if (!_currentRoom.roomBounds.Contains(transform.position - _currentRoom.transform.position))
            {
                dead();
            }*/

            _target = GetClosestPlayer();
            distance = Vector2.Distance(_target.transform.position, transform.position);
            CalculateCollisions();
            CalculateCollitionBehaviour();
            CalculateGravity();
            float wait = 1;

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

                    CalculateJump(_target);
                    CalculateWalk(_target);
                    MoveCharacter();

                    if (distance <= attackTheshold)
                    {
                        _currentState = State.Shooting;
                        wait = 1;
                    }

                    break;

                case State.Shooting:

                    if (waitForTimerATK <= 0)
                    {
                        if (!returning)
                        {
                            tongueAnim.Play("Sapo_Lengua");
                            float newScale = Mathf.SmoothDamp(tongue.localScale.x, ATKdistance, ref ATKVelocity, ATKspeed * Time.deltaTime);
                            tongue.localScale = new Vector3(newScale, 0.3f, 1);
                            tongueTimer -= Time.deltaTime;
                            if (tongueTimer <= 0)
                            {
                                returning = true;
                                tongueTimer = 1;

                            }

                        }
                        else 
                        {
                            tongueAnim.Play("Sapo_Lengua2");
                            float newScale = Mathf.SmoothDamp(tongue.localScale.x, 0, ref ATKVelocity, ATKspeed * Time.deltaTime);
                            tongue.localScale = new Vector3(newScale, 0.3f, 1);
                            tongueTimer -= Time.deltaTime;
                            if (tongueTimer <= 0)
                            {
                                returning = false;
                                tongueTimer = 1;
                                _currentState = State.Walking;
                                waitForTimerATK = 3;
                            }
                        }
                        
                    }
                    else
                    {
                        waitForTimerATK -= Time.deltaTime;
                    }
                    _currentHorizontalSpeed = 0;
                    MoveCharacter();

                    break;

            }
        }

        #endregion

        #region Attack

        [SerializeField] protected Transform tongue;
        [SerializeField] protected float tongueTimer;
        [SerializeField] protected float waitForTimerATK = 3;
        [SerializeField] protected float ATKspeed;
        [SerializeField] protected float ATKVelocity = 0;
        [SerializeField] protected float ATKdistance;
        [SerializeField] protected Animator tongueAnim;
        protected bool returning = false;

        #endregion

        #region Jump

        protected override void CalculateJump(PlayerController target)
        {
            if (_colDown)
            {
                _currentVerticalSpeed = _jumpHeight;
            }
            if (_colUp)
            {
                if (_currentVerticalSpeed > 0) _currentVerticalSpeed = 0;
            }
        }

        #endregion

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            AudioManager.instance.PlayAudio(5);
        }
    }
}
