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
    private GameObject[] players;

    private int _actorNumber;

    private List<(float, int)> _minigameRecord = new List<(float, int)>(PhotonNetwork.PlayerList.Length + 1);
    private int _goalInPlayerCount = 0;

    private void Awake()
    {
        transform.SetParent(GameManager.Instance.transform);

        players = new GameObject[PhotonNetwork.CurrentRoom.PlayerCount + 1];

        if(PhotonNetwork.IsMasterClient)
        {
            for (int actorNumber = 1; actorNumber <= PhotonNetwork.CurrentRoom.PlayerCount; ++actorNumber)
            {
                GameObject player = PhotonNetwork.Instantiate(models.MiniGameModel[0].name, positions.RaceSpawnPosition[actorNumber - 1].transform.position, Quaternion.identity);
                player.GetPhotonView().TransferOwnership(actorNumber);
                players[actorNumber] = player;
            }
        }
    }


    public void SetVirtualCamera(Transform playerTransform)
    {
        _virtualCamera.Follow = playerTransform;
        _virtualCamera.LookAt = playerTransform;
    }

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

            // TODO: 보드게임으로 씬전환 연결 후 테스트해야함
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
