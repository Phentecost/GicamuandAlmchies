using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Code_Proyectiles;

namespace Code_Boses
{
    [CreateAssetMenu(menuName = "State_RocksFall")]
    public class State_RockFall : BaseState
    {
        [SerializeField] float timeBetweenRocks;
        private float _timer;
        [SerializeField] GameObject rockPrefab;
        [SerializeField] int numbersOfRocks;
        private int _nRocks;
        private float _minX,_maxX;
        [SerializeField]private float ceiling;
        [SerializeField] private float rockFallSpeed;

        public override void EnterState(BossStateManager boss)
        {
            _timer = timeBetweenRocks;
            _nRocks = 0;
            _minX = boss.currentRoom.pointA.x;
            _maxX = boss.currentRoom.pointB.x;
        }

        public override void UpdateState(BossStateManager boss)
        {
            if (_nRocks != numbersOfRocks)
            {
                if (_timer <= 0)
                {
                    float i = Random.Range(_minX, _maxX);
                    Vector3 pos = new Vector3(i, ceiling);
                    GameObject G = Instantiate(rockPrefab,pos,Quaternion.identity);
                    G.transform.parent = null;
                    G.GetComponent<Proyectile>().SetMovement(rockFallSpeed, Vector3.down, false);
                    _timer = timeBetweenRocks;
                    _nRocks++;
                }
                else
                {
                    _timer -= Time.deltaTime;
                }
            }
            else if (_nRocks == numbersOfRocks)
            {
                boss.SwichState(boss.idle);
            }
        }
    }
}
