using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public event Action OnTurnStarted;
    public event Action OnTurnEnd;

    private void Start()
    {
        StartPlayerTurn();
    }

    private void StartPlayerTurn()
    {
        Debug.Log("턴 시작");
        OnTurnStarted?.Invoke();
    }

    public void EndPlayerTurn()
    {
        Debug.Log("턴 종료");
        OnTurnEnd?.Invoke();
    }

    public void EndMinigame()
    {
        Debug.Log("미니게임 끝");
        Invoke("StartPlayerTurn", 1f);
    }
}
