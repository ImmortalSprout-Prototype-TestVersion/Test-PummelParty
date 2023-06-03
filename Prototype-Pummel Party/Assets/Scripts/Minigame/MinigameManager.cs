using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class MinigameManager : MonoBehaviour
{
    [SerializeField] private Transform[] _playerSpawnPosition;
    [SerializeField] CinemachineVirtualCamera _virtualCamera;

    private int _actorNumber;

    private List<(float, int)> _minigameRecord = new List<(float, int)>(PhotonNetwork.PlayerList.Length + 1);
    // private List<int> _
    private int _goalInPlayerCount = 0;

    private void Awake()
    {
        _actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        GameObject newPlayerPrefab = PhotonNetwork.Instantiate("Prefabs/Minigame/BoardgamePlayer", _playerSpawnPosition[_actorNumber - 1].position, Quaternion.identity);
        
        _virtualCamera.LookAt = newPlayerPrefab.transform;
        _virtualCamera.Follow = newPlayerPrefab.transform;
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

            // 게임매니저한테 등수 리스트 만들어서 액터넘버 전달해주기
            gameObject.GetPhotonView().RPC("SendResultToGameManager", RpcTarget.All, _minigameRecord);
            // 보드게임으로 씬전환
        }
    }

    [PunRPC]
    private void SendResultToGameManager(List<(float, int)> minigameRecord)
    {
        for(int i = 0; i < minigameRecord.Count; ++i)
        {
            GameManager.Instance.MinigameResult[i + 1] = minigameRecord[i].Item2;
        }
    }
}
