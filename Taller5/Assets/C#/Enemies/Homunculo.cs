using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Code;
using Unity.VisualScripting;
using Code_Proyectiles;

namespace Code_EnemiesAndAI
{
    public class Homunculo : Enemy
    {
        [SerializeField] protected float jumpThreshold = 5;

        #region Behaviour
        protected override void Behaviour()
        {
           
            _target = GetClosestPlayer();
            distance = Vector2.Distance(_target.transform.position, transform.position);
            CalculateCollisions();
            CalculateCollitionBehaviour();
            if (waitForTimerATK <= 0 && distance < attackTheshold)
            {
                Attack();

            }
            else
            {
                waitForTimerATK -= Time.deltaTime;
            }
        }

        #endregion

        #region Attack

        [SerializeField] protected GameObject bulletPrefab;
        [SerializeField] protected float waitForTimerATK = 2;
        [SerializeField] protected float ATKspeed;

        protected override void Attack()
        {
            Vector3 vec2tar = _target.transform.position - transform.position;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.transform.parent = null;
            bullet.GetComponent<Proyectile>().SetMovement(ATKspeed, vec2tar,false);
            waitForTimerATK = 2;

        }
        #endregion

    }
}
