using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy3 : MonoBehaviour
{
    
    public float speed;
    
    public GameController gameController;
    
    private float x;
    private float y;

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
        var position = transform.position;
        position = Vector2.MoveTowards(position, position + new Vector3(x, y, 0), speed * Time.deltaTime);
        transform.position = position;
        if (transform.position.x < -20) Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 7) Target();
        if (other.gameObject.layer != 3) return;
        gameController.ZombieKill();
        Destroy(other.gameObject);
        Destroy(gameObject);
    }
}
