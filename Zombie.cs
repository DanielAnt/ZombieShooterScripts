using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    public Player player;

    public GameObject bloodEffect;
    public GameObject attackingHand;

    
    private GameObject animatedBody;
    private GameObject ragdoll;
    private GameObject playerObject;
    private Animator animator;
    private Collider zombieCollider;
    private NavMeshAgent movmentAgent;
    private float hitPoints = 15;
    private float velocity = 0;
    private float minDist = 2f;
    private float isAttacking = 0;
    private int spottingDistance = 15;
    private int loseAggroDistance = 25;
    private bool spotted;
    private bool alive;

    float attackCooldown = 0f;


    


    void Start()
    {
        spotted = false;
        alive = true;
        movmentAgent = this.GetComponent<NavMeshAgent>();
        zombieCollider = this.GetComponent<CapsuleCollider>();
        animator = this.GetComponentInChildren<Animator>();
        animatedBody = transform.Find("AnimatedBody").gameObject;
        ragdoll = transform.Find("Rigidbody").gameObject;
        animatedBody.SetActive(true);
        ragdoll.SetActive(false);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            movmentAgent.velocity = movmentAgent.velocity * 0.5f;
            //ALLWAYS SPAWNS BLOOD BEHIND ZOMBIE
            //GameObject particle = Instantiate(bloodEffect, new Vector3(this.transform.position.x, 1, this.transform.position.z), this.transform.rotation * Quaternion.Euler(0,135,0) );
            //SPAWNS BLOOD ON OPPOSITE SIDE OF ZOMBIES BULLET COLLISION
            GameObject particle = Instantiate(bloodEffect, new Vector3(this.transform.position.x, 1, this.transform.position.z), collision.gameObject.transform.rotation * Quaternion.Euler(0,135,0) );
            Destroy(particle.gameObject, 1);
            if (handleDamage(1))
            {
                Rigidbody zombieRb = ragdoll.GetComponentInChildren<Rigidbody>();
                zombieRb.AddExplosionForce(3000, transform.position + transform.forward + new Vector3(0, 1.6f, 0), 50);
            }

            /*
            if (hitPoints <= 0)
            {

                ## animation death
                int Random = UnityEngine.Random.Range(1, 3);
                if(Random == 1)
                {
                    animator.SetBool("isDead", true);
                }
                else if(Random == 2)
                {
                    animator.SetBool("isDead2", true);
                }
                ## animation death
                
                alive = false;
                zombieCollider.enabled = false;
                movmentAgent.enabled = false;
                animatedBody.SetActive(false);
                ragdoll.SetActive(true);
                CopyTransformData(animatedBody.transform, ragdoll.transform, movmentAgent.velocity);
                Rigidbody test = ragdoll.GetComponentInChildren<Rigidbody>();
                test.AddExplosionForce(3000, transform.position + transform.forward + new Vector3(0,1.6f,0) , 50);
                //isAttacking = 0;
                //velocity = 0;
                //velocity = Mathf.Clamp(velocity / 7, 0, 1);
                //isAttacking = Mathf.Clamp(isAttacking, 0, 1);
                //animator.SetFloat("isAttackingFloat", isAttacking);
                //animator.SetFloat("Velocity", velocity);
            }
            */
        }
    }

    public void spawnBlood(Transform place)
    {

    }
    public bool handleDamage(float damage)
    {
        hitPoints = hitPoints - damage;
        if (hitPoints <= 0)
        {
            alive = false;
            zombieCollider.enabled = false;
            movmentAgent.enabled = false;
            animatedBody.SetActive(false);
            ragdoll.SetActive(true);
            CopyTransformData(animatedBody.transform, ragdoll.transform, movmentAgent.velocity);
            return true;
        }
        else
        {
            return false;
        }
    }


    private void CopyTransformData(Transform sourceTransform, Transform destinationTransform, Vector3 velocity)
    {
        for (int i = 0; i < destinationTransform.childCount; i++)
        {
            var source = sourceTransform.GetChild(i);
            var destination = destinationTransform.GetChild(i);
            destination.position = source.position;
            destination.rotation = source.rotation;
            var rb = destination.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.velocity = velocity;
            }

            CopyTransformData(source, destination, velocity);

        }
    }

    
    void Update()
    {
        if (!playerObject)
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
        }
        if (alive && playerObject)
        {
            if (Vector3.Distance(transform.position, playerObject.transform.position) <= spottingDistance)
            {
                spotted = true;
            }
            else if(Vector3.Distance(transform.position, playerObject.transform.position) >= loseAggroDistance)
            {
                spotted = false;
            }

            if (spotted)
            {
                this.transform.LookAt(playerObject.transform.position);
                movmentAgent.SetDestination(playerObject.transform.position);
                velocity = movmentAgent.velocity.magnitude;

                if (Vector3.Distance(transform.position, playerObject.transform.position) <= minDist)
                {
                    isAttacking = 1f;

                }
                else
                {
                    isAttacking -= 0.01f;
                }
                velocity = Mathf.Clamp(velocity, 0, 1);
                isAttacking = Mathf.Clamp(isAttacking, 0, 1);
                animator.SetLayerWeight(1, isAttacking);
                animator.SetFloat("Velocity", velocity);
                if(isAttacking > 0 && Time.time > attackCooldown)
                {
                    Collider[] hitObjects = Physics.OverlapSphere(attackingHand.transform.position, 0.5f);
                    foreach(Collider hitObject in hitObjects)
                    {
                        if (hitObject.CompareTag("Player"))
                        {
                            Player hittedPlayer = hitObject.GetComponent<Player>();
                            hittedPlayer.GetDamage(2);
                            attackCooldown = Time.time + 2.2f;
                        }
                    }

                }

            }
        }
       

        /*
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
       */
       


    }



}
