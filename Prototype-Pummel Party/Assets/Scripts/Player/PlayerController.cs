using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event Action OnDiceRolled;

    [SerializeField] private TurnManager _turnManager;
    private float _moveTime = 1f;
    private float _rotateTime = 1f;
    [SerializeField] private int _waitTimeBeforMove = 1000;

    private Dice _dice;
    
    private Tile _currentTile;
    private Vector3 _destTilePosition;

    private int _moveCount = 0;
    private bool _canRoll = false;
    private bool _canMoveOnDirectionTile = false; // 플레이어가 회전타일에서 움직일 수 있는지 여부

    public enum DICE_RESULT
    {
        Back = -1,
        Move = 1,
    }

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_canRoll == false)
            {
                return;
            }

            ChangeDiceAvailable();
            _moveCount = _dice.Roll();
            OnDiceRolled?.Invoke();
            HelpMoveAsync().Forget();
        }
    }

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

    private async UniTaskVoid HelpMoveAsync()
    {
        if (_currentTile.CompareTag("RotationTile")) // 현재 서있는 타일이 회전타일이라면
        {
            if (0 < _moveCount) // 주사위 수가 1 이상 이라면 
            {
                await UniTask.WaitUntil(() => _canMoveOnDirectionTile == true); // _canMoveOnDirectionTile 이 true가 될 때까지 기다린다
            }
        }

        CheckGetatableTiles();

        await UniTask.Delay(_waitTimeBeforMove);
        Move().Forget(); // _canMove가 true로 되면 Move()함수를 호출한다
    }

    private async UniTaskVoid Move()
    {
        // 주사위 카운트가 남아있는 동안 한 칸 씩 이동
        Vector3 start = Vector3.zero;
        Vector3 end = Vector3.zero;

        if (_currentTile.transform.position == _destTilePosition)
        {
            _turnManager.EndPlayerTurn();
            return;
        }

        while (_moveCount >= 1)
        {
            float elapsedTime = 0f;
            start = transform.position;
            end = _destTilePosition;

            Debug.Log($"Left MoveCount : {_moveCount}");
            _moveCount -= 1;

            await LookNextDestTile((end - start).normalized);

            while (elapsedTime - 0.1f < 1f)
            {
                elapsedTime += Time.deltaTime;
                transform.position = Vector3.Lerp(start, end, elapsedTime / _moveTime);
                await UniTask.Yield();
            }

            CheckGetatableTiles();
            await UniTask.Yield();
        }

        Debug.Log("이동 끝");
        await LookForward();

        Debug.Log("회전 끝");
        _turnManager.EndPlayerTurn();
    }

    // TODO: _diceResult랑 _moveCount 의미 제대로 생각해서 분리..
    // 주사위 수에 따라 도달할 수 있는 타일 받아옴
    private void CheckGetatableTiles()
    {
        if (_moveCount >= 1)
        {
            _destTilePosition = _currentTile.GetNextTilePosition();
        }
        else if (_moveCount == -1)
        {
            _destTilePosition = _currentTile.GetBackTilePosition();
            _moveCount = 1;
        }
        else
        {
            Debug.Log("움직이지 않음");
        }
    }

    // 이동이 끝난 후 정면 바라보기
    private async UniTask<bool> LookForward()
    {
        Vector3 camDir = Vector3.forward * -1f; // 바라봐야하는 방향 벡터
        camDir.y = transform.position.y;
        camDir = camDir.normalized;

        Quaternion start = transform.rotation;
        Quaternion end = Quaternion.LookRotation(camDir, transform.up);

        float elapsedTime = 0f;
        while (elapsedTime <= _rotateTime)
        {
            elapsedTime += Time.deltaTime;
            var lerpval = Quaternion.Lerp(start, end, elapsedTime / _rotateTime);
            transform.rotation = lerpval;
            await UniTask.Yield();
        }

        return true;
    }

    // 이동하기 위해 목적지 타일 방향으로 회전
    private async UniTask<bool> LookNextDestTile(Vector3 dir)
    {
        // 회전타일에서부터 출발하는 턴에서는 회전하지 않음
        if(_currentTile.CompareTag("RotationTile") && _canMoveOnDirectionTile)
        {
            _canMoveOnDirectionTile = false;
            return true;
        }

        Quaternion start = transform.rotation;
        Quaternion end = Quaternion.LookRotation(dir);

        float elapsedTime = 0f;
        while (elapsedTime < _rotateTime)
        {
            elapsedTime += Time.deltaTime;

            if (transform.position != _destTilePosition)
            {
                var lerpval = Quaternion.Lerp(start, end, elapsedTime / _rotateTime);
                transform.rotation = lerpval;
            }
            await UniTask.Yield();
        }

        return true;
    }

    /// <summary>
    /// 주사위 굴린 결과를 반환
    /// </summary>
    public int GetDiceResult()
    {
        return _moveCount;
    }

    /// <summary>
    /// 플레이어가 회전타일에서 움직여도 된다는 것을 알려주는 함수
    /// </summary>
    public void EnablePlayerMoveOnDirectionTile()
    {
        _canMoveOnDirectionTile = true;
    }

}
