using System;
using UnityEngine;

public class Scroller : MonoBehaviour
{
    public float speed;
    public float xReset;
    public float xOffset;
    public Transform gridTransform1, gridTransform2;
    private bool _isScrolling = true;

    private void Update()
    {
        if (!_isScrolling) return;

        var movement = Vector3.left * (speed * Time.deltaTime);
        gridTransform1.position += movement;
        gridTransform2.position += movement;
        
        if (gridTransform1.position.x < xReset)
            gridTransform1.position += Vector3.right * xOffset;
        else if (gridTransform2.position.x < xReset)
            gridTransform2.position += Vector3.right * xOffset;
    }

    public void GameOver()
    {
        _isScrolling = false;
    }
}