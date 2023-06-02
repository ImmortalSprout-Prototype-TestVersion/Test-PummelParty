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
        // ������ Ŭ���̾�Ʈ ���� ���� �����ϸ鼭 ������ ������ ��򰡿� �����ؼ� ����������� ������ �ʿ䰡 ����
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(models[playerEnterOther].name, spawnPositions[playerEnterOther].position, playerRotate);
            playerEnterOther++;

            photonView.RequestOwnership();
        }

        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
    }

    // Ŭ���̾�Ʈ ���� ���� �� Ŭ���̾�Ʈ�� ������ ��򰡿� �����ؼ� ����������� ������ �ʿ䰡 ����
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
