using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components configuration")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform[] isGrounded;
    [SerializeField] private LayerMask layerFloor;
    
    [Header("Parameters configuration")]
    [SerializeField] private float horizontal;
    [SerializeField] private float impulse, minSpeed, speedMovevement, maxSpeed, 
                                   jumpForce, coyoteTime, coyoteTimeCounter, 
                                   jumpBufferTime, jumpBufferCounter, fallFaster;

    private Vector2 gravity;

    /* Testear:
     * 
     * impulse
     * minSpeed
     * maxSpeed
     * coyoteTime
     * jumBufferTime
     * fallFaster
     */

    void Start()
    {
        Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Wizard").GetComponent<Collider2D>(), GameObject.FindGameObjectWithTag("Alchemist").GetComponent<Collider2D>(), true);
 
        rb = GetComponent<Rigidbody2D>();

        gravity = new Vector2(0, -Physics2D.gravity.y);

        impulse = 0.1f; 
        minSpeed = 48f; 
        speedMovevement = minSpeed;
        maxSpeed = 80f; 
        jumpForce = 80f; 
        coyoteTime = 0.5f; 
        jumpBufferTime = 0.5f;
        fallFaster = 10f;
    }

    void Update()
    {
        /*
         * MAGO
         */
        WizardMovementSystem();

        /*
         * ALQUIMISTA
         */
        AlchemistMovementSystem();
    }

    private void FixedUpdate()
    {
        /*
         * MAGO
         */
        if (this.gameObject.tag == "Wizard")
        {
            rb.velocity = new Vector2(horizontal * speedMovevement, rb.velocity.y);
            if(horizontal == 1 || horizontal == -1)
            {
                if(speedMovevement >= maxSpeed)
                    speedMovevement = maxSpeed;
                else
                    speedMovevement += impulse + 0.2f; //Se demora 4s en llegar a maxSpeed
            }
            else
            {
                if (speedMovevement > minSpeed)
                    speedMovevement -= impulse; //Se demora 8s en llegar a minSpeed
                else
                    speedMovevement = minSpeed;
            }   
        }

        /*
         * ALQUIMISTA
         */
        if (this.gameObject.tag == "Alchemist")
        {
            rb.velocity = new Vector2(horizontal * speedMovevement, rb.velocity.y);
            if (horizontal == 1 || horizontal == -1)
            {
                if (speedMovevement >= maxSpeed)
                    speedMovevement = maxSpeed;
                else
                    speedMovevement += impulse; //Se demora 8s en llegar a maxSpeed
            }
            else
            {
                if (speedMovevement > minSpeed)
                    speedMovevement -= impulse + 0.2f; //Se demora 4s en llegar a minSpeed
                else
                    speedMovevement = minSpeed;
            }
        } 
    }

    private void WizardMovementSystem()
    {
        if (this.gameObject.tag == "Wizard")
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
                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                jumpBufferCounter = 0f;
            }

            if (Input.GetKeyUp(KeyCode.W) && rb.velocity.y > 0f) //Salta x altura de acuerdo al tiempo que se presiono la tecla
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.65f);
                coyoteTimeCounter = 0f;
            }

            //Flip
            if (horizontal == 1) //mira a la derecha
                transform.eulerAngles = new Vector2(0, 0);
            if (horizontal == -1) //mira a la izquierda
                transform.eulerAngles = new Vector2(0, 180);

            if (rb.velocity.y < 25)  //Caida automatica mas rapida
                rb.velocity -= gravity * fallFaster * Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.S)) //Caida mas rapida
                rb.velocity = new Vector2(rb.velocity.x, -jumpForce * 0.5f);
        }
    }

    private void AlchemistMovementSystem()
    {
        if (this.gameObject.tag == "Alchemist")
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
                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                jumpBufferCounter = 0f;
            }

            if (Input.GetKeyUp(KeyCode.UpArrow) && rb.velocity.y > 0) //Salta x altura de acuerdo al tiempo que se presiono la tecla
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

            if (Input.GetKeyDown(KeyCode.DownArrow)) //Caida mas rapida
                rb.velocity = new Vector2(rb.velocity.x, -jumpForce * 0.5f);
        }
    }

    private bool IsGrounded()
    {
        bool result = false;
        var raycast = new bool[3];
        raycast[0] = Physics2D.OverlapCircle(isGrounded[0].position, 0.2f, layerFloor);
        raycast[1] = Physics2D.OverlapCircle(isGrounded[1].position, 0.2f, layerFloor);
        raycast[2] = Physics2D.OverlapCircle(isGrounded[2].position, 0.2f, layerFloor);

        if (raycast[0] || raycast[1] || raycast[2])
            result = true;

        return result;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DroppingFloor"))
        {
            if (Input.GetKey(KeyCode.S) || (Input.GetKey(KeyCode.W) && !IsGrounded()))
                Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Wizard").GetComponent<Collider2D>(), GameObject.FindGameObjectWithTag("DroppingFloor").GetComponent<Collider2D>(), true);
            else
                Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Wizard").GetComponent<Collider2D>(), GameObject.FindGameObjectWithTag("DroppingFloor").GetComponent<Collider2D>(), false);

            if (Input.GetKey(KeyCode.DownArrow) || (Input.GetKey(KeyCode.UpArrow) && !IsGrounded()))
                Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Alchemist").GetComponent<Collider2D>(), GameObject.FindGameObjectWithTag("DroppingFloor").GetComponent<Collider2D>(), true);
            else
                Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Alchemist").GetComponent<Collider2D>(), GameObject.FindGameObjectWithTag("DroppingFloor").GetComponent<Collider2D>(), false);
        }
    }
}
