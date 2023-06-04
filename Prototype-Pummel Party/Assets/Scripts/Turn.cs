using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Turn
{
    public Queue<Player> turnOrder;
    private Player player;

    /// <summary>
    /// Queue 초기화
    /// </summary>
    public void Init()
    {
        turnOrder = new Queue<Player>();
    }

    /// <summary>
    /// 보드 게임 최초 시작, 미니게임이 종료되고 보드게임이 시작될때마다 호출될 플레이어의 턴 순서를 정해주는 함수
    /// 파라미터로 해당되는 플레이어의 ActorNumber 입력
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <param name="third"></param>
    /// <param name="fourth"></param>
    public void SetOrder(int first, int second, int third, int fourth) 
    {
        turnOrder.Enqueue(PhotonNetwork.CurrentRoom.GetPlayer(first));
        turnOrder.Enqueue(PhotonNetwork.CurrentRoom.GetPlayer(second));
        turnOrder.Enqueue(PhotonNetwork.CurrentRoom.GetPlayer(third));
        turnOrder.Enqueue(PhotonNetwork.CurrentRoom.GetPlayer(fourth));
    }

    /// <summary>
    /// 턴에 해당하는 플레이어를 반환하는 함수
    /// 호출 되는 순서에 따라 턴이 정해진다
    /// </summary>
    /// <returns></returns>
    public Player Guide()
    {
        return turnOrder.Dequeue();
    }

}
