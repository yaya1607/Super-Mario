using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : Enemy
{
    public GoombaMovement movement { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        movement = GetComponent<GoombaMovement>();
    }
    public override void Trampled(float trampledDirection = 0)
    {
        base.Trampled();
        movement.enabled = false;
        Flat();
    }


    protected void Flat()
    {
        GetComponent<AnimationScript>().ChangeAnimation("Dead");
        GameObject player = GameObject.Find("Mario");
        Physics2D.IgnoreCollision(player.GetComponent<Rigidbody2D>().GetComponent<Collider2D>(), rigidbody.GetComponent<Collider2D>(), true);
        Invoke(nameof(SetActiveFalse), 1f);
    }
}
