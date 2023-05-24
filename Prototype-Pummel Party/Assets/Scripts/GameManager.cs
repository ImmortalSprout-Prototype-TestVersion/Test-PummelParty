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

    public Player player;

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

    void Update()
    {
        
    }

    private void LoadMinigameScene()
    {
        SceneManager.LoadScene(minigameNumber, LoadSceneMode.Additive);
        Invoke("LoadBoardgameScene", 3f);
    }

    private void LoadBoardgameScene()
    {
        SceneManager.UnloadScene(minigameNumber);
    }
}
