using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private const string gameVersion = "0.0.1";

    private const string lobbyName = "Immortal Sprouts";
    public string NickName { get; set; } // 일단 열어둘게

    private int repeatTime = 1;
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion= gameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("서버에 연결이 되었어요!");

        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"서버 연결에 실패하였습니다... / 원인 = {cause}");
        PhotonNetwork.ConnectUsingSettings(); // 서버 연결 재시도
    }

    public override void OnJoinedLobby()
    {
        Debug.Log($"{PhotonNetwork.CurrentLobby.Name} 로비에 연결이 되었어요!");
        if (0 < repeatTime)
        {
            repeatTime -= 1;
            PhotonNetwork.LeaveLobby();
        }
        PhotonNetwork.CurrentLobby.Name= lobbyName;
    }

    public override void OnLeftLobby()
    {
        Debug.Log($"{PhotonNetwork.CurrentLobby.Name} 로비를 떠났어요!");
        PhotonNetwork.JoinLobby();
    }


    
}
