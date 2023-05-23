using Code_EnemiesAndAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;

public class BoxBarrier : MonoBehaviour
{
    [SerializeField] public float projectileSpeed;
    [SerializeField] private float lifeTime;
    private Rigidbody2D rb;

    [SerializeField] Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        lifeTime = 3.5f;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("LifeTime", lifeTime);
    }
}
