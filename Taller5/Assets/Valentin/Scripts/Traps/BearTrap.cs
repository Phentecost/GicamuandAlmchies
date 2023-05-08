using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrap : MonoBehaviour
{
    private Alchemist alchemist;
    private Wizard wizard;

    private void Start()
    {
        alchemist = GameObject.FindObjectOfType<Alchemist>();
        wizard = GameObject.FindObjectOfType<Wizard>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController controller = collision.GetComponent<PlayerController>();

        if (collision.gameObject.tag == "Alchemist")
            alchemist.HealthSystem(-1, true);

        if (collision.gameObject.tag == "Wizard")
            wizard.HealthSystem(-1, true);
    }
}
