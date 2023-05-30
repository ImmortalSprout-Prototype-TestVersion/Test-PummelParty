using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private const string gameVersion = "0.0.1";

    private const string lobbyName = "Immortal Sprouts";
    public string NickName { get; set; } // �ϴ� ����Ѱ�

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion= gameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("������ ������ �Ǿ����!");

        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
            PhotonNetwork.CurrentLobby.Name= lobbyName;
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"���� ���ῡ �����Ͽ����ϴ�... / ���� = {cause}");
        PhotonNetwork.ConnectUsingSettings(); // ���� ���� ��õ�
    }

    public override void OnJoinedLobby()
    {
        Debug.Log($"{PhotonNetwork.CurrentLobby.Name} �κ� ������ �Ǿ����!");
    }

    public override void OnLeftLobby()
    {
        Debug.Log($"{PhotonNetwork.CurrentLobby.Name} �κ� �������!");
    }

    
}