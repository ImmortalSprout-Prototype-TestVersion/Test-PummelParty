using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoardGameEvent : MonoBehaviour
{
    public UnityEvent StartTurn;

    public UnityEvent OnStartTurn;

    public UnityEvent EndTurn;

    public UnityEvent OnEndTurn;
}
