using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code_Boses
{
    [Serializable]
    public abstract class BaseState : ScriptableObject
    {
        public abstract void EnterState(BossStateManager boss);
        public abstract void UpdateState(BossStateManager boss);
    }
}
