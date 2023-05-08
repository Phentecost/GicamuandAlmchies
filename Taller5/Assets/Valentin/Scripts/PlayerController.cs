using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Player components configuration
    [Header("Components configuration")]
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected Transform[] isGrounded;
    [SerializeField] protected LayerMask layerFloor;
    
    [Header("Parameters configuration")]
    [SerializeField] protected float horizontal;
    [SerializeField] protected float impulse, minSpeed, speedMovement, maxSpeed, 
                                   jumpForce, coyoteTime, coyoteTimeCounter, 
                                   jumpBufferTime, jumpBufferCounter, fallFaster;

    [Header("Health")]
    [SerializeField] protected int health;
    [SerializeField] protected int maxHealth;
    [SerializeField] protected float stunnedTime, stunnedCounter, attackedTime, attackedCounter;
    [SerializeField] protected bool attacked;

    [Header("Abilities configuration")]
    [SerializeField] protected Transform launchPosition;

    protected Vector2 gravity;

    /* Testear:
     * 
     * impulse
     * minSpeed
     * maxSpeed
     * coyoteTime
     * jumBufferTime
     * fallFaster
     */

    #endregion

    protected void Start()
    {
        Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Wizard").GetComponent<Collider2D>(), GameObject.FindGameObjectWithTag("Alchemist").GetComponent<Collider2D>(), true);

        rb = GetComponent<Rigidbody2D>();

        gravity = new Vector2(0, -Physics2D.gravity.y);

        impulse = 0.1f;
        minSpeed = 80f;
        speedMovement = minSpeed;
        maxSpeed = 100f;
        jumpForce = 80f;
        coyoteTime = 0.5f;
        jumpBufferTime = 0.5f;
        fallFaster = 10f;

        maxHealth = 10;
        health = maxHealth;
        stunnedTime = 1f;
        attacked = false;
        attackedTime = 2.5f;
    }


    protected virtual void Update()
    {
    }

    protected void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speedMovement, rb.velocity.y);
        if (horizontal == 1 || horizontal == -1)
        {
            if (speedMovement >= maxSpeed)
                speedMovement = maxSpeed;
            else
                speedMovement += impulse + 0.2f; //Se demora 4s en llegar a maxSpeed
        }
        else
        {
            if (speedMovement > minSpeed)
                speedMovement -= impulse; //Se demora 8s en llegar a minSpeed
            else
                speedMovement = minSpeed;
        }
    }

    protected bool IsGrounded()
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

    protected void OnTriggerStay2D(Collider2D collision)
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
