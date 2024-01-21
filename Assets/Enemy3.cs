using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy3 : MonoBehaviour
{
    public float speed;
    private float x;
    private float y;
    private Vector3 lastVelocity;

    void Awake()
    {
        //rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        Target();
        
    }
    
    void Target()
    {
        x = -50;
        y = Random.Range(-50, 50);
        
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, transform.position + new Vector3(x, y, 0), speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        Target();
    }
}
