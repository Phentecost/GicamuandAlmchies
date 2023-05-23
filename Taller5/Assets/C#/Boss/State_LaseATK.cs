using Code_VFX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using TarodevController;
using Code;

namespace Code_Boses
{
    [CreateAssetMenu(menuName = "State_LaserAtk")]
    public class State_LaseATK : BaseState
    {
        [SerializeField] float castingTimer;
        private float _timer;
        [SerializeField] GameObject laserPrefab;
        private VFXLaserController G;
        private LineRenderer laser;
        private Laser L;
        [SerializeField] float laserLength;
        private float _direction;
        private float D;
        [SerializeField] float laserSpeed;
        private float _velocity;
        [SerializeField] float laserDuration;
        float _laserDuration;
        bool fireParticles;
        bool endParticles;
        Vector3 laserScale;

        private enum state { Chargind, Fire, Realese }

        private state _state;

        public override void EnterState(BossStateManager boss)
        {
            _timer = castingTimer;
            _laserDuration = laserDuration;
            _velocity = 0;
            fireParticles = false;
            endParticles= false;
            G = Instantiate(laserPrefab,boss.transform.position,Quaternion.identity).GetComponent<VFXLaserController>();
            laser = G.laser;
            L = G.L;
            G.SetUP(boss.transform.position);
            _state = state.Chargind;
            _direction = boss.transform.position.x + laserLength;
            D = laserLength;
            if (!boss.left){ _direction *= -1; D*= -1; }
            laserScale = boss.transform.position;
            laser.transform.position = new Vector3(-100,-100);
            G.StartFirstHalfVFX();
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

                    float newScale = Mathf.SmoothDamp(laserScale.x, _direction, ref _velocity, laserSpeed * Time.deltaTime);
                    laserScale = new Vector3(newScale,boss.transform.position.y, 1);
                    laser.SetPosition(1, laserScale);
                    laser.transform.position = new Vector3(boss.transform.position.x + (D / 2) , boss.transform.position.y);
                    _laserDuration -= Time.deltaTime;
                    if (_laserDuration <= 0)
                    {
                        _state= state.Realese;
                    }

                    if (!fireParticles)
                    {
                        G.StartSecondHalfVFX();
                        fireParticles= true;
                    }

                    if (!endParticles)
                    {
                        if (boss.left)
                        {
                            if (newScale <= _direction - 0.02f)
                            {
                                G.StartRestVFX(laserScale);
                                endParticles = true;
                            }
                        }
                        else if(newScale >= _direction - 0.02f)
                        {
                            G.StartRestVFX(laserScale);
                            endParticles = true;
                        }
                        
                    }

                    break;

                case state.Realese:
                    float newScale2 = Mathf.SmoothDamp(laserScale.x, boss.transform.position.x, ref _velocity, laserSpeed * Time.deltaTime);
                    laserScale = new Vector3(newScale2, boss.transform.position.y, 1);
                    laser.SetPosition(1, laserScale);
                    laser.transform.position = new Vector3(-100, -100);
                    if (endParticles)
                    {
                        G.EndFirstHalfVFX();
                        endParticles = false;
                    }

                    if (laserScale.x <= boss.transform.position.x + 0.2)
                    {
                        G.EndAllVFX();
                        Destroy(G.gameObject);
                        boss.SwichState(boss.idle);

                    }
                    break;

            }
        }

       
    }
}
