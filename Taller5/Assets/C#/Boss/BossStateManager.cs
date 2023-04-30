using TarodevController;
using Code;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Code_DungeonSystem;

namespace Code_Boses
{
    public class BossStateManager : MonoBehaviour
    {
        [SerializeField] BaseState _currentState;
        public BaseState idle;
        [SerializeField] List<BaseState> _states;
        [SerializeField] Player _gicamu, _alchies;
        public Room currentRoom;
        public bool left = true;


        void Start()
        {
            _currentState = _states.Find(x => x.GetType() == typeof(State_Idle));
            _currentState.EnterState(this);

        }

        // Update is called once per frame
        void Update()
        {
            _currentState.UpdateState(this);
        }

        public void SwichState() 
        {
            _currentState = _states.Find(x => x.GetType() == typeof(State_JumpATK));
            _currentState.EnterState(this);
        }

        public void SwichState(BaseState idle)
        {
            _currentState = idle;
            _currentState.EnterState(this);
        }

        public Player GetClosestPlayer()
        {
            {
                float EtoGicamu = Vector2.Distance(transform.position, _gicamu.transform.position);
                float EtoAlchies = Vector2.Distance(transform.position, _alchies.transform.position);

                if (EtoAlchies > EtoGicamu)
                {
                    return _gicamu;
                }
                else if (EtoAlchies < EtoGicamu)
                {
                    return _alchies;
                }

                return null;
            }
        }

        #region Walk

        [Header("WALKING")][SerializeField] protected float _acceleration = 90f;
        [SerializeField] protected float _moveClamp = 13;
        [SerializeField] protected float _deAcceleration = 60f;
        protected float _currentHorizontalSpeed, _currentVerticalSpeed;

        public virtual void CalculateWalk(Player target)
        {
            Vector2 v = target.transform.position - transform.position;

            if (v.x > 1 || v.x < -1)
            {
                v.Normalize();
                _currentHorizontalSpeed += v.x * _acceleration * Time.deltaTime;
                _currentHorizontalSpeed = Mathf.Clamp(_currentHorizontalSpeed, -_moveClamp, _moveClamp);
            }
            else
            {
                _currentHorizontalSpeed = Mathf.MoveTowards(_currentHorizontalSpeed, 0, _deAcceleration * Time.deltaTime);
            }
            if (_currentHorizontalSpeed > 0 && _colRight || _currentHorizontalSpeed < 0 && _colLeft)
            {
                _currentHorizontalSpeed = 0;
            }
        }

        #endregion

        #region Gravity

        [Header("GRAVITY")][SerializeField] protected float _fallClamp = -40f;
        [SerializeField] protected float _maxFallSpeed = 120f;
        public void CalculateGravity()
        {
            if (_colDown)
            {
                if (_currentVerticalSpeed < 0) _currentVerticalSpeed = 0;
            }
            else
            {
                _currentVerticalSpeed -= _maxFallSpeed * Time.deltaTime;
                if (_currentVerticalSpeed < _fallClamp) _currentVerticalSpeed = _fallClamp;
            }
        }

        #endregion

        #region Collisions

        [Header("Collisions")]
        [SerializeField] protected Bounds _characterBounds;
        [SerializeField] protected LayerMask _blockLayer;
        [SerializeField] protected LayerMask _groundLayer;
        [SerializeField] protected int _detectorCount = 3;
        [SerializeField] protected float _detectionRayLength = 0.1f;
        [SerializeField][Range(0.1f, 0.3f)] protected float _rayBuffer = 0.1f;

        protected RayRange _raysUp, _raysRight, _raysDown, _raysLeft;
        protected bool _colUp, _colRight, _colDown, _colLeft;

        public void CalculateCollisions()
        {
            CalculateRaysRanges();

            _colDown = Detection(_raysDown, _groundLayer);
            _colUp = Detection(_raysUp, _blockLayer);
            _colLeft = Detection(_raysLeft, _blockLayer);
            _colRight = Detection(_raysRight, _blockLayer);

            bool Detection(RayRange range, LayerMask layer)
            {
                return CalculatePointsPositions(range).Any(point => Physics2D.Raycast(point, range.Dir, _detectionRayLength, layer));
            }

        }

