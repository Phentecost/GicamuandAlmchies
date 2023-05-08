using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionZone : MonoBehaviour
{
    SpikesTrap spike;
    private int counter;

    private void Start()
    {
        spike = GameObject.FindObjectOfType<SpikesTrap>();
        counter = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (counter == 0)
        {
            if (collision.gameObject.CompareTag("Wizard"))
                spike.Activated(true);

            if (collision.gameObject.CompareTag("Alchemist"))
                spike.Activated(true);

            //counter++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wizard")) 
            spike.Disable(true);

        if (collision.gameObject.CompareTag("Alchemist")) 
            spike.Disable(true);
    }
}
