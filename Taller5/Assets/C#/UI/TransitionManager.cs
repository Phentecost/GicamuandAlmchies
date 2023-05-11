using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Code_UI
{
    public class TransitionManager : MonoBehaviour
    {
        public static TransitionManager instance { get; private set; } = null;

        private void Awake()
        {
            if (instance != null) 
            {
                Destroy(this);
                return;
            }
            instance = this;

        }

        private Animator _animator;
        void Start () 
        {
            _animator= GetComponent<Animator>();
        }

        public void FadeOut() 
        {
            _animator.Play("CrossFade_End");
        }

        public void FadeIn() 
        {
            _animator.Play("CrossFade_Start");
        }
    }
}
