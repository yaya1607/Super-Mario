using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : Movement
{

    public float maxJumpHeight = 5f;
    public float maxJumpTime = 1f;
    public float jumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f);

    public Sprite smallSprite;
    public Sprite bigSprite;
    public bool grounded { get; private set; }
    public bool jumping { get; private set; }
    public bool dead { get; private set; }
    public bool running => (Mathf.Abs(velocity.x) > 0.15f || Mathf.Abs(direction) > 0.15f) && !dead;
    public bool sliding => ((direction > 0f && velocity.x < 0f) || (direction < 0f && velocity.x > 0f)) && !dead;
    public bool falling => velocity.y < 0f && !grounded && !dead;
    public bool big => this.gameObject.tag == "BigPlayer";


    protected override void Awake()
    {
        base.Awake();
        dead = false;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        dead = false;
        jumping = false;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        dead = false;
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
            //Jump after trampled onto the enemy. 
            if (transform.DotTest(collision.transform, Vector2.down,75f))
            {
                velocity.y = jumpForce / 2f;
                //Give Koopa the direction that Mario has trampled to make a spin in the opposite direction.
                float trampledDirection = transform.position.x - collision.transform.position.x >= 0 ? 1 : -1;
                collision.gameObject.GetComponent<Enemy>().Trampled(trampledDirection);
            }
            else
            {
                dead = true;
                FallOut();
            }
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Default"))
        {   //If you jump and hit the ceiling Mario will fall
            if (transform.DotTest(collision.transform, Vector2.up,45f))
            {
                velocity.y = 0f;
            }
            //Function to handle hitting the mystery block
            if(collision.gameObject.tag != "EmptyBlock")
            {
                if (transform.DotTest(collision.transform, Vector2.up, 45f) && jumping)
                {
                    if(collision.gameObject.TryGetComponent<Block>(out Block block))
                    {
                        if (big)
                        {
                            block.Hit();
                        }
                        else
                        {
                            block.SmallHit();
                        }
                    }
                    
                }
            }
        }
        //Eat Shroom
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Shroom"))
        {
            collision.gameObject.SetActive(false);
            if (big)
            {

            }
            else
            {
                StartCoroutine(PowerUpCoroutine());
                this.gameObject.tag = "BigPlayer";
                collider.size = new Vector2(0.75f, 2f);
                collider.offset = new Vector2(0f, 0.5f);
            }
        }
    }


    private IEnumerator PowerUpCoroutine()
    {
        float duration = 1f, elapsed = 0f;
        animation.enabled = false;
        Time.timeScale = 0.0f;
        while (elapsed <= duration)
        {
            if ((elapsed / 0.125f) % 2 == 0) sprite.sprite = smallSprite;
            else sprite.sprite = bigSprite;
            elapsed += 0.125f;
            if (elapsed != duration)
                yield return new WaitForSecondsRealtime(0.125f);
        }
        Time.timeScale = 1.0f;
        animation.enabled = true;
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
            newPosition.z = fallOutPosition.z;
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
}
