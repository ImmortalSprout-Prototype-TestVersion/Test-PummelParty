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
    [SerializeField] private TurnManager _turnManager;
    [SerializeField] private CinemachineVirtualCamera virtualCam;

    public void SetVirtualCamera(Transform playerTransform)
    {
        virtualCam.Follow = playerTransform;
        virtualCam.LookAt = playerTransform;
    }

    public TurnManager ReturnTurnManager()
    {
        return _turnManager;
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


        if (PhotonNetwork.IsMasterClient)
        {
            SpawnPlayerFirstTime().Forget();
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

    //private void LoadMinigameScene()
    //{
    //    //SceneManager.LoadScene(minigameNumber, LoadSceneMode.Additive);
    //    //Invoke("LoadBoardgameScene", 2f);

    //    LoadMiniGameScene().Forget();
    //}


    private async UniTaskVoid LoadMiniGameScene()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1f)); // 1초 기다렸다가
        SceneManager.LoadScene(minigameNumber, LoadSceneMode.Additive); // 미니게임씬을 additive 모드로 겹쳐서 로드한다
        await UniTask.Delay(TimeSpan.FromSeconds(1f)); // 1초 기다렸다가
        SceneManager.UnloadScene(minigameNumber); // 미니게임씬을 다시 Unload 한다
       // _turnManager.EndMinigame();  // 미니게임씬 unload 후 턴매니저에게 알림
    }

    private void LoadBoardgameScene()
    {
        // TODO: 보드게임으로 다시 넘어오는 로직 수정됐을 때 _turnManager.EndMinigame() 관련 다시 테스트해보기
        // 작성한 시기에는 LoadMinigame -> 1초 후 unload 방식이었어서 적어둠
     //   _turnManager.EndMinigame();
        SceneManager.UnloadScene(minigameNumber);
    }

    private async UniTaskVoid SpawnPlayerFirstTime()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(2f));
            
        if (PhotonNetwork.IsMasterClient)
        {
            for (int actorNumber = 1; actorNumber < PhotonNetwork.CurrentRoom.PlayerCount + 1; actorNumber++)
            {
                PhotonNetwork.CurrentRoom.GetPlayer(actorNumber).SetCustomProperties(playerPreviousPosition); // 등록을 함
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
