using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MinigameManager : MonoBehaviour
{
    [SerializeField] private Transform[] _playerSpawnPosition;

    private void Awake()
    {
        // �÷��̾� �����ϱ� -> �̰� �� Ŭ���̾�Ʈ���� �ϴ� �ǵ� �����Ǵ� �� ��ü�� ������ �� ���� (���� �������)

        // TODO: ���������� ���� ����Ʈ �ε��� �޾ƿ���

        // �÷��̾� �����ϱ� ������
        if(PhotonNetwork.IsMasterClient)
        {
            for(int i = 0; i < PhotonNetwork.PlayerList.Length; ++i)
            {
                GameObject newPlayerPrefab = PhotonNetwork.Instantiate("Prefabs/Minigame/BoardgamePlayer", _playerSpawnPosition[i].position, Quaternion.identity);
                newPlayerPrefab.GetPhotonView().TransferOwnership(PhotonNetwork.PlayerList[i]);
            }
        }
    }

}
