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
         * ������ ���ó��?
         * ��Ƽ��� ��� Ȯ��
         * Ȯ���� ����� ������� ��, �̴ϰ��� ��� ������ ����
         * 
         * ������� �ε� ��
         */
            stopTimer = true;
            Debug.Log("�� ! ���� 1�� ����������� ���ư���");
            Debug.Log($"�ɸ� �ð� : {time}");
            // Time.timeScale = 0f;
            int playerActorNum = other.gameObject.GetPhotonView().Owner.ActorNumber;

            _minigameManager.gameObject.GetPhotonView().RPC("Record", RpcTarget.MasterClient, time, playerActorNum);
        }
    }
}
