using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovment : MonoBehaviour
{

    public GameObject playerObject;
    public Camera mainCamera;
    public Bullet bullet;
    public GameObject ak;
    private Animator playerAnimator;
    private Rigidbody playerRB;
    private Vector2 mousePos;
    private RaycastHit hit;
    private Ray ray;
    private float maxMovmentSpeed = 0.1f;
    private float maxSidesMovementSpeed = 0.05f;
    private float velocityX;
    private float velocityZ;
    readonly float acceleration = 1f;
    float nextShoot = 0;
    float shootCooldown = 0.2f;



    void Start()
    {
        playerAnimator = playerObject.GetComponent<Animator>();
        playerRB = playerObject.GetComponent<Rigidbody>();
    }

    
    void FixedUpdate()
    {
        Movment();

        if (Mouse.current.leftButton.isPressed && Time.time > nextShoot)
        {
            nextShoot = Time.time + shootCooldown;
            Bullet newBullet = Instantiate(bullet, ak.transform.position, Quaternion.identity) as Bullet;
            Rigidbody newBulletRB = newBullet.GetComponent<Rigidbody>();
            newBulletRB.AddForce(playerObject.transform.forward * 1500);
            Destroy(newBullet.gameObject, 0.5f);
        }

        
    }

    private void Movment()
    {
        bool forwardPressed = Keyboard.current.wKey.IsPressed();
        bool backwardPressed = Keyboard.current.sKey.IsPressed();
        bool leftPressed = Keyboard.current.aKey.IsPressed();
        bool rightPressed = Keyboard.current.dKey.IsPressed();

        if (forwardPressed && velocityZ < maxMovmentSpeed)
        {
            velocityZ += Time.deltaTime * acceleration;
        }

        if (backwardPressed && velocityZ > -maxMovmentSpeed)
        {
            velocityZ -= Time.deltaTime * acceleration;
        }

        if (rightPressed && velocityX < maxSidesMovementSpeed)
        {
            velocityX += Time.deltaTime * acceleration;
        }

        if (leftPressed && velocityX > -maxSidesMovementSpeed)
        {
            velocityX -= Time.deltaTime * acceleration;
        }


        if (!forwardPressed && velocityZ > 0)
        {
            velocityZ -= Time.deltaTime * acceleration;
        }

        if (!backwardPressed && velocityZ < 0)
        {
            velocityZ += Time.deltaTime * acceleration;
        }

        if (!rightPressed && velocityX > 0)
        {
            velocityX -= Time.deltaTime * acceleration;
        }

        if (!leftPressed && velocityX < 0)
        {
            velocityX += Time.deltaTime * acceleration;
        }

        Vector3 playerMovment = playerObject.transform.right.normalized * velocityX + playerObject.transform.forward.normalized * velocityZ;
        playerObject.transform.position += Vector3.ClampMagnitude(playerMovment, maxMovmentSpeed);
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
