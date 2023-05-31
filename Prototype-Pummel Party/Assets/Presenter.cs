using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;

public class Presenter : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject roomName;
    [SerializeField] private Transform[] spawnPositions;
    [SerializeField] private GameObject[] models;

    private bool[] isOccupying = new bool[PhotonNetwork.CurrentRoom.MaxPlayers + 1]; 
    private TextMeshPro roomNameText;
    private int playerEnterOther = 2;

    private void Awake()
    {
        roomNameText = roomName.GetComponent<TextMeshPro>();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("µé¾î¿È");
        PhotonNetwork.Instantiate("InRoomPlayerBlue", spawnPositions[playerEnterOther].position, Quaternion.identity);
        isOccupying[playerEnterOther] = true;
        playerEnterOther++;
    }



    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        
    }


}
