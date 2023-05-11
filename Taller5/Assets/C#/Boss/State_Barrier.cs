using Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code_Boses
{
    [CreateAssetMenu(menuName = "State_Barrier")]
    public class State_Barrier : BaseState
    {
        [SerializeField] float castingTime;
        private float _timer;
        [SerializeField] float barrierLife;
        [SerializeField] float barrierHight;
        [SerializeField] GameObject barrierPrefab;
        


        public override void EnterState(BossStateManager boss)
        {
            _timer = castingTime;
        }

        public override void UpdateState(BossStateManager boss)
        {
            if (_timer <= 0)
            {
                Vector3 pos = boss.currentRoom.pointB.transform.position + boss.currentRoom.pointA.transform.position; 
                pos = new Vector3(pos.x,pos.y + barrierHight, pos.z);
                pos -= boss.transform.position/2;
                GameObject G = Instantiate(barrierPrefab,pos,Quaternion.identity);
                G.transform.parent = null;
                G.GetComponent<Barrier>().SetUp(barrierLife);
                boss.SwichState(boss.idle);
            }
            else
            {
                _timer -= Time.deltaTime;
            }
        }
    }
}
