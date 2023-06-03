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

public class RoomManager : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private GameObject roomName;
    [SerializeField] private GameObject[] PlayerInfo;
    [SerializeField] private Transform[] spawnPositions;
    [SerializeField] private GameObject[] models;
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private Button startButton;

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

    private void Start()
    {
        ShowStartButton().Forget();
        HideStartButton().Forget();
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

    #region Ready Button 함수

    public void OnClickReady1Button()
    {
        if (PV[2].IsMine)
        {
            if (isClickedButton == false)
            {
                PlayerInfo[2].GetComponent<Image>().color = new Color32(38, 255, 0, 255);
                isClickedButton = true;
                PV[playerEnterOther].RPC("IncreaseReadyCount", RpcTarget.MasterClient);
            }

            else
            {
                PlayerInfo[2].GetComponent<Image>().color = new Color32(111, 111, 111, 255);
                isClickedButton = false;
                PV[playerEnterOther].RPC("DecreaseReadyCount", RpcTarget.MasterClient);
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
                PlayerInfo[3].GetComponent<Image>().color = new Color32(38, 255, 0, 255);
                isClickedButton = true;
                PV[playerEnterOther].RPC("IncreaseReadyCount", RpcTarget.MasterClient);
            }

            else
            {
                PlayerInfo[3].GetComponent<Image>().color = new Color32(111, 111, 111, 255);
                isClickedButton = false;
                PV[playerEnterOther].RPC("DecreaseReadyCount", RpcTarget.MasterClient);
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
                PlayerInfo[4].GetComponent<Image>().color = new Color32(38, 255, 0, 255);
                isClickedButton = true;
                PV[playerEnterOther].RPC("IncreaseReadyCount", RpcTarget.MasterClient);
            }

            else
            {
                PlayerInfo[4].GetComponent<Image>().color = new Color32(111, 111, 111, 255);
                isClickedButton = false;
                PV[playerEnterOther].RPC("DecreaseReadyCount", RpcTarget.MasterClient);
            }
        }

        else
        {
            return;
        }
    }

    #endregion

    private int readyCount = 0; // 이걸 방장한테 알려줘야할듯?
                                // RpcTarget.MasterClient 로 하면 될듯? 굳이 다른애들한테도 readyCount 를 업데이트해줄 필요가 없음

    private async UniTaskVoid ShowStartButton()
    {
        await UniTask.WaitUntil(() => readyCount == 3);
        // Start버튼을 활성화하는 함수를 실행함
        if (PhotonNetwork.IsMasterClient)
        {
            gameObject.GetPhotonView().RPC("ActivateStartButton", RpcTarget.All);
        }
    }

    private async UniTaskVoid HideStartButton()
    {
        await UniTask.WaitUntil(() => readyCount < 3);
        // Start 버튼을 비활성화하는 함수를 실행함
        if (PhotonNetwork.IsMasterClient)
        {
            gameObject.GetPhotonView().RPC("DeActivateStartButton", RpcTarget.All);
        }
    }

    // 1) 레디 버튼이 눌렸을 때 눌린 레디 버튼 개수를 1개 증가시킨다
    // 2) 레디 버튼을 뗐을 때는 레디 버튼 개수를 1개 감소시킨다

    // 3) 눌린 레디 버튼이 3개가 된다면 4개의 클라이언트의 start버튼을 모두 활성화시킨다
    // 4) 눌린 레디 버튼이 3개 미만이라면 모든 클라이언트의 start버튼을 비활성화시킨다

    [PunRPC]
    private void IncreaseReadyCount()
    {
        ++readyCount;
    }

    [PunRPC]
    private void DecreaseReadyCount()
    {
        --readyCount;
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

}
