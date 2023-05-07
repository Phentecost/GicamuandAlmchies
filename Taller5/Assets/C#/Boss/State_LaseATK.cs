using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code_Boses
{
    [CreateAssetMenu(menuName = "State_LaserAtk")]
    public class State_LaseATK : BaseState
    {
        [SerializeField] float castingTimer;
        private float _timer;
        [SerializeField] GameObject laserPrefab;
        private GameObject G;
        [SerializeField] float laserLength;
        [SerializeField] float laserSpeed;
        private float _velocity;
        [SerializeField] float laserDuration;
        float _laserDuration;

        private enum state { Chargind, Fire, Realese }

        private state _state;

        public override void EnterState(BossStateManager boss)
        {
            _timer = castingTimer;
            _laserDuration = laserDuration;
            _velocity = 0;
            G = Instantiate(laserPrefab,boss.transform.position,Quaternion.identity);
            _state = state.Chargind;
        }

        public override void UpdateState(BossStateManager boss)
        {
            switch (_state) 
            {
                case state.Chargind:

                    if (_timer <= 0)
                    {
                        _state = state.Fire;
                    }
                    else 
                    {
                        _timer -= Time.deltaTime;
                    }

                    break;

                case state.Fire:

                    float newScale = Mathf.SmoothDamp(G.transform.localScale.x, laserLength, ref _velocity, laserSpeed * Time.deltaTime);
                    G.transform.localScale = new Vector3(newScale, 0.3f, 1);
                    _laserDuration -= Time.deltaTime;
                    if (_laserDuration <= 0)
                    {
                        _state= state.Realese;
                    }

                    break;

                case state.Realese:
                    float newScale2 = Mathf.SmoothDamp(G.transform.localScale.x, 0, ref _velocity, laserSpeed * Time.deltaTime);
                    G.transform.localScale = new Vector3(newScale2, 0.3f, 1);

                    if (G.transform.localScale.x <= 0.5)
                    {
                        Destroy(G);
                        boss.SwichState(boss.idle);
                    }
                    break;

            }
        }
    }
}
