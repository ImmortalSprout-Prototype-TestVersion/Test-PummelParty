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
    private PlayerController currentController;
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

            BoardGameframeWork.StartTurn.Invoke();
        }
    }
    /// <summary>
    /// 현재 턴인 플레이어 설정
    /// </summary>
    /// <returns></returns>
    public void PlayerSet()
    {
        currentTurnPlayer = turn.Guide();
    }

    public void OnStartTurn()
    {
       cuttrentTurnView = GameManager.Instance.playerPv[currentTurnPlayer.ActorNumber];
       currentController = cuttrentTurnView.gameObject.GetComponent<PlayerController>();
       currentController.gameObject.SetActive(true);
    }

    
    //private void StartPlayerTurn()
    //{
    //    Debug.Log("턴 시작");
    //    OnTurnStarted?.Invoke();
    //}

    //public void EndPlayerTurn()
    //{
    //    Debug.Log("턴 종료");
    //    OnTurnEnd?.Invoke();
    //}

    //public void EndMinigame()
    //{
    //    Debug.Log("미니게임 끝");
    //    Invoke("StartPlayerTurn", 1f);
    //}
}
