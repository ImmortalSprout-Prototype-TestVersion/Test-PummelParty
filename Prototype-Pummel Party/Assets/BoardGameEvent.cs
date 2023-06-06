using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoardGameEvent : MonoBehaviour
{
    public UnityEvent OnStartTurn;

    public UnityEvent OnEndTurn;

    public UnityEvent OnAllEndTurn;

    public UnityEvent OnBackToBoardGame;
}
