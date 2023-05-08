using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalBall : MonoBehaviour
{
    [SerializeField] public float projectileXSpeed;
    [SerializeField] public float projectileYSpeed;
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
        transform.position += transform.up * Time.deltaTime * projectileYSpeed;
        transform.position += transform.right * Time.deltaTime * projectileXSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyPlaceHolder enemy = collision.GetComponent<EnemyPlaceHolder>();

        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyInside"))
        {
            enemy.HealthSystem(-2, true);
            Destroy(gameObject);
        }
    }
}
