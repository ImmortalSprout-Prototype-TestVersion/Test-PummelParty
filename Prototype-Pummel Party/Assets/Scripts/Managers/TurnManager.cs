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
    private PhotonView currentTurnView;
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
        WaitUntilAllPlayerInstantiated().Forget();
        Debug.Log("�� ���۵Ǿ���");
        if (PhotonNetwork.IsMasterClient)
        {
            turn.SetOrder(1, 2, 3, 4);

            BoardGameframeWork.StartTurn.Invoke();
        }
    }

    private async UniTaskVoid WaitUntilAllPlayerInstantiated()
    {
        await UniTask.WaitUntil(() => GameManager.Instance.isPlayerAllInstantiated == true);
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
        currentTurnView = GameManager.Instance.playerPv[currentTurnPlayer.ActorNumber];
        currentController = currentTurnView.gameObject.GetComponent<PlayerController>();
        currentController.gameObject.SetActive(true);
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
