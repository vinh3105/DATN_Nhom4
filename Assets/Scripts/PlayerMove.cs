using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jump = 10f;
    [SerializeField] private bool isGround = true;
    [SerializeField] private int count_jump = 2;
    [SerializeField] private int maxJump;
    // Update is called once per frame

    private void Start()
    {
        maxJump = count_jump;
    }
    void Update()
    {
        float move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2 (move * speed, rb.velocity.y);

        if(Input.GetKeyDown(KeyCode.Space) && maxJump>0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jump);
            isGround = false;
            maxJump -= 1;
        }
         
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")){
            isGround = true;
            maxJump = count_jump;
        }
    }

   


}
