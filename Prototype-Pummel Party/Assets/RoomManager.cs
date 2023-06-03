using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using Photon.Utilities;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.UI;

public class RoomManager : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private GameObject roomName;
    [SerializeField] private GameObject[] PlayerInfo;
    [SerializeField] private Transform[] spawnPositions;
    [SerializeField] private GameObject[] models;
    [SerializeField] private GameObject[] buttons;

    private PhotonView[] PV;
    private TMP_Text roomNameText;
    private Quaternion playerRotate = Quaternion.Euler(0, 180, 0);
    private int playerEnterOther = 1;
    private bool isClickedButton;

    private void Awake()
    {
        roomNameText = roomName.GetComponent<TMP_Text>();
        PV = new PhotonView[5];

        for (int i = 1; i < 5; ++i)
        {
            PV[i] = buttons[i].GetPhotonView();
        }
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
        }

        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
    }

    // 클라이언트 생성 생성 시 클라이언트의 정보를 어딘가에 저장해서 보드게임으로 가져갈 필요가 있음
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            playerEnterOther++;
            PhotonNetwork.Instantiate(models[playerEnterOther].name, spawnPositions[playerEnterOther].position, playerRotate);
            PV[playerEnterOther].TransferOwnership(newPlayer);
        }
    }

    public void OnClickReadyButton()
    {
        if (PV[playerEnterOther].IsMine)
        {
            if (isClickedButton == false)
            {
                PlayerInfo[playerEnterOther].GetComponent<Image>().color = new Color32(38, 255, 0, 255);
                isClickedButton = true;
                Debug.Log("내꺼 켜짐");
            }

            else
            {
                PlayerInfo[playerEnterOther].GetComponent<Image>().color = new Color32(111, 111, 111, 255);
                isClickedButton = false;
                Debug.Log("내꺼 꺼짐");
            }
        }

        else
        {
            return;
        }      
    }
}
