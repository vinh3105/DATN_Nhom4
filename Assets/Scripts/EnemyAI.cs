using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float chaseSpeed = 3.5f;
    [SerializeField] private float patrolDistance = 3f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform attackZone;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float attackRange = 6f;
    [SerializeField] private float attackHeight = 1f;

    private Vector2 startPoint;
    private bool isChasing = false;
    private Transform player;
    private int direction;
    private bool returningToStart = false;

    void Start()
    {
        startPoint = transform.position;
        direction = Random.value < 0.5f ? -1 : 1;
        if (direction == -1)
        {
            Flip(); // Đảo chiều ban đầu nếu đi sang trái
        }
    }

    void Update()
    {
        Vector2 boxSize = new Vector2(attackRange, 1f); 
        Collider2D playerInRange = Physics2D.OverlapBox(attackZone.position, boxSize, 0f, playerLayer);

        if (playerInRange != null)
        {
            player = playerInRange.transform;
            isChasing = true;
            returningToStart = false;
        }
        else
        {
            if (isChasing)
            {
                // Bắt đầu quay lại điểm tuần tra sau khi mất dấu player
                isChasing = false;
                returningToStart = true;
            }
        }

        if (isChasing && player != null)
        {
            ChasePlayer();
        }
        else if (returningToStart)
        {
            ReturnToStartPoint();
        }
        else
        {
            Patrol();
        }

    }
    void ReturnToStartPoint()
    {
        float distance = Vector2.Distance(transform.position, startPoint);
        float dir = Mathf.Sign(startPoint.x - transform.position.x);

        transform.Translate(Vector2.right * dir * moveSpeed * Time.deltaTime);

        if (Mathf.Abs(startPoint.x - transform.position.x) < 0.1f)
        {
            returningToStart = false;
            transform.position = new Vector3(startPoint.x, transform.position.y, transform.position.z);
        }

        // Flip nếu cần
        if (dir != direction)
        {
            direction = (int)dir;
            Flip();
        }
    }

    void Patrol()
    {
        transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);

        if (Mathf.Abs(transform.position.x - startPoint.x) >= patrolDistance)
        {
            direction *= -1;
            Flip();
        }
    }

    void ChasePlayer()
    {
        float directionToPlayer = player.position.x - transform.position.x;

        if (Mathf.Abs(directionToPlayer) > 0.1f)
        {
            transform.Translate(Vector2.right * Mathf.Sign(directionToPlayer) * chaseSpeed * Time.deltaTime);
            if (Mathf.Sign(directionToPlayer) != direction)
            {
                direction = (int)Mathf.Sign(directionToPlayer);
                Flip();
            }
        }
    }

    void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    void OnDrawGizmosSelected()
    {
        if (attackZone != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(attackZone.position, new Vector2(attackRange, attackHeight));

        }

        Gizmos.color = Color.green;
        Vector3 leftPoint = transform.position + Vector3.left * patrolDistance;
        Vector3 rightPoint = transform.position + Vector3.right * patrolDistance;
        Gizmos.DrawLine(leftPoint, rightPoint);
        Gizmos.DrawSphere(leftPoint, 0.1f);
        Gizmos.DrawSphere(rightPoint, 0.1f);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            direction *= -1;
            Flip();
        }
    }


}
