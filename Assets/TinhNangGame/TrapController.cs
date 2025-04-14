using System.Collections;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    [SerializeField] private int health = 3; // Máu của bẫy
    [SerializeField] private float fireRate = 1.5f; // Tốc độ bắn
    [SerializeField] private float detectionRadius = 5f; // Phạm vi phát hiện người chơi
    [SerializeField] private Transform firePoint; // Vị trí bắn đạn
    [SerializeField] private GameObject enemyBulletPrefab; // Prefab đạn

    private Transform player; // Tham chiếu đến người chơi
    private Coroutine shootingCoroutine = null; // Để điều khiển coroutine

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < detectionRadius)
        {
            if (shootingCoroutine == null)
                shootingCoroutine = StartCoroutine(ShootRoutine());
        }
        else
        {
            if (shootingCoroutine != null)
            {
                StopCoroutine(shootingCoroutine);
                shootingCoroutine = null;
            }
        }
    }

    IEnumerator ShootRoutine()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(fireRate);
        }
    }

    void Shoot()
    {
        if (enemyBulletPrefab != null && firePoint != null)
        {
            Instantiate(enemyBulletPrefab, firePoint.position, firePoint.rotation);
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
