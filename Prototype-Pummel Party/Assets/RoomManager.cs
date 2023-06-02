using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;

public class RoomManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public PhotonView photonView;

    [SerializeField] private GameObject roomName;
    [SerializeField] private Transform[] spawnPositions;
    [SerializeField] private GameObject[] models;

    private TMP_Text roomNameText;
    private int playerEnterOther = 1;

    private void Awake()
    {
        roomNameText = roomName.GetComponent<TMP_Text>();       
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(playerEnterOther);
        }
        else
        {
            playerEnterOther = (int)stream.ReceiveNext();
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName} 플레이어 입장");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("입장~");
        Debug.Log(playerEnterOther);
        PhotonNetwork.Instantiate(models[playerEnterOther].name, spawnPositions[playerEnterOther].position, Quaternion.identity);
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        playerEnterOther++;
    }
}
