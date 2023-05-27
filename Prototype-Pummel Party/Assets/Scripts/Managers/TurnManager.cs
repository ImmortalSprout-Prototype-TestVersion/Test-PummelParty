using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public event Action OnTurnStarted;
    public event Action OnContinueMove;
    public event Action OnTurnEnd;

    private void Start()
    {
        StartPlayerTurn();
    }

    private void StartPlayerTurn()
    {
        Debug.Log("�� ����");
        OnTurnStarted?.Invoke();
    }

    public void EndPlayerTurn()
    {
        Debug.Log("�� ����");
        OnTurnEnd?.Invoke();

        StartMinigame();
    }

    private void StartMinigame()
    {
        Debug.Log("�̴ϰ��� ����");
        Invoke("EndMinigame", 3f);
    }

    private void EndMinigame()
    {
        Debug.Log("�̴ϰ��� ��");
        Invoke("StartPlayerTurn", 1f);
    }
}
