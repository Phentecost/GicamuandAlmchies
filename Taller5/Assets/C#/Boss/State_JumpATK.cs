using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace Code_Boses
{

    [CreateAssetMenu(menuName = "State_JumpATK")]
    public class State_JumpATK : BaseState
    {
        private enum JumpingState { CalculateJumping, Jumping, Waiting, CalculateLanding, Landing, WaitingLanding}
        private Vector2 pointA , pointB ;
        [SerializeField] float timeBetweenJumps;
        private float _timer;
        [SerializeField] int numbersOfJumps;
        private int _count;
        private JumpingState _currentState;
        [SerializeField] float clampingCeiling;
        [SerializeField] float clampingFloor;
        private Vector3 _movingTo;
        private float _elapsedTime;
        [SerializeField] float movingDuration;
        [SerializeField] AnimationCurve movingBehaviour;
        private Vector3 _currentBossPosition;
        [SerializeField] float timerBetweenLandings;
        private float _landingTimer;
        private bool _finish;
        private Vector3 currentPos;
        public override void EnterState(BossStateManager boss)
        {
            _timer = timeBetweenJumps;
            _count = 0;
            _currentState = JumpingState.CalculateJumping;
            _elapsedTime = 0;
            _landingTimer = timerBetweenLandings;
            _finish = false;
            currentPos = boss.transform.position;
        }

        public override void UpdateState(BossStateManager boss)
        {
            
            switch (_currentState) 
            {
                case JumpingState.CalculateJumping:

                    Vector2 moveUp = boss.transform.position;
                    float y = Mathf.Clamp(moveUp.y + 10, clampingFloor, clampingCeiling);
                    _movingTo = new Vector3(moveUp.x, y);
                    _count++;
                    _currentBossPosition = boss.transform.position;
                    _elapsedTime = 0;
                    _currentState = JumpingState.Jumping;

                    break;

                case JumpingState.Jumping:

                    _elapsedTime += Time.deltaTime;
                    float percentageComplete = _elapsedTime / movingDuration;
                    boss.transform.position = Vector3.Lerp(_currentBossPosition, _movingTo, movingBehaviour.Evaluate(percentageComplete));

                    if (boss.transform.position == _movingTo)
                    {
                        
                        _currentState = JumpingState.Waiting;
                    }

                    break;

                    case JumpingState.Waiting:

                    if (_timer <= 0)
                    {
                        _timer = timeBetweenJumps;
                        _currentState = JumpingState.CalculateLanding;
                    }
                    else
                    {
                        _timer -= Time.deltaTime;
                    }

                    break;

                case JumpingState.CalculateLanding:

                    if (!_finish) 
                    {
                        Vector3 playerPos = boss.GetClosestPlayer().transform.position;
                        float playerY = Mathf.Clamp(playerPos.y + 10, clampingFloor, clampingCeiling);
                        Vector3 newPosition = new Vector3(playerPos.x, playerY);
                        boss.transform.position = newPosition;
                        float newY = Mathf.Clamp(boss.transform.position.y - 10, clampingFloor, clampingCeiling);
                        _movingTo = new Vector3(boss.transform.position.x, newY);
                        _currentBossPosition = boss.transform.position;
                        _elapsedTime = 0;
                        _currentState = JumpingState.Landing;
                    }
                    else
                    {
                        int i = Random.Range(0, 2);
                        Vector3 pointPosition;
                        if (i == 0)
                        {
                             pointPosition = boss.currentRoom.pointA;
                        }
                        else
                        {
                             pointPosition = boss.currentRoom.pointB;
                        }

                        if (Vector3.Distance(pointPosition,currentPos) > 1)
                        {
                            boss.Flip();
                        }

                        float pointY = Mathf.Clamp(pointPosition.y + 10, clampingFloor, clampingCeiling);
                        Vector3 newPosition = new Vector3(pointPosition.x, pointY);
                        boss.transform.position = newPosition;
                        _movingTo = pointPosition;
                        _currentBossPosition = boss.transform.position;
                        _elapsedTime = 0;
                        _currentState = JumpingState.Landing;
                    }
                    
                    

                    break;

                 case JumpingState.Landing:

                    _elapsedTime += Time.deltaTime;
                    float percentageCompleteLanding = _elapsedTime / movingDuration;
                    boss.transform.position = Vector3.Lerp(_currentBossPosition, _movingTo, movingBehaviour.Evaluate(percentageCompleteLanding));

                    if (boss.transform.position == _movingTo)
                    {
                        if (_finish)
                        {
                            boss.SwichState(boss.idle);
                        }
                        else
                        {
                            _currentState = JumpingState.WaitingLanding;
                        }
                        
                    }

                    break;

                case JumpingState.WaitingLanding:

                    if (_landingTimer <= 0)
                    {
                        _landingTimer = timerBetweenLandings;

                        if (_count == numbersOfJumps)
                        {
                            _finish = true;
                        }

                        _currentState = JumpingState.CalculateJumping;
                    }
                    else
                    {
                        _landingTimer -= Time.deltaTime;
                    }

                    break;
                        
            }
            
        }
    }
}
