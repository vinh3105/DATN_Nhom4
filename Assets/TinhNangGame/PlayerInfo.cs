using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;
    [SerializeField] private List<Image> healthIcons;
    private SpriteRenderer spriteRenderer; // Để nhấp nháy sprite
    private bool isInvincible = false; // Kiểm tra trạng thái bất tử

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible || currentHealth <= 0) return; // Ngăn chặn sát thương nếu đang bất tử hoặc không còn máu

        StartCoroutine(HandleDamage(damage));
    }

    private IEnumerator HandleDamage(int damage)
    {
        // Nhấp nháy ngay lập tức
        StartCoroutine(BlinkSprite());

        // Giảm sức khỏe
        currentHealth -= damage;

        // Cập nhật UI ngay lập tức
        UpdateHealthUI();

        // Nếu sức khỏe giảm xuống 0, bắt đầu bất tử
        if (currentHealth <= 0)
        {
            currentHealth = 0; // Đảm bảo không âm
            isInvincible = true; // Thiết lập trạng thái bất tử
            yield return new WaitForSeconds(3f); // Đợi 3 giây
            GameManager.Instance.GameOver(); // Kết thúc trò chơi nếu sức khỏe bằng 0
        }
        else
        {
            isInvincible = true; // Thiết lập trạng thái bất tử
            yield return new WaitForSeconds(3f); // Đợi 3 giây
            isInvincible = false; // Kết thúc trạng thái bất tử
        }
    }

    private IEnumerator BlinkSprite()
    {
        float blinkDuration = 3f; // Thời gian nhấp nháy
        float blinkInterval = 0.2f; // Thời gian giữa các lần nhấp nháy
        float elapsedTime = 0f;

        while (elapsedTime < blinkDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled; // Đổi trạng thái hiển thị
            yield return new WaitForSeconds(blinkInterval);
            elapsedTime += blinkInterval;
        }

        spriteRenderer.enabled = true; // Đảm bảo sprite hiển thị lại
    }

    private void UpdateHealthUI()
    {
        for (int i = 0; i < healthIcons.Count; i++)
        {
            healthIcons[i].enabled = i < currentHealth; // Cập nhật icon hiển thị theo lượng máu
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}