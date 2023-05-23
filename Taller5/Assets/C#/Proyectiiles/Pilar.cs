using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;

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
            CalculateCollisions();
            CalculateCollitionBehaviour();
            switch (_states) 
            {
                case States.Up:

                    float newScale = Mathf.SmoothDamp(transform.localScale.y, _distance, ref _velocity, _speed * Time.deltaTime);
                    transform.localScale = new Vector3(2, newScale, 1);
                    _characterBounds.size.Set(2, newScale,1);

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
                    transform.localScale = new Vector3(2, newScale2 , 1);
                    _characterBounds.size.Set(2, newScale2, 1);

                    if (transform.localScale.y < 0+.1)
                    {
                        Destroy(gameObject);
                    }

                    break;
                
            }
        }

        #region Collisions

        [Header("Collisions")]
        [SerializeField] protected Bounds _characterBounds;
        [SerializeField] protected LayerMask _blockLayer;
        [SerializeField] protected int _detectorCount = 3;
        [SerializeField] protected float _detectionRayLength = 0.1f;
        [SerializeField][Range(0.1f, 0.3f)] protected float _rayBuffer = 0.1f;

        protected RayRange _raysUp, _raysRight, _raysDown, _raysLeft;
        protected bool _colUp, _colRight, _colDown, _colLeft;
        public RaycastHit2D _hitUp, _hitDown, _hitRight, _hitLeft;

        public void CalculateCollisions()
        {
            CalculateRaysRanges();

            (_colDown, _hitDown) = Detection(_raysDown, _blockLayer);
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
                PlayerController p = hit.collider.GetComponent<PlayerController>();

                if (p != null)
                {
                    p.TakeDamage(-1);
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
    }
}
