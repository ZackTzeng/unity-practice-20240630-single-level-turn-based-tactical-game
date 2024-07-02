using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit Scriptable Object", menuName = "Unit Scriptable Object")]
public class UnitSO : ScriptableObject
{
    [field: SerializeField] public Sprite IdleSprite { get; private set; }
    [field: SerializeField] public Sprite ActiveSprite { get; private set; }
    [field: SerializeField] public int Range { get; private set; }
    [field: SerializeField] public int Attack { get; private set; }
    [field: SerializeField] public int AttackRange { get; private set; }
    [field: SerializeField] public int MaxHealth { get; private set; }
}
