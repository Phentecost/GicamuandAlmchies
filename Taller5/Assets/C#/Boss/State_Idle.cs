using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code_Boses
{
    [CreateAssetMenu(menuName = "State_Idle")]
    public class State_Idle : BaseState
    {
        [SerializeField] float _waitTimer;
        float _timer;
        

        public override void EnterState(BossStateManager boss)
        {
           
            _timer = _waitTimer;
        }

        public override void UpdateState(BossStateManager boss)
        {
            /*boss.CalculateCollisions();
            boss.CalculateGravity();
            boss.MoveCharacter();*/

            if (_timer < 0) 
            {
                boss.SwichState();
            }
            else
            {
                _timer -= Time.deltaTime;
            }
        }
    }
}
