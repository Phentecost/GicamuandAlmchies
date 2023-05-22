using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthStealSpellIcon : MonoBehaviour
{
    [SerializeField] public float projectileSpeed;
    [SerializeField] private float lifeTime;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        lifeTime = 2.5f;
        Destroy(gameObject, lifeTime);
    }
}
