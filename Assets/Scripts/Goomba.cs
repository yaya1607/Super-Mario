using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : Enemy
{
    protected override void Trampled()
    {
        base.Trampled();
        Dead();
    }
}
