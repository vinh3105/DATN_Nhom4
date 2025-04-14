using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform targetPlayer; // Tham chiếu đến người chơi
    [SerializeField] private float enemyMoveSpeed = 2f; // Tốc độ di chuyển của quái
    [SerializeField] private float detectionRadius = 5f; // Phạm vi phát hiện người chơi

    private bool isChasingPlayer = false; // Trạng thái có đang đuổi người chơi hay không

    void Update()
    {
        // Tính khoảng cách đến người chơi
        float distanceToPlayer = Vector2.Distance(transform.position, targetPlayer.position);

        // Kiểm tra nếu trong phạm vi thì bắt đầu đuổi
        if (distanceToPlayer < detectionRadius)
        {
            isChasingPlayer = true;
        }
        else
        {
            isChasingPlayer = false;
        }

        // Nếu đang đuổi, di chuyển về phía người chơi
        if (isChasingPlayer)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPlayer.position, enemyMoveSpeed * Time.deltaTime);
        }
    }

    // Vẽ vùng radius trong editor để dễ dàng kiểm tra phạm vi
    void OnDrawGizmosSelected()
    {
        // Vẽ hình tròn màu xanh dương, có bán kính là detectionRadius
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
