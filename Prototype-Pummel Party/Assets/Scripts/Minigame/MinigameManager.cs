using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MinigameManager : MonoBehaviour
{
    private void Awake()
    {
        // �÷��̾� �����ϱ� -> �̰� �� Ŭ���̾�Ʈ���� �ϴ� �ǵ� �����Ǵ� �� ��ü�� ������ �� ���� (���� �������)

        // TODO: ���������� ���� ����Ʈ �ε��� �޾ƿ���

        // �÷��̾� �����ϱ� ������
        PhotonNetwork.Instantiate("Prefabs/Minigame/BoardgamePlayer", transform.position, Quaternion.identity);
    }
}
