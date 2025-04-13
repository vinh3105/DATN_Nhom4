using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifeTime = 2f;
    private Vector2 direction; // Hướng bay của đạn

    public void SetDirection(Vector2 newDirection) // Hàm thiết lập hướng bay
    {
        direction = newDirection.normalized; // Chuẩn hóa vector để đảm bảo không bị phóng đại
    }

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyController>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}