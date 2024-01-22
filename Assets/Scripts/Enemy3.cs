using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy3 : MonoBehaviour
{
    
    public GameController gameController;
    
    private float _speed = 4f;
    private float x;
    private float y;

    void Start()
    {
        Target();
        
    }
    
    void Target()
    {
        x = -50;
        y = transform.position.y > 0 ? Random.Range(-50, 0) : Random.Range(0, 50);
    }

    void Update()
    {
        var position = transform.position;
        position = Vector2.MoveTowards(position, position + new Vector3(x, y, 0), _speed * Time.deltaTime);
        transform.position = position;
        if (transform.position.x < -20) Destroy(gameObject);
    }
    
    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 7) Target();
        if (other.gameObject.layer != 3) return;
        gameController.ZombieKill();
        if (other.gameObject.transform.localScale.x < 0.2)
            Destroy(other.gameObject);
        else
            other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(52, 0);
        Destroy(gameObject);
    }
}
