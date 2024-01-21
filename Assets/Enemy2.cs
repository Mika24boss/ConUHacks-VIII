using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Enemy2 : MonoBehaviour
{
    public float speed;
    
    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, transform.position - new Vector3(10, 0, 0), speed * Time.deltaTime);
    }
}