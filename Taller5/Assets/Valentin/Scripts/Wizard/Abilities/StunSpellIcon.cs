using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StunSpellIcon : MonoBehaviour
{
    [SerializeField] public float projectileSpeed;
    [SerializeField] private float DirectionChangeTime;
    [SerializeField] private float DirectionChangeCounter;
    [SerializeField] private Vector2 direction;

    [SerializeField] private float lifeTime;
    private Rigidbody2D rb;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        DirectionChangeTime = 0.5f;
        direction = NewDirection();

        lifeTime = 2.5f;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        DirectionChangeCounter += Time.deltaTime;

        if (DirectionChangeCounter >= DirectionChangeTime)
        {
            direction = NewDirection();
            DirectionChangeCounter = 0f;
        }

        transform.Translate(direction * projectileSpeed * Time.deltaTime);
    }

    private Vector2 NewDirection()
    {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);

        Vector2 vector2 = new Vector2(x, y).normalized;

        return vector2;
    }
}
