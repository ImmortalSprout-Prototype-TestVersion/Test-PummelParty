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

    private void Awake()
    {
        // 플레이어 생성하기 -> 이건 각 클라이언트에서 하는 건데 생성되는 거 자체는 연동될 거 같음 (포톤 기능으로)

        // TODO: 마스터한테 스폰 포인트 인덱스 받아오기

        // 플레이어 생성하기 프리팹
        if(PhotonNetwork.IsMasterClient)
        {
            for(int i = 0; i < PhotonNetwork.PlayerList.Length; ++i)
            {
                GameObject newPlayerPrefab = PhotonNetwork.Instantiate("Prefabs/Minigame/BoardgamePlayer", _playerSpawnPosition[i].position, Quaternion.identity);
                newPlayerPrefab.GetPhotonView().TransferOwnership(PhotonNetwork.PlayerList[i]);
                _playerPrefabs.Add(newPlayerPrefab);
            }
        }
    }

    private void Start()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; ++i)
        {
            PhotonView pv = _playerPrefabs[i].GetComponent<PhotonView>();

            if (pv.IsMine)
            {
                _virtualCamera.LookAt = _playerPrefabs[i].transform;
                _virtualCamera.Follow = _playerPrefabs[i].transform;
            }
        }
    }

}
