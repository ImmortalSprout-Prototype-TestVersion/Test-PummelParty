using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trophy : MonoBehaviour
{
    private int count;
    private bool start;

    private void Start()
    {
        count = 0;
        start = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            return;
        }

        if (start)
        {
            count++;

            Debug.Log("트로피 획득"); // 트로피 획득 로직
        }
        else
        {
            start = true;
            Debug.Log("최초 도착");
        }

        Debug.Log($"획득한 트로피 수 : {count}");
    }

    public void SetStart() // 플레이어 사망 시 호출할 이벤트 메소드
    {
        start = false;
    }
}

    
