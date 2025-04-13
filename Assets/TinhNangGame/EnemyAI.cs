using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform player; // Người chơi
    [SerializeField] private float speed = 2f; // Tốc độ di chuyển
    [SerializeField] private float detectionRange = 5f; // Phạm vi phát hiện

    private bool isChasing = false; // Kiểm tra có đang đuổi không

    void Update()
    {
        // Kiểm tra khoảng cách đến người chơi
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer < detectionRange)
        {
            isChasing = true; // Bắt đầu đuổi nếu người chơi nằm trong phạm vi
        }
        else
        {
            isChasing = false; // Dừng đuổi nếu ra ngoài phạm vi
        }

        if (isChasing)
        {
            // Di chuyển quái về phía người chơi
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isChasing = true; // Khi phát hiện người chơi, bắt đầu đuổi
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isChasing = false; // Khi người chơi rời khỏi vùng phát hiện, dừng đuổi
        }
    }
}