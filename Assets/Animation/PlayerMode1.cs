using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMode1 : MonoBehaviour
{
	public float moveSpeed = 5f; // Tốc độ di chuyển
	public float jumpForce = 5f; // Lực nhảy
	public Transform groundCheck; // Vị trí kiểm tra mặt đất
	public float checkRadius = 0.2f; // Bán kính kiểm tra mặt đất
	public LayerMask groundLayer; // Layer của mặt đất

	private Rigidbody2D rb;
	private bool isGrounded; // Kiểm tra nhân vật có đang chạm đất không
	private float moveInput; // Input di chuyển (trái/phải)
	private bool facingRight = true; // Hướng nhân vật (mặc định quay phải)

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		// Kiểm tra nhân vật có chạm đất không
		isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

		// Di chuyển trái/phải
		moveInput = Input.GetAxisRaw("Horizontal");
		rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

		// Nhảy
		if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
		{
			rb.velocity = new Vector2(rb.velocity.x, jumpForce);
		}

		// Lật nhân vật
		if (moveInput > 0 && !facingRight)
		{
			Flip();
		}
		else if (moveInput < 0 && facingRight)
		{
			Flip();
		}
	}

	void Flip()
	{
		facingRight = !facingRight;
		transform.Rotate(0f, 180f, 0f); // Lật nhân vật theo trục Y
	}

	// Getter để script animation biết trạng thái
	public bool IsMoving()
	{
		return moveInput != 0;
	}

	public bool IsJumping()
	{
		return !isGrounded;
	}
}
