using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Transform shootingPoint;
    public GameObject bulletPrefab;
    public float bulletForce;
    public float fireCooldown;
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
        bullet.GetComponent<Rigidbody2D>().AddForce(shootingPoint.right * bulletForce, ForceMode2D.Impulse);
    }
}
