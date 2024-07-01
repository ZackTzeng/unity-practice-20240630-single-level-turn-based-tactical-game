using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatService
{
    public void ResolveAttack(Unit attacker, Unit defender)
    {
        Debug.Log($"CombatService: ResolveAttack(): {attacker.name} attacks {defender.name}!");
        defender.TakeDamage(attacker.GetAttack());
    }
}
