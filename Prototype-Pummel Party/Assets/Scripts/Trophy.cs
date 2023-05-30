using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trophy : MonoBehaviour
{
    private int trophyCount;
    private bool canGetTrophy;

    private void Start()
    {
        trophyCount = 0;
        canGetTrophy = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            return;
        }

        if (canGetTrophy)
        {
            trophyCount++;

            Debug.Log("트로피 획득"); // 트로피 획득 로직
        }
        else
        {
            canGetTrophy = true;
            Debug.Log("최초 도착");
        }

        Debug.Log($"획득한 트로피 수 : {trophyCount}");
    }

    /// <summary>
    /// 플레이어가 트로피를 획득할 수 있는 상태로 만들어주는 함수
    /// </summary>
    public void EnableTrophyPickUp() // 플레이어 사망 시 호출할 이벤트 메소드
    {
        // 플레이어는 사망하게 되는 경우 무인도에 갔다가, 특정 주사위값을 얻게되면 탈출하고, 이후 시작타일에서 다시 생성되기 때문이다
        canGetTrophy = false;
    }

    /// <summary>
    /// 플레이어의 트로피 개수를 반환하는 함수
    /// </summary>
    /// <param name="_playerInstanceID"></param>
    /// <returns></returns>
    public int GetTrophyCount(int _playerInstanceID)
    {
        // 멀티 플레이어 포팅 시, 플레이어의 인스턴스 ID와 비교해서 뭘 해줘야할 거 같음
        return trophyCount;
    }
}

    
