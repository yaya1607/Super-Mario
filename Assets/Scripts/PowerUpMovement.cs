using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpMovement : Movement
{
    public float initialDirection;
    public SpriteRenderer sprite { get; private set; }
    private bool canCollide => this.GetComponent<PowerUp>().canCollide;

    protected override void Awake()
    {
        base.Awake();
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
    protected void OnEnable()
    {
        base.OnEnable();
        direction = initialDirection;
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
        if (collision.gameObject.layer == LayerMask.NameToLayer("Default") && canCollide)
        {
            if (rigidbody.Raycast(new Vector2(direction, 0)))
            {
                sprite.flipX = !sprite.flipX;
                direction = -direction;
                velocity.x = -velocity.x;
                Debug.Log(direction);
            }
        }
    }
}
