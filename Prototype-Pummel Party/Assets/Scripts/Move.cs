using Cinemachine;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private Rigidbody rigid;
    [SerializeField] private float movePower;
    [SerializeField] private MinigameManager _minigameManager;
    PhotonView _photonView;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();

        WaitUntilAllPlayersInstantiated().Forget();
    }

    public void SetMinigameManager(MinigameManager minigameManager)
    {
        _minigameManager = minigameManager;
    }

    private async UniTaskVoid WaitUntilAllPlayersInstantiated()
    {
        await UniTask.Delay(2000); // �ð��ʸ� ���ָ� ����� �絵���� �ѱ������ ī�޶� �־������ ������ ��

        if (_photonView.IsMine)
        {
            // MinigameManager minigameManager = GameObject.Find("MinigameManager").GetComponent<MinigameManager>();
            // TODO: �̴ϰ��� �Ŵ��� ���� �Ŵ��� ������ �ִ� �� ����
            MinigameManager minigameManager = GameManager.Instance.GetComponentInChildren<MinigameManager>();

            minigameManager.SetVirtualCamera(transform);
        }
    }
    

    void Update()
    {
        if(_photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rigid.AddForce(Vector3.forward * movePower * Time.deltaTime, ForceMode.Impulse);
            }
        }
    }
}
