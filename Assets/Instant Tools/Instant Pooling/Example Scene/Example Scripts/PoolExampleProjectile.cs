using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolExampleProjectile : MonoBehaviour
{
    public float projectileSpeed;
    Rigidbody2D rb;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ActivateProjectile() 
    {
        rb.velocity = transform.right.normalized * projectileSpeed;
    }
}
