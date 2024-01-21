using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 3f;

    public Rigidbody2D rb;

    public Vector2 movement;

    public GameController gameController;

    private bool canMove = true;

    // Update is called once per frame
    public void Update()
    {
        // input
        if (!canMove) movement.y = 0;
        else movement.y = Input.GetAxisRaw("Vertical");
    }

    public void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    public void GameOver()
    {
        canMove = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer != 6) return;
        gameController.GameOver();
    }
}