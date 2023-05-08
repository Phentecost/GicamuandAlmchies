using TarodevController;
using Code;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Code_DungeonSystem;

namespace Code_Boses
{
    public class BossStateManager : MonoBehaviour
    {
        [SerializeField] BaseState _currentState;
        public BaseState idle;
        [SerializeField] List<BaseState> _states;
        [SerializeField] Player _gicamu, _alchies;
        public Room currentRoom;
        public bool left = true;


        void Start()
        {
            _currentState.EnterState(this);

        }

        void Update()
        {
             _currentState.UpdateState(this);
        }

        public void SwichState() 
        {
            //_currentState = _states.Find(x => x.GetType() == typeof(State_LaseATK));
            int i = UnityEngine.Random.Range(0, _states.Count);
            _currentState = _states[i];
            _currentState.EnterState(this);
        }

        public void SwichState(BaseState idle)
        {
            _currentState = idle;
            _currentState.EnterState(this);
        }

        public Player GetClosestPlayer()
        {
            {
                float EtoGicamu = Vector2.Distance(transform.position, _gicamu.transform.position);
                float EtoAlchies = Vector2.Distance(transform.position, _alchies.transform.position);

                if (EtoAlchies > EtoGicamu)
                {
                    return _gicamu;
                }
                else if (EtoAlchies < EtoGicamu)
                {
                    return _alchies;
                }

                return null;
            }
        }

        #region Flip

        //protected bool isFacingRight = true;

        public void Flip()
        {
           
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            left = !left;
            transform.localScale = localScale;
            
        }

        #endregion
    }
}
