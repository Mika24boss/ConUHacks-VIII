using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float direction = 0;
    public float speed = 26f;
    
    private float _deviation = 0f;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
        StartCoroutine(DeleteMyself());
    }

    private void FixedUpdate()
    {
        rb.velocity += new Vector2(0, _deviation * Time.fixedDeltaTime);
        _deviation += direction;
    }

    // Update is called once per frame
    private void Update()
    {
        if (transform.position.x > 20) Destroy(gameObject);
    }

    private IEnumerator DeleteMyself()
    {
        yield return new WaitForSeconds(6);
        Destroy(gameObject);
    }
}