using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioMovement : MonoBehaviour
{
    private float distancePerFrame;
    private float previousPosition;

    private bool enableJump;
    public float moveSpeed = 8f;
    public float maxSpeed;
    public float maxJumpHeight = 5f;
    public float maxJumpTime = 1f;
    public LayerMask defaultLayer;

    public float jumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f);
    public float gravity => (-2f * maxJumpHeight) / Mathf.Pow(maxJumpTime / 2f, 2f);
    public Rigidbody2D rigidbody { get; private set; }
    public Transform transform { get; private set; }
    
    public bool jumping { get; private set; }
    public bool grounding => IsGrounded();
    public bool falling => rigidbody.velocity.y < 0;
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    //private void Update()
    //{
    //    HorizontalMovement();

    //    grounded = rigidbody.Raycast(Vector2.down);

    //    if (grounded)
    //    {
    //        GroundedMovement();
    //    }

    //    ApplyGravity();
    //}

    private void FixedUpdate()
    {
        Vector2 position = rigidbody.position;
        //position += velocity * Time.fixedDeltaTime;

        // clamp within the screen bounds
        Vector2 leftEdge = GetComponent<Camera>().ScreenToWorldPoint(Vector2.zero);
        Vector2 rightEdge = GetComponent<Camera>().ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        position.x = Mathf.Clamp(position.x, leftEdge.x + 0.5f, rightEdge.x - 0.5f);

        rigidbody.MovePosition(position);
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.25f, Vector2.down,0.265f, defaultLayer);
        if (hit.collider != null && hit.rigidbody != this.rigidbody)
        {
            Debug.Log("Grounded");
            return true;
        }
        else
        {
            Debug.Log("NotGrounded");
            return false;
        }
    }

    //private void Jump()
    //{
    //    if (Input.GetKey(KeyCode.Space))
    //    {
    //        if ((jumpDistance < maxJumpDistance && jumping == true ) || (jumpDistance < maxJumpDistance && IsGrounded()))
    //        {
    //            jumping = true;
    //            Debug.Log(jumping);
    //            jumpDistance += Time.deltaTime * rigidbody.velocity.y;
                
    //            rigidbody.velocity = new Vector2(rigidbody.velocity.x,jumpForce);
    //        }
            
    //    }
    //    else if (Input.GetKeyUp(KeyCode.Space))
    //    {
    //        jumpDistance = 0;
    //    }
    //}

    private void SetGrav()
    {
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, -jumpForce);

        enableJump = false;
    }

    //private void Movement()
    //{
    //    moveForce = Input.GetAxis("Horizontal") * moveSpeed;
    //}
}
