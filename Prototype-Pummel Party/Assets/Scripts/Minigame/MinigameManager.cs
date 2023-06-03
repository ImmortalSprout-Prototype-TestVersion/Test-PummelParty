using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MinigameManager : MonoBehaviour
{
    private void Awake()
    {
        // 플레이어 생성하기 -> 이건 각 클라이언트에서 하는 건데 생성되는 거 자체는 연동될 거 같음 (포톤 기능으로)

        // TODO: 마스터한테 스폰 포인트 인덱스 받아오기

        // 플레이어 생성하기 프리팹
        PhotonNetwork.Instantiate("Prefabs/Minigame/BoardgamePlayer", transform.position, Quaternion.identity);
    }
}
