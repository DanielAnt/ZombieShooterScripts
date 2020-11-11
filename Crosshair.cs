using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Crosshair : MonoBehaviour
{
    private Camera playerCamera;
    private RaycastHit hit;
    private Ray ray;

    void Start()
    {
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }


    void Update()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        transform.position = mousePos;
        /*
        ray = playerCamera.ScreenPointToRay(mousePos);
        Vector2 mouseToWorldPos = playerCamera.WorldToScreenPoint(mousePos);
        if (Physics.Raycast(ray, out hit, 100))
        {
            transform.position = new Vector3(hit.point.x, hit.point.y+0.01f, hit.point.z);
        }
        */     

    }
}
