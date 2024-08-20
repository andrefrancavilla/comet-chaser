using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InstantTools.PoolSystem;

public class PoolExampleWeapon : MonoBehaviour
{
    public string projectileType;
    public Transform spawnPos;
    public float fireRate;
    
    GameObject projectile;
    [HideInInspector]
    public float fireT;

    Vector3 dir;
    float angle;

    void Update()
    {
        //Cursor aiming code
        dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if(Input.GetButton("Fire1") && fireT >= 1)
        {
            projectile = PoolManager.ActivateObject(projectileType, spawnPos.position, Quaternion.AngleAxis(angle, Vector3.forward));

            //ActivateProjectile is a method that tells the projectile to start moving -- you can handle the fished objects in whatever way you want.
            if(projectile)
                projectile.GetComponent<PoolExampleProjectile>().ActivateProjectile(); 
            fireT = 0;
        }

        if(fireT < 1)
        {
            fireT += Time.deltaTime * fireRate;
        }        
    }
}
