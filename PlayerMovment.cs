using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovment : MonoBehaviour
{

    public GameObject playerObject;
    public Camera mainCamera;
    public Bullet bullet;
    public GameObject ak;
    private CharacterController controller;
    private Animator playerAnimator;
    private Rigidbody playerRB;
    private Vector2 mousePos;
    private RaycastHit hit;
    private Ray ray;
    private float isShooting = 0;
    private float maxMovmentSpeed = 5f;
    private float maxSidesMovementSpeed = 5f;
    private float velocityX;
    private float velocityZ;
    readonly float acceleration = 14f;
    float nextShoot = 0;
    float shootCooldown = 0.2f;



    void Start()
    {
        playerAnimator = this.GetComponent<Animator>();
        controller = this.GetComponent<CharacterController>();
        //playerRB = this.GetComponent<Rigidbody>();
        //playerRB.isKinematic = true;
    }

    
    void Update()
    {
              
        Movment();
        if (Mouse.current.leftButton.isPressed)
        {           
            isShooting = 1;
            if (Time.time > nextShoot)
            {
                nextShoot = Time.time + shootCooldown;
                Bullet newBullet = Instantiate(bullet, ak.transform.position, Quaternion.identity) as Bullet;
                Rigidbody newBulletRB = newBullet.GetComponent<Rigidbody>();
                newBulletRB.AddForce(playerObject.transform.forward * 1500);
                Destroy(newBullet.gameObject, 0.75f);
            }
        }
        else
        {
            isShooting -= 0.05f;
        }
        playerAnimator.SetFloat("isShooting", isShooting);

    }

    private void Movment()
    {
        bool forwardPressed = Keyboard.current.wKey.IsPressed();
        bool backwardPressed = Keyboard.current.sKey.IsPressed();
        bool leftPressed = Keyboard.current.aKey.IsPressed();
        bool rightPressed = Keyboard.current.dKey.IsPressed();

        if (forwardPressed && velocityZ < maxMovmentSpeed)
        {
            velocityZ += Time.fixedDeltaTime * acceleration;
        }

        if (backwardPressed && velocityZ > -maxMovmentSpeed)
        {
            velocityZ -= Time.fixedDeltaTime * acceleration;
        }

        if (rightPressed && velocityX < maxSidesMovementSpeed)
        {
            velocityX += Time.fixedDeltaTime * acceleration;
        }

        if (leftPressed && velocityX > -maxSidesMovementSpeed)
        {
            velocityX -= Time.fixedDeltaTime * acceleration;
        }


        if (!forwardPressed && velocityZ > 0)
        {
            velocityZ -= Time.fixedDeltaTime * acceleration;
            if(velocityZ < 0)
            {
                velocityZ = 0;
            }
        }

        if (!backwardPressed && velocityZ < 0)
        {
            velocityZ += Time.fixedDeltaTime * acceleration;
            if (velocityZ > 0)
            {
                velocityZ = 0;
            }
        }

        if (!rightPressed && velocityX > 0)
        {
            velocityX -= Time.fixedDeltaTime * acceleration;
            if (velocityX < 0)
            {
                velocityX = 0;
            }
        }

        if (!leftPressed && velocityX < 0)
        {
            velocityX += Time.fixedDeltaTime * acceleration;
            if (velocityX > 0)
            {
                velocityX = 0;
            }
        }
               
        Vector3 playerMovment = playerObject.transform.right.normalized * velocityX + playerObject.transform.forward.normalized * velocityZ;
        playerMovment = Vector3.ClampMagnitude(playerMovment, maxMovmentSpeed);

        //playerRB.MovePosition(transform.position + playerMovment * Time.fixedDeltaTime * 150);

        //Debug.Log(playerMovment.magnitude);
        controller.SimpleMove(playerMovment);
        playerAnimator.SetFloat("Velocity X", velocityX);
        playerAnimator.SetFloat("Velocity Z", velocityZ);


       


    }
    
    
    void LateUpdate()
    {
        if (!Keyboard.current.spaceKey.isPressed)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            ray = Camera.main.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out hit, 100))
            {
                transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
            }
        }
    }
    

}
