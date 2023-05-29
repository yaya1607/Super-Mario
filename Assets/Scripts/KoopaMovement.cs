using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoopaMovement : Movement
{
    public float initialDirection;
    public bool isShell => this.gameObject.GetComponent<Koopa>().isShell;

    protected override void Awake()
    {
        base.Awake();
        this.enabled = false;
        direction = initialDirection;
        gravity = (-2f * 2) / Mathf.Pow(1 / 2f, 2f);
    }

    private void OnBecameVisible()
    {
        this.enabled = true;
    }
    private void OnBecameInvisible()
    {
        this.enabled = false;
    }
    protected override void Update()
    {
        base.Update();
    }

    // Wall Bounce
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            if (rigidbody.Raycast(new Vector2(direction, 0)))
            {
                direction = -direction;
                velocity.x = -velocity.x;
            }
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("SpinningShell"))
        {
            FallOut();
        }
    }
    protected override void SetAnim()
    {
        if (isShell)
        {
            animation.ChangeAnimation("Shell");
        }
        else if (velocity.x > 0)
        {
            animation.ChangeAnimation("Walk");
            sprite.flipX = true;
        }
        else
        {
            animation.ChangeAnimation("Walk");
            sprite.flipX = false;
        }
    }
    protected override IEnumerator FallOutCoroutine()
    {
        float durationPerUnit = 0.13f, elapsed = 0f, duration;
        collider.enabled = false;
        sprite.flipY = true;
        Vector3 bouncedPosition = new Vector3(this.transform.position.x, this.transform.position.y + 1f, this.transform.position.z),
            fallOutPosition = new Vector3(this.transform.position.x, this.transform.position.y - 20f, this.transform.position.z),
            currentPosition = transform.position;
        duration = durationPerUnit * (bouncedPosition.y - currentPosition.y);//Calculate the distance to have exact duration.
        while (elapsed <= duration)
        {
            Vector3 newPosition = Vector3.Lerp(currentPosition, bouncedPosition, elapsed / duration);
            newPosition.z = fallOutPosition.z;
            this.transform.position = newPosition;
            elapsed += Time.deltaTime;
            yield return null;
        }
        elapsed = 0;
        duration = durationPerUnit * (bouncedPosition.y - fallOutPosition.y);
        while (elapsed <= duration)
        {
            Vector3 newPosition = Vector3.Lerp(bouncedPosition, fallOutPosition, elapsed / duration);
            newPosition.z = fallOutPosition.z;
            this.transform.position = newPosition;
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}