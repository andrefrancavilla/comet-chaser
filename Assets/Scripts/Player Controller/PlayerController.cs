using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    /*
     * I've opted to use Unity's old input system due to the simplicity of the game
     * Most features present in Unity's new input system would be unused
     */
    
    private Camera _camera;
    private bool _moving;

    private void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Move(Input.GetTouch(0).position);
        }
        else if (Input.GetButton("Fire1"))
        {
            Move(Input.mousePosition);
        }
        else
        {
            _moving = false;
        }
    }

    private void Move(Vector2 screenPosition)
    {
        Ray ray = _camera.ScreenPointToRay(screenPosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        if(hit.transform && hit.transform.CompareTag("Player") || _moving)
        {
            Vector3 worldPos = _camera.ScreenToWorldPoint(screenPosition);
            worldPos.z = transform.position.z;
            worldPos.y = transform.position.y;
            transform.position = worldPos;
            _moving = true;
        }
    }
    
}
