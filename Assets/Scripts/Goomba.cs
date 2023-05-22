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
    protected override void Trampled()
    {
        base.Trampled();
        movement.enabled = false;
        Dead();
    }
}
