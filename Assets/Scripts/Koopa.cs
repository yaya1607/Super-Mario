using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koopa : Enemy
{
    public KoopaMovement movement { get; private set; }
    public CapsuleCollider2D collider { get; private set; }
    public bool isShell { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        movement = GetComponent<KoopaMovement>();
        collider = GetComponent<CapsuleCollider2D>();
        isShell = false;
    }
    public override void Trampled(float trampledDirection = 0)
    {
        base.Trampled();
        //If the Koopa was not in the shell => Set it to cover in the shell.
        if (!isShell)
        {
            movement.moveSpeed = 0;
            movement.SetVelocity(new Vector2(0,0));
            isShell = true;
            collider.size = new Vector2(1f, 1f);
            collider.offset = new Vector2(0, 0);
        }

        //If the Koopa was in the shell => Make it spin.
        else
        {
            gameObject.layer = LayerMask.NameToLayer("SpinningShell");
            movement.SetDirection(-trampledDirection);//Take a direction the Mario has trampled in the shell to make a spin in the opposite way.
            movement.moveSpeed = 10;
            movement.SetVelocity(new Vector2(10* -trampledDirection, 0));
        }
    }
}
