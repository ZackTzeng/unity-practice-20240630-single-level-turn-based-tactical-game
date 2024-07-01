using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatService
{
    public void ResolveAttack(Unit attacker, Unit defender)
    {
        defender.TakeDamage(attacker.GetAttack());
    }
}
