using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBarrier : MonoBehaviour
{
    [SerializeField] public float projectileSpeed;
    [SerializeField] private float lifeTime;
    private Rigidbody2D rb;

    void Start()
    {
        Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Barrier").GetComponent<Collider2D>(), GameObject.FindGameObjectWithTag("Wizard").GetComponent<Collider2D>(), true);
        Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Barrier").GetComponent<Collider2D>(), GameObject.FindGameObjectWithTag("Alchemist").GetComponent<Collider2D>(), true);
        rb = GetComponent<Rigidbody2D>();
        lifeTime = 2.5f;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        
    }
}
