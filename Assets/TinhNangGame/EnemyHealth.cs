using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int health = 1; // Máu của enemy

    // Hàm giảm máu khi nhận sát thương
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    // Hàm xử lý khi enemy chết
    private void Die()
    {
        // Có thể thêm hiệu ứng chết, âm thanh, rơi vật phẩm ở đây
        Destroy(gameObject); // Phá hủy đối tượng khi chết
    }
}
