using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TarodevController;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Code;
using Code_DungeonSystem;
using Code_Core;
using UnityEngine.Rendering;

namespace Code_EnemiesAndAI
{
    public class Enemy : MonoBehaviour
    {

        #region CORE
        protected enum State
        {
            Idle, Walking, Shooting
        }

        [Header("CORE")]
        [SerializeField] protected PlayerController _gicamu, _alchies;
        [SerializeField] protected Room _currentRoom;
        protected State _currentState = State.Idle;
        protected PlayerController _target;
        protected float distance;
        protected float _waitForTimer = 1.0f;
        

        private void Start()
        {
            _currentRoom.addRegister(this);
            gameObject.SetActive(false);
        }

        void Update()
        {
            Behaviour();
            HealthSystem();
        }

        public void SetUp(PlayerController _gicamu, PlayerController _alchies) 
        {
            this._gicamu= _gicamu;
            this._alchies= _alchies;
            _currentHealth = StartHealth;
            gameObject.SetActive(true);
        }

        #endregion

        public void dead() 
        {
            _currentRoom.removeRegister(this);
            Destroy(this.gameObject);
        }

        #region Behaviour

        protected virtual void Behaviour(){}

        protected PlayerController GetClosestPlayer()
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

        #endregion

        #region Walk

        [Header("WALKING")][SerializeField] protected float _acceleration = 90f;
        [SerializeField] protected float _moveClamp = 13;
        [SerializeField] protected float _deAcceleration = 60f;
        protected float _currentHorizontalSpeed, _currentVerticalSpeed;

        protected virtual void CalculateWalk(PlayerController target)
        {
            Vector2 v = target.transform.position-transform.position;

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
        protected void CalculateGravity()
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
        [SerializeField] protected float fallDownTreshold;

        protected RayRange _raysUp, _raysRight, _raysDown, _raysLeft;
        protected bool _colUp, _colRight, _colDown, _colLeft;
        protected RaycastHit2D _hitUp, _hitDown, _hitRight, _hitLeft;

        protected void CalculateCollisions() 
        {
            CalculateRaysRanges();

            (_colDown, _hitDown) = GetClosestPlayer().transform.position.y < transform.position.y && Vector2.Distance(GetClosestPlayer().transform.position, transform.position) < fallDownTreshold ?  Detection(_raysDown, _groundLayer) : Detection(_raysDown, _blockLayer);
            (_colUp, _hitUp) = Detection(_raysUp, _blockLayer);
            (_colLeft, _hitLeft) = Detection(_raysLeft, _blockLayer);
            (_colRight, _hitRight) = Detection(_raysRight, _blockLayer);

            (bool, RaycastHit2D) Detection(RayRange range, LayerMask layer)
            {
                bool Collition = false;
                RaycastHit2D ray = new RaycastHit2D();

                foreach (Vector2 point in CalculatePointsPositions(range))
                {
                    ray = Physics2D.Raycast(point, range.Dir, _detectionRayLength, layer);

                    if (ray.collider != null)
                    {
                        Collition = true;
                    }

                }
                return (Collition, ray);
                //return CalculatePointsPositions(range).Any((point) => Physics2D.Raycast(point, range.Dir,out hit,_detectionRayLength, layer));
            }

        }

        protected void CalculateCollitionBehaviour()
        {
            RaycastHit2D hit = ReturnHit();
            if (hit)
            {
                PlayerController pj = hit.collider.GetComponent<PlayerController>();
                if (pj != null)
                {
                    pj.TakeDamage(-1);
                }
            }
        }

        protected RaycastHit2D ReturnHit()
        {
            if (_hitRight) return _hitRight;
            if (_hitUp) return _hitUp;
            if (_hitDown) return _hitDown;
            return _hitLeft;
        }

        protected void CalculateRaysRanges() 
        {
            Bounds boxBound = new Bounds(transform.position,_characterBounds.size);
            _raysUp = new RayRange(boxBound.min.x + _rayBuffer,boxBound.max.y,boxBound.max.x - _rayBuffer,boxBound.max.y,Vector2.up);
            _raysRight = new RayRange(boxBound.max.x,boxBound.min.y + _rayBuffer,boxBound.max.x,boxBound.max.y-_rayBuffer,Vector2.right);
            _raysLeft = new RayRange(boxBound.min.x,boxBound.min.y+_rayBuffer,boxBound.min.x,boxBound.max.y- _rayBuffer,Vector2.left);
            _raysDown = new RayRange(boxBound.min.x + _rayBuffer,boxBound.min.y,boxBound.max.x-_rayBuffer,boxBound.min.y,Vector2.down);
        }

        protected IEnumerable<Vector2> CalculatePointsPositions(RayRange range) 
        {
            for (var i = 0; i < _detectorCount; i++) {
                var t = (float)i / (_detectorCount -1);
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
        protected virtual void CalculateJump(PlayerController target)
        {
            if (target.transform.position.y > transform.position.y +2 && _colDown)
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
        protected void MoveCharacter()
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

        #region Attack

        [Header("ATTACK")]
        [SerializeField] protected int damage;
        [SerializeField] protected float attackTheshold;

        protected virtual void Attack() 
        {
            
        }

        #endregion

        #region Flip

        protected bool isFacingRight = true;

        protected void Flip() 
        {
            if (isFacingRight && _currentHorizontalSpeed< 0f || !isFacingRight && _currentHorizontalSpeed > 0f)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
        }

        #endregion

        #region HealthSystem

        [Header("HealthSystem")]
        [SerializeField] protected int StartHealth;
        protected int _currentHealth;

        public virtual void TakeDamage(int damage)
        {
            _currentHealth += damage;
            Debug.Log(_currentHealth);
        }

        public void HealthSystem() 
        {
            if (_currentHealth <= 0)
            {
                dead();
            }
        }

        #endregion

        #region StunSystem

        [SerializeField] private float StunTimer;
        private float _stuntimer;
        private bool _stuned = false;

        public void Stun()
        {
            if (_stuned) return;
            _stuntimer = StunTimer;
            _stuned = true;
        }

        private void StunSystem()
        {
            Debug.Log(_stuned);
            if (_stuned)
            {
                _currentHorizontalSpeed = 0;
                _currentVerticalSpeed = 0;
                if (_stuntimer <= 0)
                {
                    _stuned = false;
                }
                else
                {
                    _stuntimer -= Time.deltaTime;
                }
            }
        }

        #endregion

    }


}
