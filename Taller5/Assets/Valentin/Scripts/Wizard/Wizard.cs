using Code;
using Code_Boses;
using Code_DungeonSystem;
using Code_EnemiesAndAI;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TarodevController;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Wizard : PlayerController
{
    #region Wizard components configuration

    [SerializeField] private ElementalBall abilityB;
    [SerializeField] private StunSpellIcon abilityN;
    [SerializeField] private HealthStealSpellIcon abilityM;

    [Header("Wizard's Abilities")]
    [Header("Elemental ball")] //velocidad media
    [SerializeField] private float ballTime = 1.5f;
    [SerializeField] private float ballCounter;
    [SerializeField] private int ballAmmo = 5;
    [SerializeField] public float ballCoolDown = 2f;
    [SerializeField] public float ballCoolDownCounter;
    [SerializeField] private bool ballActivated = false;

    [Header("Stun spell")] //rango cercano
    [SerializeField] private Transform stunRadius;
    [SerializeField] private float stunTime = 2.5f;
    [SerializeField] private float stunCounter;
    [SerializeField] public float stunCoolDown = 6f; //enemigo -> 2.5 | jefe -> 1 
    [SerializeField] public float stunCoolDownCounter;
    [SerializeField] private bool stunActivated = false;
    [SerializeField] public Collider2D inside;

    [Header("Health steal spell")] //rango global - random
    [SerializeField] private int random;
    [SerializeField] private Enemy enemy;
    [SerializeField] private BossStateManager boss;
    [SerializeField] private LayerMask layerEnemies;
    [SerializeField] private int healthStole = 4;
    private int healthSpellRestored = 2;
    [SerializeField] private int healthSpellAmmo = 2;
    [SerializeField] private float stealTime = 2.5f; 
    [SerializeField] private float stealCounter;
    [SerializeField] private bool healthStealActivated = false;
    
    private Room room;
    private Alchemist alchemist;
    

    [Header("Animation")]
    [SerializeField] Animator animator;

    #endregion

    protected override void Update()
    {
        GatherInput();
        base.Update();
        AbilitiesSystem();

        inside = Physics2D.OverlapCircle(stunRadius.position, 1f, layerEnemies);

        room = DungeonManager.instance.dungeonRooms[currentRoom];

        alchemist = DungeonManager.instance.Alchies.GetComponent<Alchemist>();

        boss = GameObject.FindObjectOfType<BossStateManager>();

        animator = GetComponent<Animator>();
        
        ElementalBall();
        StunSpell();
        HealthStealSpell();
    }

    protected override void GatherInput()
    {
        Input = new FrameInput
        {
            JumpDown = UnityEngine.Input.GetKeyDown(KeyCode.UpArrow),
            JumpUp = UnityEngine.Input.GetKeyUp(KeyCode.UpArrow),
            FallDown = UnityEngine.Input.GetKey(KeyCode.DownArrow),
            X = UnityEngine.Input.GetAxisRaw("P2_Horizontal"),
            A1 = UnityEngine.Input.GetKeyDown(KeyCode.B),
            A2 = UnityEngine.Input.GetKeyDown(KeyCode.N),
            A3 = UnityEngine.Input.GetKeyDown(KeyCode.M)
        };

        animator.SetFloat("Horizontal", Mathf.Abs(Input.X));

        if (Input.JumpDown)
        {
            _lastJumpPressed = Time.time;

            animator.SetBool("Grounded", Input.JumpUp);
        }

        if (!Input.A1 && !Input.A2 && !Input.A3)
            animator.SetBool("Attack", false);
        else
            animator.SetBool("Attack", true);
    }

    private void AbilitiesSystem()
    {
        //Bola elemental diagonal
        if (Input.A1)
        {

            if (ballAmmo > 0f)
            {
                if (!ballActivated)
                {
                    AudioManager.instance.PlayAudio(6);
                    ballCounter = ballTime;
                    ballCoolDownCounter = ballCoolDown;
                    ballActivated = true;

                    abilityB.projectileXSpeed = 5f;
                    abilityB.projectileYSpeed = 5f;
                    Instantiate(abilityB, launchPosition.position, transform.rotation);
                    
                }
            }
        }

        //Conjuro stunea enemigos
        if (Input.A2)
        {
            if (!stunActivated)
            {
                if (inside)
                {
                    if(stunCoolDownCounter <= 0)
                    {
                        stunCounter = stunTime;
                        stunCoolDownCounter = stunCoolDown;
                        stunActivated = true;

                        abilityN.projectileSpeed = 5f;
                        Instantiate(abilityN, launchPosition.position, transform.rotation);
                    }
                   
                }
            }
        }

        //Hechizo roba vida
        if (Input.A3)
        {
            if (healthSpellAmmo > 0)
            {
                if (!healthStealActivated)
                {
                    if(currentRoom != 0)
                    {
                        stealCounter = stealTime;
                        healthStealActivated = true;

                        //random = Random.Range(0, room.enemies.Count);
                        if (room.enemies.Count != 0)
                        {
                            abilityM.projectileSpeed = 0f;
                            Instantiate(abilityM, launchPosition.position, transform.rotation);
                            Debug.Log("entra por enemy");
                        }
                        else if (currentRoom >= 21)
                        {
                            abilityM.projectileSpeed = 0f;
                            Instantiate(abilityM, launchPosition.position, transform.rotation);
                            Debug.Log("entra por boss");
                           
                        }
                    }
                    else
                        healthStealActivated = false;
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
            ballCoolDownCounter -= Time.deltaTime;
            //Debug.Log("Recargando elemental ball");
            if (ballCoolDownCounter <= 0f)
                ballAmmo = 5;
        }
    }

    private void StunSpell()
    {
        stunCounter -= Time.deltaTime;
        
        if (stunActivated)
        {
            if (stunCounter <= 0f)
            {
                if (inside != null)
                {
                    enemy = inside.GetComponent<Enemy>();
                    
                    if (enemy != null)
                    {
                        enemy.Stun();
                        Debug.Log("Stuneado");
                    }
                }

                stunActivated = false;
            }
            
        }

        stunCoolDownCounter -= Time.deltaTime;
    }

    private void HealthStealSpell()
    {
        stealCounter -= Time.deltaTime;

        if (healthStealActivated)
        {
            if (stealCounter <= 0)
            {
                if (!_attacked)
                {
                    foreach (Enemy e in room.enemies)
                    {
                        int i = 0;
                        if (room.enemies[i] == null)
                            i++;
                        else
                        {
                            room.enemies[i].TakeDamage(-healthStole);
                            alchemist.TakeDamage(healthSpellRestored);
                            Debug.Log("Encontro enemigo");
                            healthSpellAmmo--;
                            break;
                        }
                    }
                    //Debug.Log("Foreach rompido/terminado");

                    if (currentRoom >= 21)
                    {
                        boss.TakeDamage(-healthStole);
                        alchemist.TakeDamage(healthSpellRestored);
                        Debug.Log("Encontro jefe");
                        healthSpellAmmo--;
                    }
                }
                healthStealActivated = false;
            }
            else
            {
                //Debug.Log("Habilidad cancelada");
                //sfx comando invalido
            }

            
        }
        else
        {
            //Debug.Log("coolddown hechizo curacion");
            //sfx comando invalido
        }
    }
}


