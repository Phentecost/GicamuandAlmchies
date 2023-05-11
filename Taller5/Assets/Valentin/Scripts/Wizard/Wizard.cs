using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class Wizard : PlayerController
{
    #region Wizard components configuration

    [SerializeField] private ElementalBall abilityB;

    [Header("Wizard's Abilities")]
    [Header("Elemental ball")] //velocidad media
    [SerializeField] private float ballTime = 1.5f;
    [SerializeField] private float ballCounter;
    [SerializeField] private int ballAmmo = 5;
    [SerializeField] private float ballCoolDown = 4f;
    [SerializeField] private bool ballActivated = false;

    [Header("Stun spell")] //rango cercano
    [SerializeField] private Transform stunRadius;
    private GameObject[] enemiesInside;
    [SerializeField] private float stunTime = 2.5f;
    [SerializeField] private float stunCounter;
    [SerializeField] private float stunCoolDown = 6f; //enemigo -> 2.5 | jefe -> 1 
    [SerializeField] private bool stunActivated = false;

    [Header("Health steal spell")] //rango global - random
    [SerializeField] private int random;
    private EnemyPlaceHolder[] enemies;
    [SerializeField] private LayerMask layerEnemies;
    [SerializeField] private int healthStole = 4;
    private int healthSpellRestored = 2;
    [SerializeField] private int healthSpellAmmo = 2;
    [SerializeField] private float stealTime = 2.5f; 
    [SerializeField] private float stealCounter;
    [SerializeField] private bool healthStealActivated = false;
    
    private Alchemist alchemist;
    public bool inside;

    #endregion

    protected override void Update()
    {
        enemiesInside = GameObject.FindGameObjectsWithTag("EnemyInside");
        enemies = GameObject.FindObjectsOfType<EnemyPlaceHolder>();
        
        alchemist = GameObject.FindObjectOfType<Alchemist>();

        inside = Physics2D.OverlapCircle(stunRadius.position, 150f, layerEnemies);

        MovementSystem();
        HealthSystem(0, false);
        AbilitiesSystem();

        ElementalBall();
        StunSpell();
        HealthStealSpeel();
    }

    private void MovementSystem()
    {
        horizontal = Input.GetAxisRaw("P2_Horizontal");

        if (IsGrounded())
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.UpArrow))
            jumpBufferCounter = jumpBufferTime; //Guarda que se preciono la tecla antes de que toque el suelo
        else
            jumpBufferCounter -= Time.deltaTime;

        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
        {
            rb.velocity = new Vector2(0, jumpForce);
            jumpBufferCounter = 0f;
        }

        if (Input.GetKeyUp(KeyCode.UpArrow) && rb.velocity.y > 0f) //Salta x altura de acuerdo al tiempo que se presiono la tecla
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.65f);
            coyoteTimeCounter = 0f;
        }

        //Flip
        if (horizontal == 1) //mira a la derecha
        {
            transform.eulerAngles = new Vector2(0, 0);
            launchPosition.transform.rotation = Quaternion.Euler(0, 0, 75f);
        } 
           
        if (horizontal == -1) //mira a la izquierda
        {
            transform.eulerAngles = new Vector2(0, 180);
            launchPosition.transform.rotation = Quaternion.Euler(0, 0, 75f);
        } 
            
        if (rb.velocity.y < 25)  //Caida automatica mas rapida
            rb.velocity -= gravity * fallFaster * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.DownArrow)) //Caida mas rapida
            rb.velocity = new Vector2(rb.velocity.x, -jumpForce * 0.5f);
    }

    public void HealthSystem(int amount, bool stunned)
    {
        health = Mathf.Clamp(health + amount, 0, maxHealth);
        if (amount != 0)
        {
            //Debug.Log("Wizard's health: " + health);
            attacked = true;
            if (attacked)
                attackedCounter = attackedTime;
        }

        attackedCounter -= Time.deltaTime;

        if (attackedCounter <= 0f)
        {
            attacked = false;
            //Debug.Log("No atacado");
        }

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

        if (health <= 0)
        {
            //Reiniciar nivel
            //(Debug.Log("Mago muerto");
        }
    }

    private void AbilitiesSystem()
    {
        //Bola elemental diagonal
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (ballAmmo > 0f)
            {
                if (!ballActivated)
                {
                    ballCounter = ballTime;
                    ballCoolDown = 4f;
                    ballActivated = true;

                    abilityB.projectileXSpeed = 90f;
                    abilityB.projectileYSpeed = 15f;
                    Instantiate(abilityB, launchPosition.position, transform.rotation);
                }
            }
        }

        //Conjuro stunea enemigos
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (!stunActivated)
            {
                if (stunCoolDown <= 0)
                {
                    if (inside)
                    {
                        stunCoolDown = 6f;
                        stunCounter = stunTime;

                        stunActivated = true;

                    }

                    if (stunCounter > 0f)
                    {

                        //Debug.Log("Enemigos stuneados");
                        //speedMovement = 0;
                    }
                    else
                    {
                        //Debug.Log("Enemigos velocidad restaurada");
                        //speedMovement = minSpeed;
                        inside = false;
                        
                    }
                }
            }

            
        }

        //Hechizo roba vida
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (healthSpellAmmo > 0)
            {
                if (!healthStealActivated)
                {
                    stealCounter = stealTime;
                    healthStealActivated = true;

                    random = Random.Range(0, enemies.Length);
                    if (enemies[random] != null)
                    {
                        enemies[random].HealthSystem(-healthStole, false);
                        alchemist.HealthSystem(healthSpellRestored, false);
                    }
                    
                    //else
                    //    healthStealActivated = false;
                }
            }
        }
    }

    private void ElementalBall()
    {
        ballCounter -= Time.deltaTime;
        
        if (ballActivated)
        {
            if (ballCounter <= 0f)
            {
                ballAmmo--;
                ballActivated = false;
            }
            else
            {
                //Debug.Log("Cooldown ball");
                //sfx comando invalido
            }
        }

        if (ballAmmo <= 0)
        {
            ballCoolDown -= Time.deltaTime;
            //Debug.Log("Recargando elemental ball");
            if (ballCoolDown <= 0f)
                ballAmmo = 5;
        }
    }

    private void StunSpell()
    {
        stunCoolDown -= Time.deltaTime;

        stunCounter -= Time.deltaTime;
        if (stunActivated)
        {
            //Debug.Log("Stuneando...");
            if (stunCounter <= 0f)
            {
                if (inside)
                {
                    foreach(GameObject e in enemiesInside)
                    {
                        e.GetComponent<EnemyPlaceHolder>().stunnedCounter = 2.5f;
                        e.GetComponent<EnemyPlaceHolder>().HealthSystem(0, true);
                    }

                    //for (int i = 0; i < enemiesInside.Length; i++)
                    //{
                    //    if (gameObject.tag == "EnemyInside")
                    //    {

                    //        enemiesInside[i].CompareTag("EnemyInside");
                    //    }
                        
                    //}
                }
            }
        }
    }

    private void HealthStealSpeel()
    {
        stealCounter -= Time.deltaTime;

        if (healthStealActivated)
        {
            if (stealCounter <= 0)
            {
                if (!attacked)
                {
                    if(enemies.Length > 0)
                        healthSpellAmmo--;

                    if (enemies[random] == null)
                    {
                        foreach (EnemyPlaceHolder e in enemies)
                        {
                            int i = 0;
                            if (enemies[i] == null)
                                i++;
                            else
                            {
                                enemies[i].HealthSystem(-healthStole, false);
                                alchemist.HealthSystem(healthSpellRestored, false);
                                Debug.Log("Encontro enemigo");
                                break;
                            }
                        }
                        //Debug.Log("Foreach rompido/terminado");
                    }
                }
                else
                {
                    //Debug.Log("Habilidad cancelada");
                    //sfx comando invalido
                }

                healthStealActivated = false;
            }
            else
            {
                //Debug.Log("coolddown hechizo curacion");
                //sfx comando invalido
            }
        }
    }

}
