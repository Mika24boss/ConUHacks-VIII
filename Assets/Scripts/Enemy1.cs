using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class Enemy1 : MonoBehaviour
{
    public GameObject player;
    public GameController gameController;

    private float _speed = 4f;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, _speed * Time.deltaTime);
        if (transform.position.x < -20) Destroy(gameObject);
    }
    
    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer != 3) return;
        gameController.ZombieKill();
        Destroy(other.gameObject);
        Destroy(gameObject);
    }
}