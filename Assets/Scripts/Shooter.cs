using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Transform target;
    public float shootInterval = 1.5f;
    public float bulletSpeed = 8f;
    public float playerSpeed;

    private float timer;

    void Awake()
    {
        playerSpeed = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().speed;
        firePoint = transform;
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        if (target == null) return;

        timer += Time.deltaTime;
        if (timer >= shootInterval)
        {
            Shoot();
            timer = 0f;
        }
    }

    void Shoot()
    {
        Vector2 firePos = firePoint.position;
        Vector2 targetPos = target.position;

        float distance = Vector2.Distance(targetPos, firePos);
        float offsetY = playerSpeed * 2 * (distance / bulletSpeed);
        Vector2 aimPos = new Vector2(targetPos.x, targetPos.y + offsetY);

        Vector2 direction = (aimPos - firePos).normalized;
        GameObject bullet = Instantiate(bulletPrefab, firePos, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed;
        }
        HurtComponent hurt = bullet.GetComponent<HurtComponent>();
        if (hurt != null)
        {
            hurt.owner = HurtComponent.OwnerType.Enemy;
        }
    }
}
