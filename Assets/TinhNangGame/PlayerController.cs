using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireRate = 0.2f;

    // Thêm các mảng sprite cho animation
    [SerializeField] private Sprite[] idleSprites;  // Sprite cho trạng thái đứng yên
    [SerializeField] private Sprite[] runSprites;   // Sprite cho trạng thái chạy
    [SerializeField] private Sprite jumpUpSprite;   // Hình 1: Nhảy lên
    [SerializeField] private Sprite jumpFallSprite; // Hình 2: Rơi xuống
    [SerializeField] private float frameRate = 0.1f; // Tốc độ chuyển frame (giây mỗi frame)

    // Biến nhảy từ PlayerMove
    [SerializeField] private bool isGround = true;
    [SerializeField] private int count_jump = 2;
    [SerializeField] private int maxJump;

    private Rigidbody2D rb;
    private float nextFireTime = 0f;
    private SpriteRenderer spriteRenderer;
    private PlayerInfo playerInfo;

    // Biến cho animation
    private enum State { Idle, Run, Jump }
    private State currentState;
    private Sprite[] currentSprites; // Mảng sprite hiện tại
    private int currentFrame = 0;    // Frame hiện tại
    private float timer = 0f;        // Bộ đếm thời gian

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            Destroy(gameObject);
        }
        else
        {
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            playerInfo = GetComponent<PlayerInfo>();
            maxJump = count_jump;
        }
    }

    void Update()
    {
        // Di chuyển ngang
        float move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(move * speed, rb.velocity.y);

        // Nhảy
        if (Input.GetKeyDown(KeyCode.Space) && maxJump > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGround = false;
            maxJump -= 1;
        }

        // Bắn đạn - Thay đổi phím bắn từ Z sang R
        if (Input.GetKey(KeyCode.R) && Time.time > nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }

        // Cập nhật animation
        UpdateAnimationState(move);
        UpdateAnimation();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
            maxJump = count_jump;
        }
    }

    private void UpdateAnimationState(float move)
    {
        if (!isGround)
        {
            currentState = State.Jump;
            spriteRenderer.sprite = rb.velocity.y > 0 ? jumpUpSprite : jumpFallSprite;
        }
        else if (Mathf.Abs(move) > 0.1f)
        {
            currentState = State.Run;
            currentSprites = runSprites;
        }
        else
        {
            currentState = State.Idle;
            currentSprites = idleSprites;
        }

        // Xử lý flip X của sprite dựa trên hướng di chuyển
        if (move > 0.1f)
        {
            spriteRenderer.flipX = false; // Quay mặt phải
        }
        else if (move < -0.1f)
        {
            spriteRenderer.flipX = true;  // Quay mặt trái
        }
    }

    void UpdateAnimation()
    {
        if (currentSprites == null || currentSprites.Length == 0) return;

        timer += Time.deltaTime;

        if (frameRate <= 0) return; // Chặn lỗi chia 0 tại đây

        if (timer >= frameRate)
        {
            timer = 0f;
            currentFrame = (currentFrame + 1) % currentSprites.Length;
            spriteRenderer.sprite = currentSprites[currentFrame];
        }
    }

    void Shoot()
    {
        if (firePoint == null || bulletPrefab == null)
        {
            return;
        }

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();

        if (rbBullet != null)
        {
            // Kiểm tra phím W hoặc S để xác định hướng bắn
            Vector2 direction = Vector2.zero;

            if (Input.GetKey(KeyCode.W))
            {
                direction = Vector2.up;  // Hướng lên trời
            }
            else if (Input.GetKey(KeyCode.S))
            {
                direction = Vector2.down;  // Hướng xuống đất
            }
            else if (spriteRenderer.flipX) // Nếu nhân vật đang quay trái, bắn sang trái
            {
                direction = Vector2.left;
            }
            else // Nếu không phải, bắn sang phải
            {
                direction = Vector2.right;
            }

            rbBullet.velocity = direction * 10f; // Đặt vận tốc cho viên đạn (10f là tốc độ)
        }
    }

    public void TakeDamage(int damage)
    {
        playerInfo.TakeDamage(damage);
    }
}
