using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    
    public Rigidbody2D rb;
    
    public Vector2 movement;

    // Update is called once per frame
    public void Update()        
    {
        // input
        movement.x = 0;
        movement.y = Input.GetAxisRaw("Vertical");
        
    }

    public void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        
    }
}
