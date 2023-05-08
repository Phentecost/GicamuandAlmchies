using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code_VFX
{
    public abstract class VFXController : MonoBehaviour
    {
        public abstract void StartVFX();
        public abstract void EndVFX();
    }
}
