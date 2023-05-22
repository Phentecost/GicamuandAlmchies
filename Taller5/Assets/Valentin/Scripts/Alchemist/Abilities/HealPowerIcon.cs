using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code
{
    public class HealPowerIcon : MonoBehaviour
    {
        [SerializeField] public float projectileSpeed;
        [SerializeField] private float lifeTime;
        private Rigidbody2D rb;

        [SerializeField] Animator animator;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            lifeTime = 1.5f;
            Destroy(gameObject, lifeTime);
        }

        void Update()
        {
            animator = GetComponent<Animator>();
            animator.SetFloat("LifeTime", lifeTime);
        }
    }
}
