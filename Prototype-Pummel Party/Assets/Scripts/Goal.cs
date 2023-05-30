using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Goal : MonoBehaviour
{
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
            Time.timeScale = 0f;
        }


    }
}
