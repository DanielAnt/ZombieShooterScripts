using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player: MonoBehaviour
{

    #region Instance
    private static Player _instance;
    public static Player Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion


    public CameraScript cameraPrefab;
    public Bullet bullet;
    public GameObject ak;
    public Grenade grenade;
    public int hitPoints = 5;
    public Vector3 crosshairPosition;


    private Camera playerCamera;
    private CharacterController controller;
    private Animator playerAnimator;
    private Rigidbody playerRB;
    private RaycastHit hit;
    private Ray ray;
    private float isShooting = 0;
    private float maxMovmentSpeed = 5f;
    private float maxSidesMovementSpeed = 5f;
    private float velocityX;
    private float velocityZ;
    readonly float acceleration = 14f;
    float nextShoot = 0;
    float shootCooldown = 0.05f;



    void Start()
    {
        playerAnimator = this.GetComponent<Animator>();
        controller = this.GetComponent<CharacterController>();
        CameraScript playerCameraScript = Instantiate(cameraPrefab, new Vector3(0, 0, 0), Quaternion.identity) as CameraScript;
        playerCameraScript.SetObjectToLookAt(Instance);
        playerCamera = playerCameraScript.GetComponent<Camera>();
        CalculateCrosshairPosition();
        //playerRB = this.GetComponent<Rigidbody>();
        //playerRB.isKinematic = true;

    }

    
    void Update()
    {
        CalculateCrosshairPosition();
        Movment();
        if (Keyboard.current.kKey.isPressed && Time.time > nextShoot)
        {
            nextShoot = Time.time + 5f;
            Grenade newGrenade = Instantiate(grenade, new Vector3(transform.position.x, transform.position.y+1, transform.position.z) , Quaternion.identity) as Grenade;
            newGrenade.CalculatePath(this.transform, crosshairPosition);
        }
        if (Mouse.current.leftButton.isPressed)
        {           
            isShooting = 1;
            if (Time.time > nextShoot)
            {

                Vector3 shootnigDirection = (crosshairPosition - ak.transform.position).normalized;
                nextShoot = Time.time + shootCooldown;
                Bullet newBullet = Instantiate(bullet, new Vector3(ak.transform.position.x, ak.transform.position.y, ak.transform.position.z), transform.rotation * Quaternion.Euler(90,0,0)) as Bullet;
                Rigidbody newBulletRB = newBullet.GetComponent<Rigidbody>();
                newBulletRB.AddForce(shootnigDirection * 750);
                Destroy(newBullet.gameObject, 2);
            }
        }
        else
        {
            isShooting -= 0.05f;
        }
        playerAnimator.SetFloat("isShooting", isShooting);

    }

    void CalculateCrosshairPosition()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        ray = playerCamera.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out hit, 100))
        {
            crosshairPosition = new Vector3(hit.point.x, hit.point.y+0.05f, hit.point.z);
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
               
        Vector3 playerMovment = transform.right.normalized * velocityX + transform.forward.normalized * velocityZ;
        playerMovment = Vector3.ClampMagnitude(playerMovment, maxMovmentSpeed);


        
        controller.SimpleMove(playerMovment);
        playerAnimator.SetFloat("Velocity X", velocityX);
        playerAnimator.SetFloat("Velocity Z", velocityZ);
    }

   
    void LateUpdate()
    {
        
        if (!Keyboard.current.spaceKey.isPressed)
        {
            /*
            Vector2 mousePos = Mouse.current.position.ReadValue();
            ray = playerCamera.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out hit, 100))
            {
                transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
            }
            */
            transform.LookAt(new Vector3(crosshairPosition.x, transform.position.y, crosshairPosition.z));
        }
        
    }
    

    public void getDamage(int aDamage)
    {
        hitPoints -= aDamage;
        if(hitPoints <= 0)
        {
            playerAnimator.SetBool("isDead", true);
        }
    }


}
