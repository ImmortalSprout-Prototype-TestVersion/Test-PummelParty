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
using Cysharp.Threading.Tasks;
using System;

public class RoomManager : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private GameObject roomName;
    [SerializeField] private Image[] StatusBar;
    [SerializeField] private Transform[] spawnPositions;
    [SerializeField] private GameObject[] models;
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private Button startButton;

    private PhotonView[] PV;
    private TMP_Text roomNameText;
    private Quaternion playerRotate = Quaternion.Euler(0, 180, 0);
    private int playerEnterOther = 1;
    private bool isClickedButton;

    private Color defaultColor = Color.grey;
    //private Color hostColor = Color.red;
    private Color readyColor = Color.green;

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
        if (this != null)
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
            PV[playerEnterOther].TransferOwnership(newPlayer);
        }
    }

    #region Ready Button �Լ�

    public void OnClickReady1Button()
    {
        if (PV[2].IsMine)
        {
            if (isClickedButton == false)
            {
                //StatusBar[2].GetComponent<Image>().color = new Color32(38, 255, 0, 255);
                isClickedButton = true;
                PhotonView.Get(gameObject).RPC("IncreaseReadyCount", RpcTarget.MasterClient);
                PhotonView.Get(gameObject).RPC("ChangeColor", RpcTarget.All, 2, isClickedButton);
            }

            else
            {
                StatusBar[2].GetComponent<Image>().color = new Color32(111, 111, 111, 255);
                isClickedButton = false;
                PhotonView.Get(gameObject).RPC("DecreaseReadyCount", RpcTarget.MasterClient);
                PhotonView.Get(gameObject).RPC("ChangeColor", RpcTarget.All, 2, isClickedButton);
            }
        }

        else
        {
            return;
        }
    }

    public void OnClickReady2Button()
    {
        if (PV[3].IsMine)
        {
            if (isClickedButton == false)
            {
                //StatusBar[3].GetComponent<Image>().color = new Color32(38, 255, 0, 255);
                isClickedButton = true;
                PhotonView.Get(gameObject).RPC("IncreaseReadyCount", RpcTarget.MasterClient);
                PhotonView.Get(gameObject).RPC("ChangeColor", RpcTarget.All, 3, isClickedButton);
            }

            else
            {
                //StatusBar[3].GetComponent<Image>().color = new Color32(111, 111, 111, 255);
                isClickedButton = false;
                PhotonView.Get(gameObject).RPC("DecreaseReadyCount", RpcTarget.MasterClient);
                PhotonView.Get(gameObject).RPC("ChangeColor", RpcTarget.All, 3, isClickedButton);
            }
        }

        else
        {
            return;
        }
    }

    public void OnClickReady3Button()
    {
        if (PV[4].IsMine)
        {
            if (isClickedButton == false)
            {
                //StatusBar[4].GetComponent<Image>().color = new Color32(38, 255, 0, 255);
                isClickedButton = true;
                PhotonView.Get(gameObject).RPC("IncreaseReadyCount", RpcTarget.MasterClient);
                PhotonView.Get(gameObject).RPC("ChangeColor", RpcTarget.All, 4, isClickedButton);
            }

            else
            {
                //StatusBar[4].GetComponent<Image>().color = new Color32(111, 111, 111, 255);
                isClickedButton = false;
                PhotonView.Get(gameObject).RPC("DecreaseReadyCount", RpcTarget.MasterClient);
                PhotonView.Get(gameObject).RPC("ChangeColor", RpcTarget.All, 4, isClickedButton);
            }
        }

        else
        {
            return;
        }
    }

    #endregion

    public void OnClickStartButton()
    {
        PhotonNetwork.LoadLevel("BoardGame 1");
    }

    public int readyCount = 0; // �̰� �������� �˷�����ҵ�?
                               // RpcTarget.MasterClient �� �ϸ� �ɵ�? ���� �ٸ��ֵ����׵� readyCount �� ������Ʈ���� �ʿ䰡 ����

    //private async UniTaskVoid ShowStartButton1()
    //{
    //    while (true)
    //    {
    //        if (!isInRoom)
    //        {
    //            return;
    //        }

    //        Debug.Log("�濡 ����");
    //        await UniTask.WaitUntil(() => readyCount == 3); // ���� ������ ����
    //                                                        // Start��ư�� Ȱ��ȭ�ϴ� �Լ��� ������
    //        Debug.Log("3�� �Ǿ���");
    //        if (PhotonNetwork.IsMasterClient)
    //        {
    //            Debug.Log("�����̶� ACTIVATE RPC�� �Ѹ�");
    //            PhotonView.Get(gameObject).RPC("ActivateStartButton", RpcTarget.All);
    //        }
    //    }
    //}

    //private async UniTaskVoid HideStartButton1()
    //{
    //    while (true)
    //    {
    //        if (!isInRoom)
    //        {
    //            return;
    //        }

    //        await UniTask.WaitUntil(() => readyCount < 3); // ��ü�� ���� ���� �ʾҴٸ�
    //                                                       // Start ��ư�� ��Ȱ��ȭ�ϴ� �Լ��� ������
    //        if (PhotonNetwork.IsMasterClient)
    //        {
    //            PhotonView.Get(gameObject).RPC("DeActivateStartButton", RpcTarget.All);
    //        }
    //    }
    //}

   
    private void OnActiveStartButton()
    {
        if (readyCount == 3 && PhotonNetwork.IsMasterClient)
        {
            PhotonView.Get(gameObject).RPC("ActivateStartButton", RpcTarget.All);
        }

        else
        {
            PhotonView.Get(gameObject).RPC("DeActivateStartButton", RpcTarget.All);
        }
    }

    [PunRPC]
    private void IncreaseReadyCount()
    {
        ++readyCount;
        OnActiveStartButton();
    }

    [PunRPC]
    private void DecreaseReadyCount()
    {
        --readyCount;
        OnActiveStartButton();
    }

    [PunRPC]
    private void ActivateStartButton()
    {
        startButton.interactable = true;
    }

    [PunRPC]
    private void DeActivateStartButton()
    {
        startButton.interactable = false;
    }

    [PunRPC]
    private void ChangeColor(int number, bool readyResult)
    {
        if (readyResult)
        {
            StatusBar[number].color = readyColor;
        }
        else
        {
            StatusBar[number].color = defaultColor;
        }
    }

    public void OnClickBackButton()
    {
        PhotonNetwork.LoadLevel("Lobby 1");
    }
}
