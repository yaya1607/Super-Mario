using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : Block
{
    AnimationScript animation;
    SpriteRenderer sprite;
    [SerializeField] public Sprite[] brickBreak;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        animation = GetComponent<AnimationScript>();
    }
    public override void SmallHit()
    {
        //Small Mario just make a bounce.
        base.SmallHit();
    }
    public override void Hit()
    {
        //Big Mario break down the brick.
        base.Hit();
        animation.ChangeAnimation("Break");
        Invoke(nameof(SetActiveFalse),0.3f);
    }

    private void SetActiveFalse()
    {
        this.gameObject.SetActive(false);
    }
}
