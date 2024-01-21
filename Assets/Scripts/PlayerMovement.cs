using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 3f;

    public Rigidbody2D rb;

    public Vector2 movement;

    public GameController gameController;
    
    public bool inverted = false;

    // Update is called once per frame
    public void Update()
    {
        // input
        movement.y = Input.GetAxisRaw("Vertical");
        if (inverted) movement.y *= -1;
    }

    public void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer != 6) return;
        gameController.GameOver();
    }
}