using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;
using Cysharp.Threading.Tasks;

public class MinigameManager : MonoBehaviour
{
    [SerializeField] private Transform[] _playerSpawnPosition;
    [SerializeField] CinemachineVirtualCamera _virtualCamera;
    [SerializeField] BoardGame result;

    [SerializeField] private PlayerModelData models;
    [SerializeField] private PositionData positions;
    private GameObject[] players;
    private PhotonView[] _photonView;
    

    private int _actorNumber;

    private List<(float, int)> _minigameRecord = new List<(float, int)>(PhotonNetwork.PlayerList.Length + 1);
    private int _goalInPlayerCount = 0;

   
   
    private async UniTaskVoid WaitUntilAllPlayersInstantiated()
    {
        await UniTask.Delay(2000); // �ð��ʸ� ���ָ� ����� �絵���� �ѱ������ ī�޶� �־������ ������ ��

        for (int actorNumber = 1; actorNumber <= PhotonNetwork.CurrentRoom.PlayerCount; actorNumber++)
        {
            if (_photonView[actorNumber].IsMine)
            {
                // MinigameManager minigameManager = GameObject.Find("MinigameManager").GetComponent<MinigameManager>();
                // TODO: �̴ϰ��� �Ŵ��� ���� �Ŵ��� ������ �ִ� �� ����
                SetVirtualCamera(_photonView[actorNumber].transform);
            }
        }

      
    }


    private void Awake()
    {
        //transform.SetParent(GameManager.Instance.transform);
        _photonView = new PhotonView[5];
        result.Init();
        players = new GameObject[PhotonNetwork.CurrentRoom.PlayerCount + 1];

        if(PhotonNetwork.IsMasterClient)
        {
            for (int actorNumber = 1; actorNumber <= PhotonNetwork.CurrentRoom.PlayerCount; ++actorNumber)
            {
                GameObject player = PhotonNetwork.Instantiate(models.MiniGameModel[0].name, positions.RaceSpawnPosition[actorNumber - 1].transform.position, Quaternion.identity);
                _photonView[actorNumber] = player.GetPhotonView();
                _photonView[actorNumber].TransferOwnership(actorNumber);
                players[actorNumber] = player;
            }
        }
    }
    private void Start()
    {
        WaitUntilAllPlayersInstantiated().Forget();
    }

    public void SetVirtualCamera(Transform playerTransform)
    {
        _virtualCamera.Follow = playerTransform;
        _virtualCamera.LookAt = playerTransform;
    }

    

    [PunRPC]
    public void Record(float time, int actorNumber)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            _minigameRecord.Add((time, actorNumber));
            _goalInPlayerCount++;

             if(_goalInPlayerCount == PhotonNetwork.PlayerList.Length)
            {
                Rank();
            }
        }
    }

    private void Rank()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            _minigameRecord.Sort();

            for(int i = 0; i < _minigameRecord.Count; i++)
            {
                int actorNumber = _minigameRecord[i].Item2;
                gameObject.GetPhotonView().RPC("SendResultToGameManager", RpcTarget.MasterClient, actorNumber);
            }

            result.asdf = false;
            // TODO: ����������� ����ȯ ���� �� �׽�Ʈ�ؾ���
            PhotonNetwork.LoadLevel(2);
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    [PunRPC]
    private void SendResultToGameManager(int actorNumber)
    {
        if(PhotonNetwork.IsMasterClient)
           result.result.Enqueue(actorNumber);
    }
}
