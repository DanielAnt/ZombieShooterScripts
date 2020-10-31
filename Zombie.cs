using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public GameObject playerObject;
    
    private Collider boxCollider;
    private Animator animator;
    private float hitPoints = 3;
    private float maxSpeed = 0.11f;
    private float velocity = 0;
    private float acceleration = 0.05f;
    private int spottingDistance = 15;
    private int loseAggroDistance = 25;
    private bool spotted;
    private bool alive;
    private int minDist = 1;
    
    

    void Start()
    {
        spotted = false;
        alive = true;
        animator = this.GetComponent<Animator>();
        boxCollider = GetComponent<Collider>();
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
                boxCollider.enabled = false;
            }
        }
    }


    void FixedUpdate()
    {
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
                animator.SetBool("isAttacking", true);
                velocity -= 0.1f;
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
            Debug.Log(velocity);


            animator.SetFloat("Velocity", velocity);
            transform.position += transform.forward * velocity;
        }
        else
        {
            if(velocity > 0)
            {
                velocity -= Time.deltaTime * acceleration * 10;
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
