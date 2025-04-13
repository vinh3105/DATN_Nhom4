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

	private Rigidbody2D rb;
	private bool isGrounded;
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
			DontDestroyOnLoad(gameObject);

			// Khởi tạo trạng thái ban đầu
			SetState(State.Idle);
		}
	}

	void Update()
	{
		HandleMovement();
		HandleShooting();
		UpdateAnimation(); // Cập nhật animation
	}

	private void HandleMovement()
	{
		float move = Input.GetAxis("Horizontal");
		rb.velocity = new Vector2(move * speed, rb.velocity.y);

		if (move > 0)
			transform.rotation = Quaternion.Euler(0, 0, 0);
		else if (move < 0)
			transform.rotation = Quaternion.Euler(0, 180, 0);

		if (Input.GetButton("Jump") && isGrounded)
		{
			rb.velocity = new Vector2(rb.velocity.x, jumpForce);
		}

		// Xác định trạng thái dựa trên di chuyển và nhảy
		if (!isGrounded)
		{
			SetState(State.Jump);
		}
		else if (move != 0)
		{
			SetState(State.Run);
		}
		else
		{
			SetState(State.Idle);
		}
	}

	private void HandleShooting()
	{
		if (Input.GetKey(KeyCode.R) && Time.time >= nextFireTime)
		{
			Shoot();
			nextFireTime = Time.time + fireRate;
		}
	}

	private void Shoot()
	{
		GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
		PlayerBullet bulletScript = bullet.GetComponent<PlayerBullet>();
		float direction = transform.eulerAngles.y == 0 ? 1 : -1;
		bulletScript.SetDirection(new Vector2(direction, 0));

		if (direction == -1)
		{
			bullet.transform.Rotate(0, 180, 0);
		}
	}

	public void TakeDamage(int damage)
	{
		playerInfo.TakeDamage(damage);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
		{
			isGrounded = true;
		}
		else if (collision.gameObject.CompareTag("Enemy"))
		{
			TakeDamage(1);
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
		{
			isGrounded = false;
		}
	}

	// Hệ thống animation
	private void SetState(State newState)
	{
		if (currentState != newState)
		{
			currentState = newState;
			currentFrame = 0; // Reset frame khi đổi trạng thái
			timer = 0f;

			// Chọn mảng sprite tương ứng
			switch (currentState)
			{
				case State.Idle:
					currentSprites = idleSprites;
					break;
				case State.Run:
					currentSprites = runSprites;
					break;
				case State.Jump:
					currentSprites = null; // Jump sẽ được xử lý đặc biệt
					break;
			}
		}
	}

	private void UpdateAnimation()
	{
		// Xử lý animation cho trạng thái Jump
		if (currentState == State.Jump)
		{
			if (rb.velocity.y > 0) // Đang nhảy lên
			{
				spriteRenderer.sprite = jumpUpSprite; // Hình 1
			}
			else if (rb.velocity.y < 0) // Đang rơi xuống
			{
				spriteRenderer.sprite = jumpFallSprite; // Hình 2
			}
			return; // Không cần chạy animation frame-by-frame cho Jump
		}

		// Xử lý animation cho các trạng thái khác (Idle, Run)
		if (currentSprites == null || currentSprites.Length == 0) return;

		timer += Time.deltaTime;
		if (timer >= frameRate)
		{
			timer = 0f;
			currentFrame = (currentFrame + 1) % currentSprites.Length; // Lặp lại animation
			spriteRenderer.sprite = currentSprites[currentFrame]; // Cập nhật sprite
		}
	}
}