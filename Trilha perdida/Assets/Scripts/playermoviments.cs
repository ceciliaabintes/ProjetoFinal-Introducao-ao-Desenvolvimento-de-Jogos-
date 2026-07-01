using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoviments : MonoBehaviour
{
    [Header("Movement Setup")]
    public float speed = 5f;
    public float speedrun = 8f;
    public Rigidbody2D MyPlayer;
    private float _currentspeed;
    private bool direction = true;

    [Header("Jump Setup")]
    public int ForceJump = 12; 
    public Collider2D playerCollider;
    public float spaceToground = 0.1f;
    private float DistToGround;

    private void Awake()
    {
        if (playerCollider != null)
        {
            DistToGround = playerCollider.bounds.extents.y;
        }
    }

    private void Update()
    {
        HandleJump();
    }

    private void FixedUpdate()
    {
        HandleMoviments();
    }

    private bool isGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, DistToGround + spaceToground);
    }

    private void HandleMoviments()
    {
        if (Input.GetKey(KeyCode.LeftControl))
            _currentspeed = speed;
        else
            _currentspeed = speedrun;

        float moveInput = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            moveInput = -1f;
            if (direction)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                direction = false;
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveInput = 1f;
            if (direction == false)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                direction = true;
            }
        }
        MyPlayer.linearVelocity = new Vector2(moveInput * _currentspeed, MyPlayer.linearVelocity.y);
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
        {
            MyPlayer.linearVelocity = new Vector2(MyPlayer.linearVelocity.x, Vector2.up.y * ForceJump);
        }
    }
}