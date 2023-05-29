using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : Action
{
    public Enemy target;

    public Attack(Enemy target)
    {
        this.target = target;
    }
}
