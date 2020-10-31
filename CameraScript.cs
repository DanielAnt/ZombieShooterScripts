using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{

    public Camera mainCamera;
    public GameObject player;

    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(0, 20, -8);
        mainCamera.transform.position = player.transform.position + offset;
        //mainCamera.transform.LookAt(player.transform.position);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        offset -= (offset * Mouse.current.scroll.y.ReadValue() / 1500);
        mainCamera.transform.position = player.transform.position + offset;
        //mainCamera.transform.LookAt(player.transform.position);
    }
}
