using System.Collections;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    [SerializeField] private int health = 3; // Máu của bẫy
    [SerializeField] private float fireRate = 1.5f; // Tốc độ bắn
    [SerializeField] private Transform firePoint; // Vị trí bắn đạn
    [SerializeField] private GameObject enemyBulletPrefab; // Prefab đạn
    private Transform player; // Người chơi
    private bool isShooting = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isShooting)
        {
            player = collision.transform; // Ghi nhận vị trí người chơi
            StartCoroutine(ShootRoutine());
        }
    }

    IEnumerator ShootRoutine()
    {
        isShooting = true;
        while (player != null)
        {
            Shoot();
            yield return new WaitForSeconds(fireRate);
        }
        isShooting = false;
    }

    void Shoot()
    {
        if (firePoint == null || enemyBulletPrefab == null || player == null)
        {
            return;
        }

        GameObject bullet = Instantiate(enemyBulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            Vector2 direction = (player.position - firePoint.position).normalized;
            bullet.GetComponent<EnemyBullet>().SetDirection(direction);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
