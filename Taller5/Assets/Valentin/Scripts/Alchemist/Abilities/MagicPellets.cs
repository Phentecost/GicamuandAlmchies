using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MagicPellets : MonoBehaviour
{
    [SerializeField] public float projectileSpeed;
    [SerializeField] private float lifeTime;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        lifeTime = 10f;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += transform.right * Time.deltaTime * projectileSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyPlaceHolder enemy = collision.GetComponent<EnemyPlaceHolder>();

        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyInside"))
        {
            enemy.HealthSystem(-3, true);
            Destroy(gameObject);
        }
    }
}