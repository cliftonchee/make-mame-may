using UnityEngine;

public class RangedWeapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    [SerializeField] private float bulletCooldown = 0.1f;
    private float bulletStart = 0f;

    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            if (Time.time > bulletStart + bulletCooldown)
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
