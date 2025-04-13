using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private Transform player;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject enemyBulletPrefab;
    [SerializeField] private int health = 1;

    void Start()
    {
        InvokeRepeating("Shoot", 1f, 1f);
    }

    void Update()
    {
        if (player != null && Vector2.Distance(transform.position, player.position) < detectionRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

    void Shoot()
    {
        if (player == null || enemyBulletPrefab == null || firePoint == null)
            return;

        GameObject bullet = Instantiate(enemyBulletPrefab, firePoint.position, Quaternion.identity);
        EnemyBullet bulletScript = bullet.GetComponent<EnemyBullet>();

        if (bulletScript != null)
        {
            Vector2 direction = (player.position - firePoint.position).normalized;
            bulletScript.SetDirection(direction);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
