using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TurnManager _turnManager;

    private const int boardgameNumber = 0;
    private const int minigameNumber = 1;
    public int playerCount = 4;


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
        _turnManager.OnTurnEnd -= LoadMinigameScene;
        _turnManager.OnTurnEnd += LoadMinigameScene;
    }

    private void LoadMinigameScene()
    {
        //SceneManager.LoadScene(minigameNumber, LoadSceneMode.Additive);
        //Invoke("LoadBoardgameScene", 2f);

        LoadMiniGameScene().Forget();
    }

    private async UniTaskVoid LoadMiniGameScene()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1f)); // 1초 기다렸다가
        SceneManager.LoadScene(minigameNumber, LoadSceneMode.Additive); // 미니게임씬을 additive 모드로 겹쳐서 로드한다
        await UniTask.Delay(TimeSpan.FromSeconds(1f)); // 1초 기다렸다가
        SceneManager.UnloadScene(minigameNumber); // 미니게임씬을 다시 Unload 한다
    }

    private void LoadBoardgameScene()
    {
        SceneManager.UnloadScene(minigameNumber);
    }

    

}
