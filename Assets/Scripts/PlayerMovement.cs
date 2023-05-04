using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : Movement
{

    public float maxJumpHeight = 5f;
    public float maxJumpTime = 1f;
    public float jumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f);
    public bool grounded { get; private set; }
    public bool jumping { get; private set; }
    public bool running => Mathf.Abs(velocity.x) > 0.25f || Mathf.Abs(direction) > 0.25f;
    public bool sliding => (direction > 0f && velocity.x < 0f) || (direction < 0f && velocity.x > 0f);
    public bool falling => velocity.y < 0f && !grounded;

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
        grounded = rigidbody.Raycast(Vector2.down);

        jumping = velocity.y > 1.5f;
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

        if (rigidbody.Raycast(Vector2.right * velocity.x)) {
            velocity.x = 0f;
        }
    }
    private void GroundedMovement()
    {
        velocity.y = Mathf.Max(velocity.y, 0f);
        if (Input.GetButtonDown("Jump"))
        {
            velocity.y = jumpForce;
            jumping = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (transform.DotTest(collision.transform, Vector2.down,45f))
            {
                velocity.y = jumpForce / 2f;
                jumping = true;
            }
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Default")) 
        {
            if (transform.DotTest(collision.transform, Vector2.up,75f))
            {
                velocity.y = 0f;
            }
        }
    }

    protected override void SetAnim()
    {
        base.SetAnim();
        if(jumping == true)
        {
            anim.SetTrigger("Jump");
        }
    }
}
