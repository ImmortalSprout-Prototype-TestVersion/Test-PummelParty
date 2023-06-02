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

    private PhotonView PV;
    private TMP_Text roomNameText;
    private Quaternion playerRotate = Quaternion.Euler(0, 180, 0);
    private int playerEnterOther = 1;
    private bool isClickedButton;

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
        // ������ Ŭ���̾�Ʈ ���� ���� �����ϸ鼭 ������ ������ ��򰡿� �����ؼ� ����������� ������ �ʿ䰡 ����
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(models[playerEnterOther].name, spawnPositions[playerEnterOther].position, playerRotate);
        }

        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
    }

    // Ŭ���̾�Ʈ ���� ���� �� Ŭ���̾�Ʈ�� ������ ��򰡿� �����ؼ� ����������� ������ �ʿ䰡 ����
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            playerEnterOther++;
            PhotonNetwork.Instantiate(models[playerEnterOther].name, spawnPositions[playerEnterOther].position, playerRotate);
            PV = buttons[playerEnterOther].GetPhotonView();
            PV.TransferOwnership(newPlayer);
        }
    }

    public void OnClickReadyButton()
    {
        PV = buttons[playerEnterOther].GetPhotonView();

        if (PV.IsMine)
        {
            if (isClickedButton == false)
            {
                PlayerInfo[playerEnterOther].GetComponent<Image>().color = new Color32(38, 255, 0, 255);
                isClickedButton = true;
            }

            else
            {
                PlayerInfo[playerEnterOther].GetComponent<Image>().color = new Color32(111, 111, 111, 255);
                isClickedButton = false;
            }
        }

        else
        {
            return;
        }      
    }
}
