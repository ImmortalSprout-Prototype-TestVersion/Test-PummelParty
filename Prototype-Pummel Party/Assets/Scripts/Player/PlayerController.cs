using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event Action OnDiceRolled;

    [SerializeField] private TurnManager _turnManager;
    private float _moveTime = 1f;
    private float _rotateTime = 1f;
    private int _waitTimeBeforMove = 3000;

    private Dice _dice;
    
    private Tile _currentTile;
    private Vector3 _destTilePosition;

    private int _moveCount = 0;
    private bool _canRoll = false;

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
            CheckGetatableTiles();
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
        await UniTask.Delay(_waitTimeBeforMove);
        Move().Forget();
    }

    private async UniTaskVoid Move()
    {
        // �ֻ��� ī��Ʈ�� �����ִ� ���� �� ĭ �� �̵�
        Vector3 start = Vector3.zero;
        Vector3 end = Vector3.zero;

        if (_currentTile.transform.position == _destTilePosition)
        {
            return;
        }

        while (_moveCount >= 1)
        {
            float elapsedTime = 0f;
            start = transform.position;
            end = _destTilePosition;

            await LookNextDestTile((end - start).normalized);

            while (elapsedTime - 0.1f < 1f)
            {
                elapsedTime += Time.deltaTime;
                transform.position = Vector3.Lerp(start, end, elapsedTime / _moveTime);
                await UniTask.Yield();
            }

            Debug.Log($"Left MoveCount : {_moveCount}");
            _moveCount -= 1;

            CheckGetatableTiles();
            await UniTask.Yield();
        }

        Debug.Log("�̵� ��");
        await LookForward();

        Debug.Log("ȸ�� ��");
        _turnManager.EndPlayerTurn();
    }

    // TODO: _diceResult�� _moveCount �ǹ� ����� �����ؼ� �и�..
    // �ֻ��� ���� ���� ������ �� �ִ� Ÿ�� �޾ƿ�
    private void CheckGetatableTiles()
    {
        if (_moveCount >= 1)
        {
            _destTilePosition = _currentTile.GetNextTilePosition();
        }
        else if (_moveCount == -1)
        {
            _destTilePosition = _currentTile.GetBackTilePosition();
            // _destTilePosition.y = transform.position.y;
            _moveCount = 1;
        }
        else
        {
            Debug.Log("�������� ����");
        }
    }

    // �̵��� ���� �� ���� �ٶ󺸱�
    private async UniTask<bool> LookForward()
    {
        Vector3 camDir = Vector3.forward * -1f; // �ٶ�����ϴ� ���� ����
        camDir.y = transform.position.y;
        camDir = camDir.normalized;

        Quaternion start = transform.rotation;
        Quaternion end = Quaternion.LookRotation(camDir, transform.up);

        float elapsedTime = 0f;
        while (elapsedTime < _rotateTime)
        {
            elapsedTime += Time.deltaTime;
            var lerpval = Quaternion.Lerp(start, end, elapsedTime / _rotateTime);
            transform.rotation = lerpval;
            await UniTask.Yield();
        }

        return true;
    }

    // �̵��ϱ� ���� ������ Ÿ�� �������� ȸ��
    private async UniTask<bool> LookNextDestTile(Vector3 dir)
    {
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
    /// �ֻ��� ���� ����� ��ȯ
    /// </summary>
    public int GetDiceResult()
    {
        return _moveCount;
    }
}
