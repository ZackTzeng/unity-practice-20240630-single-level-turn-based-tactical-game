using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public event Action PrerequisiteStateEntered;

    public static TurnManager Instance { get; private set; }
    public TurnManager CurrentTurnState { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}

public enum TurnState
{
    Prerequisite,
    PlayerTurn,
    EnemyTurn,
    PlayerWin,
    EnemyWin
}