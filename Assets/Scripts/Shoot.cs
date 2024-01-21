using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Transform shootingPoint;
    public GameObject bulletPrefab;
    public float bulletForce;
    public float fireCooldown;
    public float cheapBulletDeviation;

    public bool isCheap;
    public bool isBig;
    
    private float _nextfire;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Shooting();
        }
            
    }

    private void Shooting()
    {
        if (!(Time.time > _nextfire)) return;
        _nextfire = Time.time + fireCooldown;
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation) as GameObject;
        if (isCheap) bullet.GetComponent<Bullet>().direction = Random.Range(-cheapBulletDeviation, cheapBulletDeviation);
        if (isBig)
        {
            bullet.transform.localScale *= 4;
            bullet.GetComponent<Bullet>().speed *= 2f;
        }

        bullet.GetComponent<Rigidbody2D>().AddForce(shootingPoint.right * bulletForce, ForceMode2D.Impulse);
    }
}
