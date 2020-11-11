using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{


    private Vector3 offset;
    public float smoothTime = 1F;

    private Player player;
    private Transform playerPos;
    private Vector3 velocity = Vector3.zero;


    void Update()
    {
        offset -= (offset * Mouse.current.scroll.y.ReadValue() / 1500);
        Vector3 targetPosition = Vector3.Lerp(playerPos.position, player.crosshairPosition, 0.3f) + offset;
        
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    public void SetObjectToLookAt(Player aPlayer)
    {
        player = aPlayer;
        playerPos = player.transform;
        offset = new Vector3(0, 20, -8);
        transform.position = playerPos.position + offset;
        transform.rotation = Quaternion.Euler(67, 0, 0);
    }
}
