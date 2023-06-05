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
    private int playerEnterOrder = 1;
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
                stream.SendNext(playerEnterOrder);
            }
            else
            {
                playerEnterOrder = (int)stream.ReceiveNext();
            }
        }
    }

    public override void OnJoinedRoom()
    {
        // 마스터 클라이언트 생성 구간 생성하면서 방장의 정보를 어딘가에 저장해서 보드게임으로 가져갈 필요가 있음
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(models[playerEnterOrder].name, spawnPositions[playerEnterOrder].position, playerRotate);
        }

        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
    }

    // 클라이언트 생성 생성 시 클라이언트의 정보를 어딘가에 저장해서 보드게임으로 가져갈 필요가 있음
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            playerEnterOrder++;
            GameObject newPlayerModel = PhotonNetwork.Instantiate(models[playerEnterOrder].name, spawnPositions[playerEnterOrder].position, playerRotate);
            newPlayerModel.GetPhotonView().TransferOwnership(newPlayer);
            PV[playerEnterOrder].TransferOwnership(newPlayer);
        }
    }

    #region Ready Button 함수

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
        if (PhotonNetwork.IsMasterClient)
        {

            PhotonNetwork.LoadLevel(2);

        }
    }

    public int readyCount = 0; // 이걸 방장한테 알려줘야할듯?
                               // RpcTarget.MasterClient 로 하면 될듯? 굳이 다른애들한테도 readyCount 를 업데이트해줄 필요가 없음

    //private async UniTaskVoid ShowStartButton1()
    //{
    //    while (true)
    //    {
    //        if (!isInRoom)
    //        {
    //            return;
    //        }

    //        Debug.Log("방에 있음");
    //        await UniTask.WaitUntil(() => readyCount == 3); // 방장 본인은 빼줌
    //                                                        // Start버튼을 활성화하는 함수를 실행함
    //        Debug.Log("3이 되었음");
    //        if (PhotonNetwork.IsMasterClient)
    //        {
    //            Debug.Log("방장이라서 ACTIVATE RPC로 뿌림");
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

    //        await UniTask.WaitUntil(() => readyCount < 3); // 전체가 레디를 하지 않았다면
    //                                                       // Start 버튼을 비활성화하는 함수를 실행함
    //        if (PhotonNetwork.IsMasterClient)
    //        {
    //            PhotonView.Get(gameObject).RPC("DeActivateStartButton", RpcTarget.All);
    //        }
    //    }
    //}


    private void OnActiveStartButton()
    {
        // TODO: 테스트용으로 레디 필요한 수 바꿈 -> 커밋 or 머지할 때 꼭 지우기!!!
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
        
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonView.Get(gameObject).RPC("KickOutPlayers", RpcTarget.All);
        }
        else
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    [PunRPC]
    private void KickOutPlayers()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("Lobby");
    }
    
}
