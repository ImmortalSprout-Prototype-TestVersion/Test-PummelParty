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

            Debug.Log("Ʈ���� ȹ��"); // Ʈ���� ȹ�� ����
        }
        else
        {
            canGetTrophy = true;
            Debug.Log("���� ����");
        }

        Debug.Log($"ȹ���� Ʈ���� �� : {trophyCount}");
    }

    /// <summary>
    /// �÷��̾ Ʈ���Ǹ� ȹ���� �� �ִ� ���·� ������ִ� �Լ�
    /// </summary>
    public void EnableTrophyPickUp() // �÷��̾� ��� �� ȣ���� �̺�Ʈ �޼ҵ�
    {
        // �÷��̾�� ����ϰ� �Ǵ� ��� ���ε��� ���ٰ�, Ư�� �ֻ������� ��ԵǸ� Ż���ϰ�, ���� ����Ÿ�Ͽ��� �ٽ� �����Ǳ� �����̴�
        canGetTrophy = false;
    }

    /// <summary>
    /// �÷��̾��� Ʈ���� ������ ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <param name="_playerInstanceID"></param>
    /// <returns></returns>
    public int GetTrophyCount(int _playerInstanceID)
    {
        // ��Ƽ �÷��̾� ���� ��, �÷��̾��� �ν��Ͻ� ID�� ���ؼ� �� ������� �� ����
        return trophyCount;
    }
}

    
