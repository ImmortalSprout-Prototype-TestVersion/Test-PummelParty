using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private TurnManager _turnManager;
    [SerializeField] private GameObject _controller;

    private Dice _dice;
    private Tile _currentTile;
    private Vector3 _destTilePosition;

    private int _moveCount = 0;
    private bool _canRoll = false;

    private void Awake()
    {
        _dice = new Dice();
    }

    private void OnEnable()
    {
        _turnManager.OnTurnStarted -= ChangeDiceAvailable;
        _turnManager.OnTurnStarted += ChangeDiceAvailable;
    }

    private void OnDisable()
    {
        _turnManager.OnTurnStarted -= ChangeDiceAvailable;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(_canRoll == false)
            {
                return;
            }

            ChangeDiceAvailable();
            _moveCount = _dice.Roll();
            CheckGetatableTiles();
            HelpMoveAsync();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _currentTile = other.gameObject.GetComponent<Tile>();
    }

    private void ChangeDiceAvailable()
    {
        _canRoll = !_canRoll;
    }

    private void HelpMoveAsync()
    {
        Move().Forget();
    }

    private async UniTaskVoid Move()
    {
        // 주사위 카운트가 남아있는 동안 한 칸 씩 이동
        while (_moveCount >= 1)
        {
            float t = 0f;
            Vector3 start = _controller.transform.position;
            Vector3 end = _destTilePosition;

            while (t - 0.1f < 1f)
            {
                t += Time.deltaTime;
                _controller.transform.position = Vector3.Lerp(start, end, t / 1f);
                await UniTask.Yield(); // yield return null;
            }

            Debug.Log($"Left MoveCount : {_moveCount}");
            _moveCount -= 1;
            CheckGetatableTiles();

            // await UniTask.Delay(100);  => yield return new WaitForSeconds(100);
            await UniTask.Yield();
        }

        Debug.Log("이동 끝");
        _turnManager.EndPlayerTurn();
    }

    // 주사위 수에 따라 도달할 수 있는 타일 받아옴
    private void CheckGetatableTiles()
    {
        if (_moveCount >= 1)
        {
            _destTilePosition = _currentTile.GetNextTilePositions()[0];
        }
        else if(_moveCount == -1)
        {
            _destTilePosition = _currentTile.GetBackTilePosition();
        }
        else if (_moveCount == 0)
        {
            Debug.Log("moveCount = 0");
        }
    }
}
