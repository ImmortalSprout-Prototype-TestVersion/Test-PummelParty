using Cinemachine;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public bool isPlayerAllInstantiated;

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
    }

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnPlayer().Forget();
        }

        //_turnManager.OnTurnEnd -= LoadMinigameScene;
        //_turnManager.OnTurnEnd += LoadMinigameScene;
    }

    private void LoadMinigameScene()
    {
        //SceneManager.LoadScene(minigameNumber, LoadSceneMode.Additive);
        //Invoke("LoadBoardgameScene", 2f);

        LoadMiniGameScene().Forget();
    }

    private async UniTaskVoid LoadMiniGameScene()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1f)); // 1�� ��ٷȴٰ�
        SceneManager.LoadScene(minigameNumber, LoadSceneMode.Additive); // �̴ϰ��Ӿ��� additive ���� ���ļ� �ε��Ѵ�
        await UniTask.Delay(TimeSpan.FromSeconds(1f)); // 1�� ��ٷȴٰ�
        SceneManager.UnloadScene(minigameNumber); // �̴ϰ��Ӿ��� �ٽ� Unload �Ѵ�
       // _turnManager.EndMinigame();  // �̴ϰ��Ӿ� unload �� �ϸŴ������� �˸�
    }

    private void LoadBoardgameScene()
    {
        // TODO: ����������� �ٽ� �Ѿ���� ���� �������� �� _turnManager.EndMinigame() ���� �ٽ� �׽�Ʈ�غ���
        // �ۼ��� �ñ⿡�� LoadMinigame -> 1�� �� unload ����̾�� �����
     //   _turnManager.EndMinigame();
        SceneManager.UnloadScene(minigameNumber);
    }

    private async UniTaskVoid SpawnPlayer()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(2f));
            
        if (PhotonNetwork.IsMasterClient)
        {
            for (int actorNumber = 1; actorNumber < PhotonNetwork.CurrentRoom.PlayerCount + 1; actorNumber++)
            {
                GameObject player = PhotonNetwork.Instantiate(models.BoardGameModel[actorNumber].name, positions.BoardGameSpawnPosition[actorNumber].transform.position,
                    Quaternion.identity);
                PhotonView pv = player.GetPhotonView();
                playerPv[actorNumber] = pv;
                pv.TransferOwnership(actorNumber);
            }
        }
        
        isPlayerAllInstantiated = true;
    }
    

}
