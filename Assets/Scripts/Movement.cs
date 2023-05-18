using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 8f;

    protected Rigidbody2D rigidbody;
    protected Collider2D collider;
    protected Camera camera;
    protected SpriteRenderer sprite;
    protected Animator anim;

    protected float direction;
    protected float gravity ;
    protected Vector2 velocity;
    protected virtual void Awake()
    {
        camera = Camera.main;
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    protected virtual void OnEnable()
    {
        GetComponent<Rigidbody2D>().isKinematic = false;
        GetComponent<Collider2D>().enabled = true;
        velocity = Vector2.zero;
    }

    protected virtual void OnDisable()
    {
        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<Collider2D>().enabled = false;
        velocity = Vector2.zero;
    }

    protected void ApplyGravity()
    {
        bool falling = velocity.y < 0f || !Input.GetButton("Jump");
        float multiplier = falling ? 2f : 1f;

        velocity.y += gravity * multiplier * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, gravity / 2f);
    }
    protected virtual void FixedUpdate()
    {
        Vector2 position = rigidbody.position;
        position += velocity * Time.fixedDeltaTime;
        if (this.gameObject.tag == "Player")
        {
            Vector2 leftEdge = camera.ScreenToWorldPoint(Vector2.zero);
            Vector2 rightEdge = camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
            position.x = Mathf.Clamp(position.x, leftEdge.x + 0.5f, rightEdge.x - 0.5f);
        }
        rigidbody.MovePosition(position);
    }
    protected virtual void HorizontalMovement()
    {
        velocity.x = Mathf.MoveTowards(velocity.x, direction * moveSpeed, moveSpeed * Time.deltaTime);
    }

    protected virtual void SetAnim()
    {
        if (velocity.x >= 0.5f)
        {
            anim.SetBool("Run", true);
            sprite.flipX = false;
        }
        else if (velocity.x <= -0.5f)
        {
            anim.SetBool("Run", true);
            sprite.flipX = true;
        }
        else
        {
            anim.SetBool("Run", false);
        }
    }

    protected virtual void Update()
    {
        HorizontalMovement();
        ApplyGravity();
        SetAnim();
    }
}