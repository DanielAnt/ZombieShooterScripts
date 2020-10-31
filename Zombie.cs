using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    public GameObject playerObject;
    
    private Animator animator;
    private CharacterController controller;
    private float hitPoints = 3;
    private float maxSpeed = 0.11f;
    private float velocity = 0;
    private float acceleration = 0.045f;
    private float minDist = 1.2f;
    private float isAttacking = 0;
    private int spottingDistance = 15;
    private int loseAggroDistance = 25;
    private bool spotted;
    private bool alive;

    
    
    

    void Start()
    {
        spotted = false;
        alive = true;
        animator = this.GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            hitPoints--;
            if (hitPoints <= 0)
            {
                alive = false;
                animator.SetBool("isDead", true);
                controller.enabled = false;
            }
        }
    }


    void FixedUpdate()
    {
        isAttacking -= 0.01f;
        animator.SetBool("isAttacking", false);
        if (alive)
        {
            
            if (Vector3.Distance(transform.position, playerObject.transform.position) <= spottingDistance)
            {
                spotted = true;
                transform.LookAt(playerObject.transform.position);
            }

            if (Vector3.Distance(transform.position, playerObject.transform.position) >= minDist && spotted && velocity < maxSpeed)
            {
                velocity += Time.deltaTime * acceleration;
            }
            else if(Vector3.Distance(transform.position, playerObject.transform.position) < minDist && velocity >= 0)
            {
                //animator.SetBool("isAttacking", true);
                isAttacking = 1f;
                velocity -= 0.04f;
                if (velocity < 0) {
                    velocity = 0;
                }
            }
            else if (!spotted && velocity > 0)
            {
                velocity -= Time.deltaTime * acceleration;
                if (velocity < 0)
                {
                    velocity = 0;
                }
            }
            if(velocity > maxSpeed)
            {
                velocity = maxSpeed;
            }
            
            if(isAttacking < 0)
            {
                isAttacking = 0;
            }

            controller.Move(transform.forward.normalized * velocity);
            Debug.Log(velocity + " " + isAttacking);
            animator.SetFloat("Velocity", velocity * 10);
            animator.SetFloat("isAttackingFloat", isAttacking);
            //transform.position += transform.forward * velocity;
        }
        else
        {
            if(velocity > 0)
            {
                velocity -= Time.deltaTime * acceleration * 10;
                isAttacking = 0;
                if(velocity < 0)
                {
                    velocity = 0;
                }
            }
        }
       
    }

    void LateUpdate()
    {
        if(Vector3.Distance(transform.position, playerObject.transform.position) > loseAggroDistance)
        {
            spotted = false;
        }
    }


}
