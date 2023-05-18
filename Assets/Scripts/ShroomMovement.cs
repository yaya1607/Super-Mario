using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroomMovement : Movement
{
    public float initialDirection;
    public SpriteRenderer sprite { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        this.enabled = true;
        sprite = GetComponent<SpriteRenderer>();
        direction = initialDirection;
    }
    private void OnBecameVisible()
    {
        this.enabled = true;
    }
    private void OnBecameInvisible()
    {
        this.enabled = false;
    }
    private void Update()
    {
        gravity = (-2f * 2) / Mathf.Pow(1 / 2f, 2f);
        HorizontalMovement();
        ApplyGravity();
    }

    // Wall Bounce
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            if (rigidbody.Raycast(new Vector2(direction, 0)))
            {
                sprite.flipX = !sprite.flipX;
                direction = -direction;
                velocity.x = -velocity.x;
            }
        }
    }
}
