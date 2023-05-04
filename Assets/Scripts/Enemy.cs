using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform transform { get; protected set; }
    public Animator anim { get; protected set; }
    public Rigidbody2D rigidbody{ get; protected set; }

    protected virtual void Awake()
    {
        transform = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
    }
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (transform.DotTest(collision.transform, Vector2.up,50f))
            {
                Trampled();
            }
            else
            {

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
        anim.SetTrigger("Dead");
        Physics2D.IgnoreCollision(player.GetComponent<Rigidbody2D>().GetComponent<Collider2D>(),rigidbody.GetComponent<Collider2D>(),true);
        Invoke(nameof(SetActive),1f);
    }
}
