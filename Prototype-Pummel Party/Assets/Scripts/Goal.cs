using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;

public class Goal : MonoBehaviourPunCallbacks
{
    [SerializeField] MinigameManager _minigameManager;

    private float time;
    private bool stopTimer;

    private void Update()
    {
        if (stopTimer == false)
        {
            time += Time.deltaTime;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
         /*--------------------------------------------------------
         * 무언가의 통계처리?
         * 멀티라면 등수 확보
         * 확보한 등수를 보드게임 신, 미니게임 결과 신으로 전달
         * 
         * 보드게임 로드 신
         */
            stopTimer = true;
            Debug.Log("와 ! 도착 1등 보드게임으로 돌아가요");
            Debug.Log($"걸린 시간 : {time}");
            // Time.timeScale = 0f;
            int playerActorNum = other.gameObject.GetPhotonView().Owner.ActorNumber;

            _minigameManager.gameObject.GetPhotonView().RPC("Record", RpcTarget.MasterClient, time, playerActorNum);
        }
    }
}
