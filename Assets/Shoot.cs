using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    public Transform shootingPoint;
    public GameObject bulletPrefab;
    public float bulletForce;
    public float firerate;
    private float nextfire;

    
    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.spaceKey.isPressed)
        {
            Shooting();
        }
            
    }

    void Shooting()
    {
        if (Time.time > nextfire)
        {
            nextfire = Time.time + firerate;
            GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation) as GameObject;
            bullet.GetComponent<Rigidbody2D>().AddForce(shootingPoint.right * bulletForce, ForceMode2D.Impulse);
        }
    }
}
