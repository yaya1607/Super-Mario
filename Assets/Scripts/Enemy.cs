using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Sprite deadSprite;
    public Transform transform { get; protected set; }
    public SpriteRenderer sprite { get; private set; }
    public Rigidbody2D rigidbody{ get; protected set; }

    protected virtual void Awake()
    {
        transform = GetComponent<Transform>();
        rigidbody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (transform.DotTest(collision.transform, Vector2.up,60f))
            {
                Trampled();
            }
            else
            {
                GameObject.FindGameObjectWithTag("Player").SetActive(false);
            }
        }
    }
    private void SetActive()
    {
        gameObject.SetActive(false);
    }
    protected virtual void Trampled() { }

    protected virtual void Dead()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GetComponent<AnimationScript>().ChangeAnimation("Dead");
        Physics2D.IgnoreCollision(player.GetComponent<Rigidbody2D>().GetComponent<Collider2D>(),rigidbody.GetComponent<Collider2D>(),true);
        Invoke(nameof(SetActive),1f);
    }
}
