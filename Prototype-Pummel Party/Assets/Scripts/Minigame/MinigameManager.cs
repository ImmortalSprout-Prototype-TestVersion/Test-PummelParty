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

    private List<int> _ranking = new List<int>();
    private void Rank()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            _minigameRecord.Sort();

            for(int i = 0; i < _minigameRecord.Count; i++)
            {
                int ranking = i;
                int actorNumber = _minigameRecord[i].Item2;
                gameObject.GetPhotonView().RPC("SendResultToGameManager", RpcTarget.All, ranking, actorNumber);
            }
            // 보드게임으로 씬전환
        }
    }

    [PunRPC]
    private void SendResultToGameManager(int ranking, int actorNumber)
    {
         GameManager.Instance.MinigameResult.Add(actorNumber);
    }
}
