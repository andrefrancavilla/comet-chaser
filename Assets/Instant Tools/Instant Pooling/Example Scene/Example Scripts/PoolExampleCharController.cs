using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolExampleCharController : MonoBehaviour
{
    public float moveSpeed;

    Rigidbody2D rb;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, rb.velocity.y);
    }
}
