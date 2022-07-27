using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float fireSpeed = 20f;

    private void Start() {
        GameManager.Instance.projSpeed = fireSpeed;
    }
    public void Fire(){
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * GameManager.Instance.projSpeed, ForceMode2D.Impulse);
        Destroy(bullet, 10f);
    }
}
