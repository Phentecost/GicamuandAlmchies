using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SpikesTrap : MonoBehaviour
{
    [SerializeField] private float activationTime, activationCounter, disableTime, disableCounter;
    [SerializeField] private float speed;
    private Alchemist alchemist;
    private Wizard wizard;

    private void Start()
    {
        activationTime = 0.8f;
        disableTime = 1.6f;
        speed = 0.015f;

        alchemist = GameObject.FindObjectOfType<Alchemist>();
        wizard = GameObject.FindObjectOfType<Wizard>();
    }

    private void Update()
    {
        Activated(false);
        Disable(false);
    }

    public void Activated(bool activated)
    {
        if (activated)
            activationCounter = activationTime;

        if (activationCounter >= 0f)
        {
            activationCounter -= Time.deltaTime;
            transform.Translate(0f, speed, 0f);
        }
        else
            transform.Translate(0f, 0, 0f);
    }

    public void Disable(bool disable)
    {
        if (disable)
            disableCounter = disableTime;

        if (disableCounter >= 0.0f && disableCounter <= 1.6f)
        {
            disableCounter -= Time.deltaTime;

            if (disableCounter >= 0.8f && disableCounter <= 1.6f)
                transform.Translate(0f, 0, 0f);
            else if (disableCounter >= 0f && disableCounter <= 0.8f)
                transform.Translate(0f, -speed, 0f);
            else
                transform.Translate(0f, 0, 0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController controller = collision.GetComponent<PlayerController>();

        if (collision.gameObject.tag == "Alchemist")
            alchemist.HealthSystem(-1, false);

        if (collision.gameObject.tag == "Wizard")
            wizard.HealthSystem(-1, false);
    }
}
