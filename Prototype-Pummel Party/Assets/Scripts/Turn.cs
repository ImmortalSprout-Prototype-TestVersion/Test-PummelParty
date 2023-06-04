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
    /// Queue �ʱ�ȭ
    /// </summary>
    public void Init()
    {
        turnOrder = new Queue<Player>();
    }

    /// <summary>
    /// ���� ���� ���� ����, �̴ϰ����� ����ǰ� ��������� ���۵ɶ����� ȣ��� �÷��̾��� �� ������ �����ִ� �Լ�
    /// �Ķ���ͷ� �ش�Ǵ� �÷��̾��� ActorNumber �Է�
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
    /// �Ͽ� �ش��ϴ� �÷��̾ ��ȯ�ϴ� �Լ�
    /// ȣ�� �Ǵ� ������ ���� ���� ��������
    /// </summary>
    /// <returns></returns>
    public Player Guide()
    {
        return turnOrder.Dequeue();
    }

}
