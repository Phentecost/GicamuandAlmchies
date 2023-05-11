using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alchemist : PlayerController
{
    #region Alchemist components configuration

    [SerializeField] private BoxBarrier abilityQ;
    [SerializeField] private MagicPellets abilityE;

    [Header("Alchemist's Abilities")]
    [Header("Barrier")]
    [SerializeField] private float barrierCoolDown = 12f;
    [SerializeField] private bool barrierActivated = false;

    [Header("Magic pellets")]
    [SerializeField] private int pelletsAmmo = 3;
    [SerializeField] private float pelletsTime = 1.5f;
    [SerializeField] private float pelletsCounter;
    [SerializeField] private float pelletsCoolDown = 0.5f;
    [SerializeField] private bool pelletsActivated = false;

    [Header("Heal power")]
    [SerializeField] private Transform healRadius;
    [SerializeField] private LayerMask layerOtherPlayer;
    [SerializeField] private float healTime = 1.5f; //testear
    [SerializeField] private float healCounter;
    [SerializeField] private int healAmmo = 3;
    private int healthPowerRestored = 3;
    private bool inside;
    [SerializeField] private bool healActivated = false;

    private Wizard wizard;

    #endregion

    protected override void Update()
    {
        wizard = GameObject.FindObjectOfType<Wizard>();
        inside = Physics2D.OverlapCircle(healRadius.position, 50f, layerOtherPlayer);

        MovementSystem();
        HealthSystem(0, false);
        AbilitiesSystem();

        Barrier();
        MagicPellets();
        HealPower();
    }

    private void MovementSystem()
    {
        horizontal = Input.GetAxisRaw("P1_Horizontal");

        if (IsGrounded())
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.W))
            jumpBufferCounter = jumpBufferTime; //Guarda que se preciono la tecla antes de que toque el suelo
        else
            jumpBufferCounter -= Time.deltaTime;

        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
        {
            rb.velocity = new Vector2(0, jumpForce);
            jumpBufferCounter = 0f;
        }

        if (Input.GetKeyUp(KeyCode.W) && rb.velocity.y > 0) //Salta x altura de acuerdo al tiempo que se presiono la tecla
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.65f);
            coyoteTimeCounter = 0f;
        }

        //Flip
        if (horizontal == 1) //mira a la derecha
            transform.eulerAngles = new Vector2(0, 0);
        if (horizontal == -1)//mira a la izquierdad
            transform.eulerAngles = new Vector2(0, 180);

        if (rb.velocity.y < 25)  //Caida automatica mas rapida
            rb.velocity -= gravity * fallFaster * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.S)) //Caida mas rapida
            rb.velocity = new Vector2(rb.velocity.x, -jumpForce * 0.5f);
    }

    public void HealthSystem(int amount, bool stunned)
    {
        health = Mathf.Clamp(health + amount, 0, maxHealth);
        if (amount != 0)
        {
            //Debug.Log("Alquimist's health: " + health);
            attacked = true;
            //Debug.Log("atacado");
            if (attacked)
                attackedCounter = attackedTime;
        }

        attackedCounter -= Time.deltaTime;

        if (attackedCounter <= 0f)
        {
            attacked = false;
            //Debug.Log("No atacado");
        }
        /*
        if (stunned)
            stunnedCounter = stunnedTime;

        if (stunnedCounter > 0f)
        {
            stunnedCounter -= Time.deltaTime;
            speedMovement = 0;
        }
        else
        {
            speedMovement = minSpeed;
            stunned = false;
        }
         */

        if (stunned)
            stunnedCounter = stunnedTime;

        if (stunnedCounter > 0f)
        {
            stunnedCounter -= Time.deltaTime;
            speedMovement = 0;
        }
        else
        {
            speedMovement = minSpeed;
            stunned = false;
        }

        /*
        stunnedCounter -= Time.deltaTime;

        if (stunned)
        {
            stunnedCounter = stunnedTime;
            stunned = false;
        }
            

        if (stunnedCounter <= 0f)
        {
            stunned = false;
            speedMovement = 0;
        }
        else
        {
            speedMovement = minSpeed;
            stunned = false;
        }
         */



        if (health <= 0)
        {
            //Reiniciar nivel
            Debug.Log("Alquimista muerto");
        }
    }

    protected void AbilitiesSystem()
    {
        //Barrera
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!barrierActivated)
            {
                barrierCoolDown = 14.5f;
                barrierActivated = true;

                abilityQ.projectileSpeed = 0f;
                Instantiate(abilityQ, launchPosition.position, transform.rotation);
            }
        }

        //Perdigones magicos horizontales
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (pelletsAmmo > 0f)
            {
                if (!pelletsActivated)
                {
                    pelletsCounter = pelletsTime;
                    pelletsCoolDown = 0.5f;
                    pelletsActivated = true;

                    abilityE.projectileSpeed = 100f;
                    Instantiate(abilityE, launchPosition.position, transform.rotation);
                }
            }
            else
            {
                Debug.Log("No ammo");
                //sfx comando invalido
            }
        }

        //Heal cercano
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (healAmmo > 0)
            {
                if (inside)
                {
                    if (!healActivated)
                    {
                        healCounter = healTime;
                        healActivated = true;
                    }
                }
                else
                {
                    Debug.Log("Fuera de rango");
                    //sfx comando invalido
                }
            }
        }
    }

    private void Barrier()
    {
        barrierCoolDown -= Time.deltaTime;

        if (barrierActivated)
        {
            if (barrierCoolDown <= 0f)
            {
                barrierActivated = false;
                Debug.Log("CoolDown barrier terminado");
            }
            else
            {
                Debug.Log("barrera no cargada");
                //sfx comando invalido
            }
        }
    }

    private void MagicPellets()
    {
        pelletsCoolDown -= Time.deltaTime;
   
        if (pelletsActivated)
        {
            if (pelletsCoolDown <= 0f)
            {
                pelletsAmmo--;
                pelletsActivated = false;
            }
            else
            {
                //Debug.Log("Cooldown pellets");
                //sfx comando invalido
            }
        }

        if (pelletsAmmo <= 0f)
        {
            pelletsCounter -= Time.deltaTime;
            //Debug.Log("Recargando...");
            if (pelletsCounter <= 0f)
                pelletsAmmo = 3;
        }
    }

    private void HealPower()
    {
        healCounter -= Time.deltaTime;

        if (healActivated)
        {
            //Debug.Log("Curando...");
            if (healCounter <= 0)
            {
                if (!attacked)
                {
                    if (inside)
                    {
                        wizard.HealthSystem(healthPowerRestored, false);
                        healAmmo--;
                    }
                    else
                    {
                        //Debug.Log("Fuera de rango");
                        //sfx comando invalido
                    }
                }
                else
                {
                    //Debug.Log("Habilidad cancelada");
                    //sfx comando invalido
                }

                healActivated = false;
            }
        }
    }
}


