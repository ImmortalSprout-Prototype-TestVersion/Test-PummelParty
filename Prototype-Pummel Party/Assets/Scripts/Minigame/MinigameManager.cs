using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;
using HashTable = ExitGames.Client.Photon.Hashtable; 


public class MinigameManager : MonoBehaviour
{
    [SerializeField] private Transform[] _playerSpawnPosition;
    [SerializeField] CinemachineVirtualCamera _virtualCamera;

    private int _actorNumber;

    private List<(float, int)> _minigameRecord = new List<(float, int)>(PhotonNetwork.PlayerList.Length + 1);
    private int _goalInPlayerCount = 0;

    private HashTable playerRanking;

    private void Awake()
    {
        _actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        GameObject newPlayerPrefab = PhotonNetwork.Instantiate("Prefabs/Minigame/BoardgamePlayer", _playerSpawnPosition[_actorNumber - 1].position, Quaternion.identity);
        
        _virtualCamera.LookAt = newPlayerPrefab.transform;
        _virtualCamera.Follow = newPlayerPrefab.transform;

        //if (playerRanking == null)
        //{
        //    playerRanking = new HashTable() { { "Ranking", 0 } };
        //}

        PhotonNetwork.LocalPlayer.SetCustomProperties(new HashTable() { { "Rankiing", 1 } });

        // PhotonNetwork.LocalPlayer.SetCustomProperties(playerRanking);

        playerRanking = PhotonNetwork.LocalPlayer.CustomProperties;

        Debug.Log(playerRanking["Rankiing"]);
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

            for(int i = 1; i < _minigameRecord.Count + 1; ++i)
            {
                int ranking = i;
                int actorNumber = _minigameRecord[i].Item2;

                if (PhotonNetwork.CurrentRoom.GetPlayer(actorNumber).CustomProperties.ContainsKey("Ranking"))
                {
                    playerRanking["Ranking"] = i;
                }

                // gameObject.GetPhotonView().RPC("SendResultToGameManager", RpcTarget.MasterClient, actorNumber);

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
