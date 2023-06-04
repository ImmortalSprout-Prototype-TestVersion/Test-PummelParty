using Cysharp.Threading.Tasks;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    //public event Action OnTurnStarted;
    //public event Action OnTurnEnd;
    public BoardGameEvent BoardGameframeWork;

    private Player currentTurnPlayer;
    private PhotonView cuttrentTurnView;

    private Turn turn;

    private void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            turn = new Turn();
            turn.Init();
        }
    }

    private void Start()
    {
        //StartPlayerTurn();
        if (PhotonNetwork.IsMasterClient)
        {
            turn.SetOrder(1, 2, 3, 4);

            Debug.Log(turn.turnOrder.Dequeue().ActorNumber);
            BoardGameframeWork.StartTurn.Invoke();
        }
    }
    /// <summary>
    /// ���� ���� �÷��̾� ����
    /// </summary>
    /// <returns></returns>
    public void PlayerSet()
    {
        currentTurnPlayer = turn.Guide();
    }

    public void OnStartTurn()
    {
       cuttrentTurnView = PhotonView.Find(currentTurnPlayer.ActorNumber);
       cuttrentTurnView.gameObject.GetComponent<PlayerController>();


        
    }

    //private void StartPlayerTurn()
    //{
    //    Debug.Log("�� ����");
    //    OnTurnStarted?.Invoke();
    //}

    //public void EndPlayerTurn()
    //{
    //    Debug.Log("�� ����");
    //    OnTurnEnd?.Invoke();
    //}

    //public void EndMinigame()
    //{
    //    Debug.Log("�̴ϰ��� ��");
    //    Invoke("StartPlayerTurn", 1f);
    //}
}
