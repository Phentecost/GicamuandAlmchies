using Code_VFX;
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
        private VFXLaserController G;
        private LineRenderer laser;
        [SerializeField] float laserLength;
        private float _direction;
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
            G.SetUP(boss.transform.position);
            _state = state.Chargind;
            _direction = laserLength;
            if (!boss.left) _direction *= -1;
            laserScale = Vector3.zero;
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
                    laserScale = new Vector3(newScale, 0.3f, 1);
                    laser.SetPosition(1, laserScale);
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
                    laserScale = new Vector3(newScale2, 0.3f, 1);
                    laser.SetPosition(1, laserScale);

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
