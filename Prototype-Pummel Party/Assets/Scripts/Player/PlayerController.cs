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
    // ����, �켮�� collider�� isTrigger�� �����Ͽ���. �׷��� �Ʒ��� OnCollisionEnter�� �ٲ�

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
        // �ֻ��� ī��Ʈ�� �����ִ� ���� �� ĭ �� �̵�
        while (_moveCount >= 1)
        {
            float t = 0f;
            Vector3 start = _controller.transform.position;
            Vector3 end = _destTilePosition;

            // start�� end�� ���� ��� �Ʒ� while���� Ż �ʿ䰡 ���µ�, ��� Ÿ�� ����!

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

        Debug.Log("�̵� ��");
        _turnManager.EndPlayerTurn();
    }

    // TODO: _diceResult�� _moveCount �ǹ� ����� �����ؼ� �и�..
    // �ֻ��� ���� ���� ������ �� �ִ� Ÿ�� �޾ƿ�
    private void CheckGetatableTiles()
    {
        if (_moveCount >= 1)
        {
            //_destTilePosition = _currentTile.GetNextTilePositions()[0];
            // ���� �ο��̰� �ۼ��� �ڵ��ε�, �ű�鼭 �Ʒ��� �ٲ��־���
            _destTilePosition = _currentTile.GetNextTilePosition();  
        }
        else if(_moveCount == -1)
        {
            _destTilePosition = _currentTile.GetBackTilePosition();
            _moveCount = 1;
        }
        else
        {
            Debug.Log("�������� ����");
        }
    }
}
