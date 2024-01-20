using System;
using UnityEngine;

public class Scroller : MonoBehaviour
{
    public float speed;
    public float xReset;
    public float xOffset;
    private Transform _gridTransform;

    private void Start()
    {
        _gridTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        _gridTransform.position += Vector3.left * (speed * Time.deltaTime);
        if (_gridTransform.position.x < xReset)
        {
            _gridTransform.position += Vector3.right * xOffset;
        }
    }
}
