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
    [SerializeField] private GameObject model;

    private GameObject playerMeterial;
    private TMP_Text roomNameText;
    private int playerEnterOther = 1;
    private Quaternion playerRotate = Quaternion.Euler(0, 180, 0);

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

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(model.name, spawnPositions[playerEnterOther].position, playerRotate);
            playerEnterOther++;
        }

        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(model.name, spawnPositions[playerEnterOther].position, playerRotate);
            playerEnterOther++;
        }
    }
      




}
