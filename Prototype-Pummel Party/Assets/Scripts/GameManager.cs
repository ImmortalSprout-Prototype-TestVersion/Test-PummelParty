using Cinemachine;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] TurnManager turnManager;



    public void SetVirtualCamera(Transform playerTransform)
    {
        virtualCamera.Follow = playerTransform;
        virtualCamera.LookAt = playerTransform;
    }

    public TurnManager ReturnTurnManager()
    {
        return turnManager;
    }

    private const int boardgameNumber = 0;
    private const int minigameNumber = 1;
    public int playerCount = 4;
    public int playerOrderNumber;
    public PlayerModelData models;
    public PositionData positions;
    public PhotonView[] playerPv;
    private Hashtable playerPreviousPosition;

    public bool isPlayerAllInstantiated;
    public bool isfirstTurn = true;

    public List<int> MinigameResult = new List<int>(5);



    private static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }

            return instance;
        }
    }

    private void Awake()
    {
        playerPv = new PhotonView[playerCount + 1];
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        turnManager = GetComponentsInChildren<TurnManager>()[0];
        virtualCamera = GetComponentsInChildren<CinemachineVirtualCamera>()[0];

        if (isfirstTurn)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }
            SpawnPlayerFirstTime().Forget();
        }
        else
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }
            SpawnPlayerAsync().Forget();
        }
    }


    void Start()
    {
        if (playerPreviousPosition == null)
        {
            playerPreviousPosition = new Hashtable()
            {
                { "MyPreviousPosition", new Vector3()}
            };
        }
    }

    private async UniTaskVoid SpawnPlayerFirstTime()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(2f));
            
        if (PhotonNetwork.IsMasterClient)
        {
            for (int actorNumber = 1; actorNumber < PhotonNetwork.CurrentRoom.PlayerCount + 1; actorNumber++)
            {
                Player currentPlayer = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber);
                currentPlayer.SetCustomProperties(playerPreviousPosition); // 등록을 함
                GameObject player = PhotonNetwork.Instantiate(models.BoardGameModel[actorNumber].name, positions.BoardGameSpawnPosition[actorNumber].transform.position,
                    Quaternion.identity);
                PhotonView pv = player.GetPhotonView();
                playerPv[actorNumber] = pv;
                pv.TransferOwnership(actorNumber);
            }
        }
        
        isPlayerAllInstantiated = true;
    }



    public void SpawnPlayer()
    {
        SpawnPlayerAsync().Forget();
    }

    private async UniTaskVoid SpawnPlayerAsync()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(2f));

        if (PhotonNetwork.IsMasterClient)
        {
            for (int actorNumber = 1; actorNumber < PhotonNetwork.CurrentRoom.PlayerCount + 1; actorNumber++)
            {
                Player photonPlayer = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber);
                GameObject player = PhotonNetwork.Instantiate(models.BoardGameModel[actorNumber].name, (Vector3) photonPlayer.CustomProperties["MyPreviousPosition"],
                    Quaternion.identity);
                PhotonView pv = player.GetPhotonView();
                playerPv[actorNumber] = pv;
                pv.TransferOwnership(actorNumber);
            }
        }

        isPlayerAllInstantiated = true;
    }


}
