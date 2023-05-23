using Code;
using Code_DungeonSystem;
using System.Collections;
using System.Collections.Generic;
using TarodevController;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Alchemist : PlayerController
{
    [SerializeField] private BoxBarrier abilityQ;
    [SerializeField] private MagicPellets abilityE;
    [SerializeField] private HealPowerIcon abilityF;


    [Header("Alchemist's Abilities")]
    [Header("Barrier")]
    [SerializeField] public float barrierCoolDown = 12f;
    [SerializeField] private bool barrierActivated = false;

    [Header("Magic pellets")]
    [SerializeField] private int pelletsAmmo = 3;
    [SerializeField] private float pelletsTime = 1.5f;
    [SerializeField] private float pelletsCounter;
    [SerializeField] public float pelletsCoolDown = 0.5f;
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

    [Header("Animation")]
    [SerializeField] Animator animator;

    protected override void Update()
    {
        GatherInput();
        base.Update();
        AbilitiesSystem();

        wizard = DungeonManager.instance.Gicamu.GetComponent<Wizard>();

        inside = Physics2D.OverlapCircle(healRadius.position, 0.5f, layerOtherPlayer);

        animator = GetComponent<Animator>();
        
        Barrier();
        MagicPellets();
        HealPower();
    }

    protected override void GatherInput()
    {
        Input = new FrameInput
        {
            JumpDown = UnityEngine.Input.GetKeyDown(KeyCode.W),
            JumpUp = UnityEngine.Input.GetKeyUp(KeyCode.W),
            FallDown = UnityEngine.Input.GetKey(KeyCode.S),
            X = UnityEngine.Input.GetAxisRaw("P1_Horizontal"),
            A1 = UnityEngine.Input.GetKeyDown(KeyCode.Q),
            A2 = UnityEngine.Input.GetKeyDown(KeyCode.E),
            A3 = UnityEngine.Input.GetKeyDown(KeyCode.F)
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

    protected void AbilitiesSystem()
    {
        //Barrera
        if (Input.A1)
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
        if (Input.A2)
        {
            if (pelletsAmmo > 0f)
            {
                if (!pelletsActivated)
                {
                    pelletsCounter = pelletsTime;
                    pelletsCoolDown = 0.5f;
                    pelletsActivated = true;

                    abilityE.projectileSpeed = 10f;
                    Instantiate(abilityE, launchPosition.position, transform.rotation);

                    cd(2);
                }
            }
            else
            {
                Debug.Log("No ammo");
                //sfx comando invalido
            }
        }

        //Heal cercano
        if (Input.A3)
        {
            if (healAmmo > 0)
            {
                if (inside)
                {
                    if (!healActivated)
                    {
                        healCounter = healTime;
                        healActivated = true;

                        abilityE.projectileSpeed = 0f;
                        Instantiate(abilityF, launchPosition.position, transform.rotation);
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
                if (!_attacked)
                {
                    if (inside)
                    {
                        wizard.TakeDamage(healthPowerRestored);
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


