using Code;
using EZCameraShake;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code_Boses
{
    [CreateAssetMenu(menuName = "State_Rush_ATK")]
    public class State_RushATK : BaseState
    {
        [SerializeField]float duration;
        private float elapsedTime;
        [SerializeField]float waitTimer;
        private float _timer;
        [SerializeField] AnimationCurve _Xcurve;
        [SerializeField] AnimationCurve _Ycurve;
        [SerializeField] float height;
        Vector3 pointA, pointB;
        private bool secondAtk;
        

        public override void EnterState(BossStateManager boss)
        {
            elapsedTime = 0;
            pointB = boss.currentRoom.pointB.transform.position;
            pointA = boss.currentRoom.pointA.transform.position;
            secondAtk = false;
            _timer = waitTimer;

            if (!boss.left)
            {
                var a = pointA;
                pointA = pointB;
                pointB = a;
            }

            boss.transform.position = pointA;
        }

        public override void UpdateState(BossStateManager boss)
        {
            if (!secondAtk)
            {
                elapsedTime += Time.deltaTime;
                float percentageComplete = elapsedTime / duration;
                boss.transform.position = Vector3.Lerp(pointA, pointB, _Xcurve.Evaluate(percentageComplete));
                if (boss.transform.position == pointB)
                {
                    boss.Flip();

                    if (Random.Range(0, 100) <= 50)
                    {
                        secondAtk = true;
                        elapsedTime = 0;
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
                    elapsedTime += Time.deltaTime;
                    float percentageCompletey = elapsedTime / duration;
                    float h1 = _Ycurve.Evaluate(percentageCompletey);
                    float h2 = Mathf.Lerp(0f,height,h1);

                    boss.transform.position = Vector3.Lerp(pointB,pointA, percentageCompletey) + new Vector3(0f,h2);

                    if (boss.transform.position == pointA)
                    {
                        CameraShaker.Instance.ShakeOnce(4f,4f,.1f,1f);
                        boss.Flip();
                        boss.SwichState(boss.idle);
                    }
                }
                else
                {
                    _timer -= Time.deltaTime;
                }
            } 
        }
    }
}
