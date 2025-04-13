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

	// Khi người chơi bước vào vùng trigger (nếu có gắn collider dạng trigger)
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			isChasingPlayer = true;
		}
	}

	// Khi người chơi bước ra khỏi vùng trigger
	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			isChasingPlayer = false;
		}
	}
}
