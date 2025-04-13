using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Bắn")]
    [SerializeField ] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float fireRate = 1f;

    [Header("Phát hiện Player")]
    [SerializeField] private float detectionRange = 8f;

    [SerializeField] private float nextFireTime = 0f;
    [SerializeField] private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange && Time.time >= nextFireTime)
        {
            FireAtPlayer();
            nextFireTime = Time.time + 1f / fireRate;
        }

        FlipTowardsPlayer();

    }

    void FireAtPlayer()
    {
        Vector2 direction = (player.position - firePoint.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * bulletSpeed;
    }

    // Vẽ vùng detection trong Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    void FlipTowardsPlayer()
    {
        if (player == null) return;

        Vector3 scale = transform.localScale;

        if (player.position.x < transform.position.x)
        {
            scale.x = Mathf.Abs(scale.x); // Quay sang trái
        }
        else
        {
            scale.x = -Mathf.Abs(scale.x); // Quay sang phải
        }

        transform.localScale = scale;
    }
}

