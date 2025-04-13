using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animatiomore : MonoBehaviour
{
	public Sprite[] idleSprites;  // Sprite cho trạng thái đứng yên
	public Sprite[] runSprites;   // Sprite cho trạng thái chạy
	public Sprite[] jumpSprites;  // Sprite cho trạng thái nhảy
	public float frameRate = 0.1f; // Tốc độ chuyển frame (giây mỗi frame)

	private SpriteRenderer spriteRenderer;
	private PlayerMode1 playerMovement; // Tham chiếu đến script di chuyển
	private Sprite[] currentSprites; // Mảng sprite hiện tại
	private int currentFrame = 0;    // Frame hiện tại
	private float timer = 0f;        // Bộ đếm thời gian

	private enum State { Idle, Run, Jump }
	private State currentState;

	void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		playerMovement = GetComponent<PlayerMode1>();
		SetState(State.Idle); // Bắt đầu với trạng thái Idle
	}

	void Update()
	{
		// Xác định trạng thái dựa trên di chuyển và nhảy
		if (playerMovement.IsJumping())
		{
			SetState(State.Jump);
		}
		else if (playerMovement.IsMoving())
		{
			SetState(State.Run);
		}
		else
		{
			SetState(State.Idle);
		}

		// Cập nhật animation
		UpdateAnimation();
	}

	void SetState(State newState)
	{
		if (currentState != newState)
		{
			currentState = newState;
			currentFrame = 0; // Reset frame khi đổi trạng thái

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
					currentSprites = jumpSprites;
					break;
			}
		}
	}

	void UpdateAnimation()
	{
		if (currentSprites == null || currentSprites.Length == 0) return;

		// Tăng timer
		timer += Time.deltaTime;

		// Chuyển frame khi đủ thời gian
		if (timer >= frameRate)
		{
			timer = 0f;
			currentFrame = (currentFrame + 1) % currentSprites.Length; // Lặp lại animation
			spriteRenderer.sprite = currentSprites[currentFrame]; // Cập nhật sprite
		}
	}
}