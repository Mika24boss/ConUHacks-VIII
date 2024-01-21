using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Enemy2 : MonoBehaviour
{
    public GameController gameController;
    
    private float _speed = 4f;
    
    // Update is called once per frame
    void Update()
    {
        var position = transform.position;
        position = Vector2.MoveTowards(position, position - new Vector3(10, 0, 0), _speed * Time.deltaTime);
        transform.position = position;
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