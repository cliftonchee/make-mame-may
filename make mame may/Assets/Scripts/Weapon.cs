using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    [SerializeField] private float bulletCooldown = 0.1f;
    private float bulletStart = 0f;
    // Update is called once per frame
    void Update()
    {
       if (Input.GetButtonDown("Fire2"))
       {
            if(Time.time > bulletStart + bulletCooldown)
            {
                bulletStart = Time.time;
                Shoot();         
            }
            
       } 
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
