using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Enemy2 : MonoBehaviour
{
    public float speed;
    
    public GameController gameController;
    
    // Update is called once per frame
    void Update()
    {
        var position = transform.position;
        position = Vector2.MoveTowards(position, position - new Vector3(10, 0, 0), speed * Time.deltaTime);
        transform.position = position;
        if (transform.position.x < -20) Destroy(gameObject);
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer != 3) return;
        gameController.ZombieKill();
        Destroy(other.gameObject);
        Destroy(gameObject);
    }
}