using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyPlaceHolder : MonoBehaviour
{
    [SerializeField] private int health, maxHealth;
    [SerializeField] public float stunnedCounter;
    [SerializeField] private float stunnedTime;
    private Alchemist alchemist;
    private Wizard wizard;

    void Start()
    {
        maxHealth = 6;
        health = maxHealth;
        stunnedTime = 1f;

        alchemist = GameObject.FindObjectOfType<Alchemist>();
        wizard = GameObject.FindObjectOfType<Wizard>();
    }

    void Update()
    {
        HealthSystem(0, false);

        if (wizard.inside)
        {
            this.gameObject.tag = "EnemyInside";
        }
        else
            this.gameObject.tag = "Enemy";
    }

    public void HealthSystem(int amount, bool stunned)
    {
        health = Mathf.Clamp(health + amount, 0, maxHealth);
        if (amount != 0)
            Debug.Log("Enemy's health: " + health);

        if (stunned)
            stunnedCounter = stunnedTime;

        if (stunnedCounter > 0f)
        {
            stunnedCounter -= Time.deltaTime;
        }
        else
        {
            stunned = false;
            stunnedTime = 1f;
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Alchemist")
        {
            alchemist.HealthSystem(-2, false);
        }

        if (collision.gameObject.tag == "Wizard")
        {
            wizard.HealthSystem(-2, false);
        }
    }
}

