using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private float time;

    private void Update()
    {
        time += Time.deltaTime;   
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        /*--------------------------------------------------------
         * ������ ���ó��?
         * ��Ƽ��� ��� Ȯ��
         * Ȯ���� ����� ������� ��, �̴ϰ��� ��� ������ ����
         * 
         * ������� �ε� ��
        */

        Debug.Log("�� ! ���� 1�� ����������� ���ư���");
        Debug.Log($"�ɸ� �ð� : {time}");
        Time.timeScale = 0f;
    }
}
