using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class MinigameManager : MonoBehaviour
{
    [SerializeField] private Transform[] _playerSpawnPosition;
    [SerializeField] CinemachineVirtualCamera _virtualCamera;

    [SerializeField] private PlayerModelData models;
    [SerializeField] private PositionData positions;

    private int _actorNumber;

    private List<(float, int)> _minigameRecord = new List<(float, int)>(PhotonNetwork.PlayerList.Length + 1);
    private int _goalInPlayerCount = 0;

    private void Awake()
    {
        _actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        GameObject newPlayerPrefab = PhotonNetwork.Instantiate("Prefabs/Minigame/BoardgamePlayer", _playerSpawnPosition[_actorNumber - 1].position, Quaternion.identity);
        
        _virtualCamera.LookAt = newPlayerPrefab.transform;
        _virtualCamera.Follow = newPlayerPrefab.transform;

        if(!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        // ������ ����� �Ʒ��� c() �ּ�ó�� �صа� ���� �ϴٰ� Ŀ���ϴ� �� ��Ծ �صаŴϱ� �׳� ����� �غ��� ��!!
        //for(int actorNumber = 1; actorNumber <= PhotonNetwork.CurrentRoom.PlayerCount; ++actorNumber)
        //{

        //}
    }

    //void c()
    //{
    //    if (PhotonNetwork.IsMasterClient)
    //    {
    //        for (int actorNumber = 1; actorNumber < PhotonNetwork.CurrentRoom.PlayerCount + 1; actorNumber++)
    //        {
    //            GameObject player = PhotonNetwork.Instantiate(models.BoardGameModel[actorNumber].name, positions.BoardGameSpawnPosition[actorNumber].transform.position,
    //                Quaternion.identity);
    //            PhotonView pv = player.GetPhotonView();
    //            playerPv[actorNumber] = pv;
    //            pv.TransferOwnership(actorNumber);
    //        }
    //    }

    //    isPlayerAllInstantiated = true;
    //}

    [PunRPC]
    public void Record(float time, int actorNumber)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            _minigameRecord.Add((time, actorNumber));
            _goalInPlayerCount++;

             if(_goalInPlayerCount == PhotonNetwork.PlayerList.Length)
            {
                Rank();
            }
        }
    }

    private void Rank()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            _minigameRecord.Sort();

            for(int i = 0; i < _minigameRecord.Count; i++)
            {
                int actorNumber = _minigameRecord[i].Item2;
                gameObject.GetPhotonView().RPC("SendResultToGameManager", RpcTarget.MasterClient, actorNumber);
            }

            // TODO: ����������� ����ȯ ���� �� �׽�Ʈ�ؾ���
            PhotonNetwork.LoadLevel(2);
        }
    }

    [PunRPC]
    private void SendResultToGameManager(int actorNumber)
    {
        if(PhotonNetwork.IsMasterClient)
           GameManager.Instance.MinigameResult.Add(actorNumber);
    }
}
