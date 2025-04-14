using System.Collections;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    [SerializeField] private float detectionRange = 5f;           // Phạm vi phát hiện người chơi
    [SerializeField] private Transform firePoint;                 // Vị trí bắn đạn
    [SerializeField] private GameObject enemyBulletPrefab;       // Prefab đạn
    [SerializeField] private int health = 3;                      // Máu của bẫy
    [SerializeField] private float shootInterval = 1f;           // Khoảng thời gian giữa các lần bắn

    private Transform player;                                    // Tham chiếu tới người chơi
    private Coroutine shootCoroutine;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogWarning("Không tìm thấy Player!");
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < detectionRange)
        {
            if (shootCoroutine == null)
                shootCoroutine = StartCoroutine(ShootRoutine());
        }
        else
        {
            if (shootCoroutine != null)
            {
                StopCoroutine(shootCoroutine);
                shootCoroutine = null;
            }
        }
    }

    IEnumerator ShootRoutine()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(shootInterval);
        }
    }

    void Shoot()
    {
        if (enemyBulletPrefab != null && firePoint != null && player != null)
        {
            GameObject bullet = Instantiate(enemyBulletPrefab, firePoint.position, Quaternion.identity);
            EnemyBullet bulletScript = bullet.GetComponent<EnemyBullet>();

            if (bulletScript != null)
            {
                Vector2 direction = (player.position - firePoint.position).normalized;
                bulletScript.SetDirection(direction);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject); // Phá hủy bẫy khi hết máu
        }
    }

    // Vẽ vùng detection trong Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
