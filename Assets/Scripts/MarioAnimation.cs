using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioAnimation : MonoBehaviour
{
    private SpriteRenderer sprite;
    private Animator anim;
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    public void SetJumpAnimation()
    {
        anim.SetTrigger("Jump");
    }
    public void SetRunLeftAnimation()
    {
        anim.SetBool("Run",true);
        sprite.flipX = true;
    }
    public void SetRunRightAnimation()
    {
        anim.SetBool("Run", true);
        sprite.flipX = false;
    }
    public void SetStandAnimation()
    {
        anim.SetBool("Run", false);
    }

}
