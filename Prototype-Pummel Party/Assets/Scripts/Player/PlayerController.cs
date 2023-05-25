using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private TurnManager _turnManager;  // TODO: �ϸŴ��� ���ӸŴ��� ���ؼ� ������ �� �ֵ��� ����
    
    private GameObject _controller;
    private float _rotateTime = 1f;

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
        _controller = transform.parent.gameObject;
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
            CheckGetatableTiles();
            HelpMoveAsync();
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

    private void HelpMoveAsync()
    {
        Move().Forget();
    }

    private async UniTaskVoid Move()
    {
        // �ֻ��� ī��Ʈ�� �����ִ� ���� �� ĭ �� �̵�
        Vector3 start = Vector3.zero;
        Vector3 end = Vector3.zero;

        while (_moveCount >= 1)
        {
            float t = 0f;
            start = _controller.transform.position;
            end = _destTilePosition;


            await LookNextDestTile((end - start).normalized);

            while (t - 0.1f < 1f)
            {
                t += Time.deltaTime;
                _controller.transform.position = Vector3.Lerp(start, end, t / 1f);
                await UniTask.NextFrame(); // yield return null;
            }

            Debug.Log($"Left MoveCount : {_moveCount}");
            _moveCount -= 1;

            CheckGetatableTiles();
            // await UniTask.Delay(100);  => yield return new WaitForSeconds(100);
            await UniTask.NextFrame();
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
            // TODO: Ÿ�� ������ �ٽ� �ϼ� �� �ּ� ���� �� �׽�Ʈ
            // _destTilePosition = _currentTile.GetNextTilePosition();
        }
        else if (_moveCount == -1)
        {
            _destTilePosition = _currentTile.GetBackTilePosition();
            _destTilePosition.y = _controller.transform.position.y;
            _moveCount = 1;
        }
        else
        {
            Debug.Log("�������� ����");
        }
    }

    // TODO: Ÿ�� ��ġ �� �׽�Ʈ �ٽ� �ؾ� �� (���� Ȯ��)
    // �̵��� ���� �� ���� �ٶ󺸱�
    private async UniTask<bool> LookForward()
    {
        // Vector3 camDir = Camera.main.transform.position - _controller.transform.position; // ī�޶� ���� ���⺤��
        Vector3 camDir = Vector3.forward; // �ٶ�����ϴ� ���� ����
        camDir.y = _controller.transform.position.y;
        camDir = camDir.normalized;

        Quaternion start = _controller.transform.rotation;
        Quaternion end = Quaternion.LookRotation(camDir, _controller.transform.up);

        float elapsedTime = 0f;
        while (elapsedTime < _rotateTime)
        {
            elapsedTime += Time.deltaTime;
            var lerpval = Quaternion.Lerp(start, end, elapsedTime / _rotateTime);
            _controller.transform.rotation = lerpval;
            await UniTask.NextFrame();
        }

        return true;
    }

    // �̵��ϱ� ���� ������ Ÿ�� �������� ȸ��
    private async UniTask<bool> LookNextDestTile(Vector3 dir)
    {
        Quaternion start = _controller.transform.rotation;
        Quaternion end = Quaternion.LookRotation(dir);

        float elapsedTime = 0f;
        while (elapsedTime < _rotateTime)
        {
            elapsedTime += Time.deltaTime;

            if (_controller.transform.position != _destTilePosition)
            {
                var lerpval = Quaternion.Lerp(start, end, elapsedTime / _rotateTime);
                _controller.transform.rotation = lerpval;
            }
            await UniTask.NextFrame();
        }

        return true;
    }
}
