using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class TurnManager : MonoBehaviour
{
    //public event Action OnTurnStarted;
    //public event Action OnTurnEnd;
    public BoardGameEvent BoardGameframeWork;

    //private Player currentTurnPlayer;
    public Player currentTurnPlayer;
    private PhotonView currentPlayerPhotonView;
    public PlayerController currentController; // 테스트용 에디터에서 볼려구
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
        WaitUntilAllPlayerInstantiated().Forget();
        //Debug.Log("턴 시작되쓰요");
        //if (PhotonNetwork.IsMasterClient)
        //{
        //    turn.SetOrder(1, 2, 3, 4);

        //    BoardGameframeWork.StartTurn.Invoke();
        //}
    }

    private async UniTaskVoid WaitUntilAllPlayerInstantiated()
    {
        await UniTask.WaitUntil(() => GameManager.Instance.isPlayerAllInstantiated == true);
        Debug.Log("턴 시작되쓰요");
        if (PhotonNetwork.IsMasterClient)
        {
            turn.SetPlayerOrder(1, 2, 3, 4);

            BoardGameframeWork.OnStartTurn.Invoke();
        }
    }

    [PunRPC]
    public void InvokePlayerTurnEndEventRPC()
    {
        BoardGameframeWork.OnEndTurn.Invoke();
    }

    public void InvokePlayerTurnEndEvent()
    {
        BoardGameframeWork.OnEndTurn.Invoke();
    }

    public void InvokeOnBackToBoardGame()
    {
        BoardGameframeWork.OnBackToBoardGame.Invoke();
    }


    /// <summary>
    /// 현재 턴인 플레이어 설정
    /// </summary>
    /// <returns></returns>
    public void PlayerSet()
    {
        if (turn.turnOrder.Count == 0)
        {
            BoardGameframeWork.OnAllEndTurn.Invoke();
        }
        else
        {
            currentTurnPlayer = turn.GetCurrentPlayer();
        }
    }

    public void OnStartTurn()
    {
        currentPlayerPhotonView = GameManager.Instance.playerPv[currentTurnPlayer.ActorNumber];
        currentController = currentPlayerPhotonView.gameObject.GetComponent<PlayerController>();
        currentPlayerPhotonView.RPC("EnablePlayerMove", currentTurnPlayer);
    }


    public void LoadMinigameScene()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int actorNumber = 1; actorNumber < PhotonNetwork.CurrentRoom.PlayerCount + 1 ;++actorNumber)
            {
                Player currentPlayer = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber);
                currentPlayer.CustomProperties["MyPreviousPosition"] = GameManager.Instance.playerPv[actorNumber].gameObject.transform.position;
            }
            PhotonNetwork.LoadLevel(3);
        }
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
