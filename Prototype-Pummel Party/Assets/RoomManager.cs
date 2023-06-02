using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using Photon.Utilities;

public class RoomManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public PhotonView photonView;

    [SerializeField] private GameObject roomName;
    [SerializeField] private Transform[] spawnPositions;
    [SerializeField] private GameObject[] models;
    [SerializeField] private GameObject[] buttons;

    private GameObject playerMeterial;
    private TMP_Text roomNameText;
    private int playerEnterOther = 1;
    private Quaternion playerRotate = Quaternion.Euler(0, 180, 0);

    private void Awake()
    {
        roomNameText = roomName.GetComponent<TMP_Text>();
        photonView = buttons[1].GetPhotonView();
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
        // 마스터 클라이언트 생성 구간 생성하면서 방장의 정보를 어딘가에 저장해서 보드게임으로 가져갈 필요가 있음
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(models[playerEnterOther].name, spawnPositions[playerEnterOther].position, playerRotate);
            playerEnterOther++;

            photonView.RequestOwnership();
        }

        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
    }

    // 클라이언트 생성 생성 시 클라이언트의 정보를 어딘가에 저장해서 보드게임으로 가져갈 필요가 있음
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(models[playerEnterOther].name, spawnPositions[playerEnterOther].position, playerRotate);
            playerEnterOther++;
        }

        //if ()
        //{
            
        //}
    }
}
