using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float speed = 2f; // Tốc độ di chuyển của quái
    [SerializeField] private Transform player; // Tham chiếu đến người chơi
    [SerializeField] private float detectionRange = 5f; // Phạm vi phát hiện người chơi
    [SerializeField] private Transform firePoint; // Vị trí bắn đạn
    [SerializeField] private GameObject enemyBulletPrefab; // Prefab của viên đạn
    [SerializeField] private int health = 1; // Máu của quái
    [SerializeField] private float shootInterval = 1f; // Thời gian giữa các lần bắn

    private bool isChasingPlayer = false; // Trạng thái đuổi theo người chơi

    void Start()
    {
        InvokeRepeating("Shoot", 1f, shootInterval); // Bắt đầu bắn theo chu kỳ
    }

    void Update()
    {
        // Kiểm tra khoảng cách giữa quái vật và người chơi
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Nếu trong phạm vi phát hiện, bắt đầu đuổi theo người chơi
        if (distanceToPlayer < detectionRange)
        {
            isChasingPlayer = true;
        }
        else
        {
            isChasingPlayer = false;
        }

        // Di chuyển về phía người chơi nếu đang đuổi
        if (isChasingPlayer)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

    void Shoot()
    {
        // Kiểm tra nếu quái vật đang trong phạm vi phát hiện và không bắn khi ngoài phạm vi
        if (!isChasingPlayer || player == null || enemyBulletPrefab == null || firePoint == null)
            return;

        // Tạo viên đạn
        GameObject bullet = Instantiate(enemyBulletPrefab, firePoint.position, Quaternion.identity);
        EnemyBullet bulletScript = bullet.GetComponent<EnemyBullet>();

        if (bulletScript != null)
        {
            // Tính hướng bắn và thiết lập cho viên đạn
            Vector2 direction = (player.position - firePoint.position).normalized;
            bulletScript.SetDirection(direction);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject); // Phá hủy quái vật khi máu về 0
        }
    }

    // Vẽ vùng radius phát hiện người chơi trong editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange); // Vẽ vòng tròn phạm vi phát hiện
    }
}
