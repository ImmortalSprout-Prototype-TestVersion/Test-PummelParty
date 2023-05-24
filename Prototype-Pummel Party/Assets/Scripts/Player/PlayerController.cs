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

    private int _diceResult;
    private int _moveCount = 0;
    private bool _canRoll = false;

    private const int DICE_ONE = 1;
    private const int DICE_MINUS = -1;

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

    //private void OnTriggerEnter(Collider other)
    //{
    //    _currentTile = other.gameObject.GetComponent<Tile>();
    //} 
    // 은수, 우석이 collider의 isTrigger를 해제하였음. 그래서 아래의 OnCollisionEnter로 바꿈

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Tile"))
        {
            _currentTile = collision.gameObject.GetComponent<Tile>();
        }
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

            // start와 end가 같은 경우 아래 while문을 탈 필요가 없는데, 계속 타고 있음!

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

    // TODO: _diceResult랑 _moveCount 의미 제대로 생각해서 분리..
    // 주사위 수에 따라 도달할 수 있는 타일 받아옴
    private void CheckGetatableTiles()
    {
        if (_moveCount >= 1)
        {
            //_destTilePosition = _currentTile.GetNextTilePositions()[0];
            // 위는 민영이가 작성한 코드인데, 옮기면서 아래로 바꿔주었음
            _destTilePosition = _currentTile.GetNextTilePosition();  
        }
        else if(_moveCount == -1)
        {
            _destTilePosition = _currentTile.GetBackTilePosition();
            _moveCount = 1;
        }
        else
        {
            Debug.Log("움직이지 않음");
        }
    }
}
