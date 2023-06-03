using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class MinigameManager : MonoBehaviour
{
    [SerializeField] private Transform[] _playerSpawnPosition;
    [SerializeField] CinemachineVirtualCamera _virtualCamera;

    private List<GameObject> _playerPrefabs = new List<GameObject>();
    private List<int> _playerPrefabId = new List<int>();

    private void Awake()
    {
        //if(PhotonNetwork.IsMasterClient)
        //{
        //    for(int i = 0; i < PhotonNetwork.PlayerList.Length; ++i)
        //    {
        //        GameObject newPlayerPrefab = PhotonNetwork.Instantiate("Prefabs/Minigame/BoardgamePlayer", _playerSpawnPosition[i].position, Quaternion.identity);
        //        newPlayerPrefab.GetPhotonView().TransferOwnership(PhotonNetwork.PlayerList[i]);
        //        _playerPrefabId.Add(newPlayerPrefab.GetPhotonView().ViewID);
        //        PhotonNetwork.PlayerList[i].
        //        // _playerPrefabs.Add(newPlayerPrefab);
        //    }
        //}

        int num = PhotonNetwork.LocalPlayer.ActorNumber;
        GameObject newPlayerPrefab = PhotonNetwork.Instantiate("Prefabs/Minigame/BoardgamePlayer", _playerSpawnPosition[num - 1].position, Quaternion.identity);
        // newPlayerPrefab.GetPhotonView().TransferOwnership(PhotonNetwork.PlayerList[i]);
        _virtualCamera.LookAt = newPlayerPrefab.transform;
        _virtualCamera.Follow = newPlayerPrefab.transform;

    }

    //private void Start()
    //{
    //    for (int i = 0; i < PhotonNetwork.PlayerList.Length; ++i)
    //    {
    //        PhotonView pv = _playerPrefabs[i].GetComponent<PhotonView>();

    //        if (pv.IsMine)
    //        {
    //            _virtualCamera.LookAt = _playerPrefabs[i].transform;
    //            _virtualCamera.Follow = _playerPrefabs[i].transform;
    //        }
    //    }
    //}

    //[]

}