        protected void CalculateRaysRanges()
        {
            Bounds boxBound = new Bounds(transform.position, _characterBounds.size);
            _raysUp = new RayRange(boxBound.min.x + _rayBuffer, boxBound.max.y, boxBound.max.x - _rayBuffer, boxBound.max.y, Vector2.up);
            _raysRight = new RayRange(boxBound.max.x, boxBound.min.y + _rayBuffer, boxBound.max.x, boxBound.max.y - _rayBuffer, Vector2.right);
            _raysLeft = new RayRange(boxBound.min.x, boxBound.min.y + _rayBuffer, boxBound.min.x, boxBound.max.y - _rayBuffer, Vector2.left);
            _raysDown = new RayRange(boxBound.min.x + _rayBuffer, boxBound.min.y, boxBound.max.x - _rayBuffer, boxBound.min.y, Vector2.down);
        }

        protected IEnumerable<Vector2> CalculatePointsPositions(RayRange range)
        {
            for (var i = 0; i < _detectorCount; i++)
            {
                var t = (float)i / (_detectorCount - 1);
                yield return Vector2.Lerp(range.Start, range.End, t);
            }
        }

        private void OnDrawGizmos()
        {
            // Bounds
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position + _characterBounds.center, _characterBounds.size);

            // Rays
            if (!Application.isPlaying)
            {
                CalculateRaysRanges();
                Gizmos.color = Color.blue;
                foreach (var range in new List<RayRange> { _raysUp, _raysRight, _raysDown, _raysLeft })
                {
                    foreach (var point in CalculatePointsPositions(range))
                    {
                        Gizmos.DrawRay(point, range.Dir * _detectionRayLength);
                    }
                }
            }
        }

        #endregion

        #region Jump
        [Header("JUMP")]
        [SerializeField] protected float _jumpHeight = 30;
        public virtual void CalculateJump(Player target)
        {
            if (target.transform.position.y > transform.position.y + 2 && _colDown)
            {
                _currentVerticalSpeed = _jumpHeight;
            }
            if (_colUp)
            {
                if (_currentVerticalSpeed > 0) _currentVerticalSpeed = 0;
            }
        }

        #endregion

        #region Move

        [Header("MOVE")]
        [SerializeField, Tooltip("Raising this value increases collision accuracy at the cost of performance.")]
        protected int _freeColliderIterations = 10;
        protected Vector3 RawMovement { get; private set; }
        public void MoveCharacter()
        {
            Flip();
            var pos = transform.position;
            RawMovement = new Vector3(_currentHorizontalSpeed, _currentVerticalSpeed);
            var move = RawMovement * Time.deltaTime;
            var furthestPoint = pos + move;

            var hit = Physics2D.OverlapBox(furthestPoint, _characterBounds.size, 0, _blockLayer);
            if (!hit)
            {
                transform.position += move;
                return;
            }

            var positionToMoveTo = transform.position;
            for (int i = 1; i < _freeColliderIterations; i++)
            {
                var t = (float)i / _freeColliderIterations;
                var posToTry = Vector2.Lerp(pos, furthestPoint, t);

                if (Physics2D.OverlapBox(posToTry, _characterBounds.size, 0, _blockLayer))
                {
                    transform.position = positionToMoveTo;
                    if (i == 1)
                    {
                        if (_currentVerticalSpeed < 0) _currentVerticalSpeed = 0;
                        var dir = transform.position - hit.transform.position;
                        transform.position += dir.normalized * move.magnitude;
                    }

                    return;
                }

                positionToMoveTo = posToTry;
            }
        }

        #endregion

        #region Flip

        //protected bool isFacingRight = true;

        public void Flip()
        {
            
            
                //isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            
        }

        #endregion
    }
}
