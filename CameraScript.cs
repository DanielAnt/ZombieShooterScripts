using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{

    public GameObject player;

    private Vector3 offset;

    public float smoothTime = 1F;
    private Transform playerPos;
    private Vector3 velocity = Vector3.zero;
     


    void Start()
    {
        playerPos = player.transform;
        offset = new Vector3(0, 20, -8);
        transform.position = player.transform.position + offset;
        transform.rotation = Quaternion.Euler(67, 0, 0);
    }
    void Update()
    {
        offset -= (offset * Mouse.current.scroll.y.ReadValue() / 1500);
        Vector3 targetPosition = player.transform.position + offset;
        
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
