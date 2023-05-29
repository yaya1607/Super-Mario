using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform transform { get; protected set; }
    public SpriteRenderer sprite { get; private set; }
    public Rigidbody2D rigidbody{ get; protected set; }

    protected virtual void Awake()
    {
        transform = GetComponent<Transform>();
        rigidbody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }
    protected void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }
    public virtual void Trampled(float trampledDirection = 0) { }
}
