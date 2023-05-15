using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;

namespace Code_DungeonSystem
{
    public class Portals : MonoBehaviour
    {
        private int id = 0;
        bool triger = false;
        public void SetUp(int id) 
        {
            this.id = id;
        }

        private void Update()
        {
            CalculateCollisions();
            CalculateCollitionBehaviour();
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
                if (!triger)
                {
                    triger= true;
                    DungeonManager.instance.onChangingRoom(id);
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
