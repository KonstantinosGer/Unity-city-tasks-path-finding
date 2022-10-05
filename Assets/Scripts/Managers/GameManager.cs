using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState GameState;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ChangeState(GameState.GenerateGrid);
    }

    public void ChangeState(GameState newState)
    {
        GameState = newState;
        switch (newState)
        {
            case GameState.GenerateGrid:
                GridManager.Instance.GenerateGrid();
                break;
            case GameState.SpawnGold:
                UnitManager.Instance.SpawnGold();
                break;
            case GameState.SpawnEnergyPot:
                UnitManager.Instance.SpawnEnergyPot();
                break;
            case GameState.Null:
                break;
            //case GameState.EnemiesTurn:
             //   break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }
}

public enum GameState
{
    GenerateGrid = 0,
    SpawnGold = 1,
    SpawnEnergyPot = 2,
    Null = 3
    //HeroesTurn = 3,
    //EnemiesTurn = 4
}
