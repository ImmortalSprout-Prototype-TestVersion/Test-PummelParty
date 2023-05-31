using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public PhotonView photonView;

    [SerializeField] private GameObject roomName;
    [SerializeField] private Transform[] spawnPositions;
    [SerializeField] private GameObject[] models;

    private bool[] isOccupying = new bool[PhotonNetwork.CurrentRoom.MaxPlayers + 1]; 
    private TMP_Text roomNameText;
    private int playerEnterOther = 1;

    private void Awake()
    {
        roomNameText = roomName.GetComponent<TMP_Text>();
    }


    public override void OnEnable()
    {
        Debug.Log(playerEnterOther);
        PhotonNetwork.Instantiate(models[playerEnterOther].name, spawnPositions[playerEnterOther].position, Quaternion.identity);
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
    }

    public override void OnJoinedRoom()
    {
        photonView.RPC("PlusOrderNumber", RpcTarget.All);
    }


    [PunRPC]
    void PlusOrderNumber()
    {
        playerEnterOther++;
        Debug.Log("»£√‚");

    }

}
