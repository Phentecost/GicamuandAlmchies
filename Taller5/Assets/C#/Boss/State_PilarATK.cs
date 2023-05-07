using Code;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Code_Boses
{
    [CreateAssetMenu(menuName = "State_PilarAtk")]
    public class State_PilarATK : BaseState
    {
        [SerializeField] float waitForActivation;
        private float _activationTimer;
        [SerializeField] GameObject pilarPrefab;
        [SerializeField] GameObject sealPrefab;
        [SerializeField] float pilarVelocity;
        [SerializeField] float pilarHigh;
        [SerializeField] float pilarLifeTimerDuration;
        [SerializeField] float floor;
        private Vector2 pos;

        public override void EnterState(BossStateManager boss)
        {
            _activationTimer = waitForActivation;
            pos = boss.GetClosestPlayer().transform.position;
            pos = new Vector2 (pos.x,floor);
            GameObject G = Instantiate(sealPrefab,pos,Quaternion.identity);
            G.transform.parent = null;
            Destroy(G,waitForActivation);
        }

        public override void UpdateState(BossStateManager boss)
        {
            if (_activationTimer <= 0)
            {
                GameObject G = Instantiate(pilarPrefab,pos,Quaternion.identity);
                G.transform.parent = null;
                G.GetComponent<Pilar>().SetUp(pilarHigh, pilarVelocity, pilarLifeTimerDuration);
                boss.SwichState(boss.idle);
            }
            else
            {
                _activationTimer-= Time.deltaTime;
            }
        }
    }
}
