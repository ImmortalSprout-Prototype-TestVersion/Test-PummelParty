using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BoardGameData : ScriptableObject
{
    public TurnManager turnManager;

    public CinemachineVirtualCamera virtualCamera;
}
