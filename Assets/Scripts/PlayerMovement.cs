using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : Movement
{

    public float maxJumpHeight = 5f;
    public float maxJumpTime = 1f;
    public float jumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f);
    public bool grounded { get; private set; }
    public bool jumping { get; private set; }
    public bool dead { get; private set; }
    public bool big { get; private set; }
    public bool immune { get; private set; }
    public bool star => gameObject.tag == "StarPlayer";
    public bool running => (Mathf.Abs(velocity.x) > 0f || Mathf.Abs(direction) > 0.05f) && !dead;
    public bool sliding => ((direction > 0f && velocity.x < 0f) || (direction < 0f && velocity.x > 0f)) && !dead;
    public bool falling => velocity.y < 0f && !grounded && !dead;


    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        jumping = false;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        jumping = false;
    }
    protected override void Update()
    {
        gravity = (-2f * maxJumpHeight) / Mathf.Pow(maxJumpTime / 2f, 2f);
        grounded = rigidbody.Raycast(Vector2.down) && !dead;
        jumping = !grounded && !falling && !dead;
        if (grounded)
        {
            GroundedMovement();
        }
        base.Update();
    }
    protected override void HorizontalMovement()
    {
        direction = Input.GetAxis("Horizontal");
        base.HorizontalMovement();

        if (direction == 0 && (velocity.x < 1 && velocity.x > -1))
            velocity.x = 0;

        
    }
    private void GroundedMovement()
    {
        velocity.y = Mathf.Max(velocity.y, 0f);
        if (Input.GetButtonDown("Jump"))
        {
            velocity.y = jumpForce;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (rigidbody.UndergroundPipeInCheck())
            {
                StartCoroutine(EnterPipeCoroutine());
            }
        }
    }
    protected override void SetAnim()
    {
        // Set animation
        if (!dead)
        {
            if (!big)
            {
                if (grounded)
                {
                    if (running && direction > 0 && !sliding)
                    {
                        animation.ChangeAnimation("Run");
                        sprite.flipX = false;
                    }
                    else if (running && direction < 0 && !sliding)
                    {
                        animation.ChangeAnimation("Run");
                        sprite.flipX = true;
                    }
                    else if (sliding)
                    {
                        animation.ChangeAnimation("Slide");
                    }
                    else
                    {
                        animation.ChangeAnimation("Idle");
                    }
                }
                else
                {
                    animation.ChangeAnimation("Jump");
                }
            }
            else
            {
                if (grounded)
                {
                    if (running && direction > 0 && !sliding)
                    {
                        animation.ChangeAnimation("Big_Run");
                        sprite.flipX = false;
                    }
                    else if (running && direction < 0 && !sliding)
                    {
                        animation.ChangeAnimation("Big_Run");
                        sprite.flipX = true;
                    }
                    else if (sliding)
                    {
                        animation.ChangeAnimation("Big_Slide");
                    }
                    else
                    {
                        animation.ChangeAnimation("Big_Idle");
                    }
                }
                else
                {
                    animation.ChangeAnimation("Big_Jump");
                }
            }
        }
        else
        {
            animation.ChangeAnimation("Fall");
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") || collision.gameObject.layer == LayerMask.NameToLayer("SpinningShell"))
        {
            if (!immune && !star)
            {
                //Jump after trampled onto the enemy. 
                if (transform.DotTest(collision.transform, Vector2.down, 65f))
                {
                    velocity.y = jumpForce / 2f;
                }
            }
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Default"))
        {   //If you jump and hit the ceiling Mario will fall
            if (transform.DotTest(collision.transform, Vector2.up, big ? 25f : 45f))
            {
                velocity.y = 0f;
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Default"))
        {   
            if (collision.gameObject.CompareTag("UndergroundPipeOut"))
            {
                if (rigidbody.UndergroundPipeOutCheck(direction < 0 ? Vector2.left : direction > 0 ? Vector2.right : Vector2.zero))
                {
                    StartCoroutine(ExitPipeCoroutine());
                }
            }
        }
       
    }
    private IEnumerator EnterPipeCoroutine()
    {
        float durationPerUnit = 0.5f, elapsed = 0f, duration;
        Time.timeScale = 0;
        collider.enabled = false;
        Vector3 downPosition = new Vector3(this.transform.position.x, this.transform.position.y - 5f, this.transform.position.z),
            currentPosition = transform.position;
        duration = durationPerUnit * (currentPosition.y - downPosition.y);//Calculate the distance to have exact duration.
        while (elapsed <= duration)
        {
            Vector3 newPosition = Vector3.Lerp(currentPosition, downPosition, elapsed / duration);
            newPosition.z = downPosition.z;
            this.transform.position = newPosition;
            elapsed += 0.02f;
            yield return new WaitForSecondsRealtime(0.02f);
        }
        Underground(true);
    }
    private IEnumerator ExitPipeCoroutine()
    {
        float durationPerUnit = 0.5f, elapsed = 0f, duration;
        Time.timeScale = 0;
        collider.enabled = false;
        //Enter the pipe
        Vector3 innerPosition = new Vector3(this.transform.position.x + 1f * direction, this.transform.position.y, this.transform.position.z),
            currentPosition = transform.position;
        duration = durationPerUnit * Mathf.Abs(currentPosition.x - innerPosition.x);//Calculate the distance to have exact duration.
        while (elapsed <= duration)
        {
            Vector3 newPosition = Vector3.Lerp(currentPosition, innerPosition, elapsed / duration);
            newPosition.z = innerPosition.z;
            this.transform.position = newPosition;
            elapsed += 0.02f;
            yield return new WaitForSecondsRealtime(0.02f);
        }

        Underground(false);
        elapsed = 0f;

        //Exit the pipe
        currentPosition = new Vector3(transform.position.x + 2.25f, transform.position.y + 15.5f,transform.position.z);
        Vector3 outterPosition = new Vector3(transform.position.x + 2.25f, transform.position.y + 19f, transform.position.z);
        duration = durationPerUnit * Mathf.Abs(currentPosition.y - outterPosition.y);
        while (elapsed <= duration)
        {
            Vector3 newPosition = Vector3.Lerp(currentPosition, outterPosition, elapsed / duration);
            newPosition.z = outterPosition.z;
            this.transform.position = newPosition;
            elapsed += 0.02f;
            yield return new WaitForSecondsRealtime(0.02f);
        }
        Time.timeScale = 1f;
        collider.enabled = true;
    }
    private void Underground(bool underground)
    {
        if(underground == true)
        {
            Time.timeScale = 1f;
            collider.enabled = true;
            transform.position = new Vector3(GameManager.Instance.undergroundPositionX - 7f, GameManager.Instance.undergroundPositionY + 6f, transform.position.z);
            camera.GetComponent<SideScrolling>().SetUnderground(true);
        }
        else
        {
            camera.GetComponent<SideScrolling>().SetUnderground(false);
        }
        
    }

    protected override IEnumerator FallOutCoroutine()
    {
        float durationPerUnit = 0.15f, elapsed = 0f, duration;
        Time.timeScale = 0;
        collider.enabled = false;
        Vector3 bouncedPosition = new Vector3(this.transform.position.x, this.transform.position.y + 2f, this.transform.position.z),
            fallOutPosition = new Vector3(this.transform.position.x, this.transform.position.y - 20f, this.transform.position.z),
            currentPosition = transform.position;
        duration = durationPerUnit * (bouncedPosition.y - currentPosition.y);//Calculate the distance to have exact duration.
        while (elapsed <= duration)
        {
            Vector3 newPosition = Vector3.Lerp(currentPosition, bouncedPosition, elapsed / duration);
            newPosition.z = bouncedPosition.z;
            this.transform.position = newPosition;
            elapsed += 0.01f;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        elapsed = 0;
        duration = durationPerUnit * (bouncedPosition.y - fallOutPosition.y);
        while (elapsed <= duration)
        {
            Vector3 newPosition = Vector3.Lerp(bouncedPosition, fallOutPosition, elapsed / duration);
            newPosition.z = fallOutPosition.z;
            this.transform.position = newPosition;
            elapsed += 0.01f;
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }
    public void SetDead(bool isDead)
    {
        dead = isDead;
    }
    public void SetBig(bool isBig)
    {
        big = isBig;
    }
    public void SetImmune(bool isImmune)
    {
        immune = isImmune;
    }
}
