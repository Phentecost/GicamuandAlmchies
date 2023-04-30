using Code_Proyectiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Code_Boses
{
    [CreateAssetMenu(menuName = "State_Shooting")]
    public class State_Shoting : BaseState
    {
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float timeBetweenBullets;
        [SerializeField] private int numberOfBullets;
        [SerializeField] private float bulletSpeed;
        [SerializeField] private int secondATKProbability;
        private float _timer;
        private float _nBullets;
        private bool _secondATK = false;

        public override void EnterState(BossStateManager boss)
        {
            _timer = 0;
            _nBullets = 0;
            _secondATK = false;
        }

        public override void UpdateState(BossStateManager boss)
        {
            if (!_secondATK)
            {
                if (_timer <= 0)
                {
                    Vector3 vec2tar = boss.GetClosestPlayer().transform.position - boss.transform.position;
                    GameObject bullet = Instantiate(bulletPrefab, boss.transform.position, Quaternion.identity);
                    bullet.transform.parent = null;
                    bullet.GetComponent<Proyectile>().SetMovement(bulletSpeed, vec2tar,false);
                    _timer = timeBetweenBullets;
                    _nBullets++;
                }
                else
                {
                    _timer -= Time.deltaTime;
                }

                if (_nBullets == numberOfBullets)
                {
                    if (Random.Range(0,101) <= secondATKProbability)
                    {
                        _secondATK = true;
                        _timer = 3;
                    }
                    else
                    {
                        boss.SwichState(boss.idle);
                    }
                    
                }
            }
            else
            {
                if (_timer <= 0)
                {
                    Vector3 vec2tar = boss.GetClosestPlayer().transform.position - boss.transform.position;
                    GameObject bullet = Instantiate(bulletPrefab, boss.transform.position, Quaternion.identity);
                    bullet.transform.parent = null;
                    bullet.GetComponent<Proyectile>().SetMovement(bulletSpeed/4, vec2tar,true);
                    boss.SwichState(boss.idle);
                }
                else
                {
                    _timer -= Time.deltaTime;
                }
            }
        }
    }
}
