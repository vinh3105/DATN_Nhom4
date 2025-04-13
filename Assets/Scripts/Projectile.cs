using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 5f; // Sau 5 giây, tự xóa viên đạn

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Gọi hàm nhận sát thương nếu có hệ thống máu
            // other.GetComponent<PlayerHealth>().TakeDamage(damage);
            Debug.Log("Player trúng đạn!");
            Destroy(gameObject);
        }
        else if (!other.isTrigger) // Xóa viên đạn khi chạm vào vật thể
        {
            Destroy(gameObject);
        }
    }
}