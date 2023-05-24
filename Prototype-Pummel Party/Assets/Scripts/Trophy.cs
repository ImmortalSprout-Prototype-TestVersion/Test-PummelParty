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

            Debug.Log("Ʈ���� ȹ��"); // Ʈ���� ȹ�� ����
        }
        else
        {
            start = true;
            Debug.Log("���� ����");
        }

        Debug.Log($"ȹ���� Ʈ���� �� : {count}");
    }

    public void SetStart() // �÷��̾� ��� �� ȣ���� �̺�Ʈ �޼ҵ�
    {
        start = false;
    }
}

    
