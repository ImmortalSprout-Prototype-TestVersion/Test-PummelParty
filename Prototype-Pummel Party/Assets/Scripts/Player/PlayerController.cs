using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private TurnManager _turnManager;
    [SerializeField] private GameObject _controller;
    [SerializeField] private float _rotateTime = 1f;
    private Dice _dice;
    private Tile _currentTile;
    private Vector3 _destTilePosition;

    private int _diceResult;
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

        Debug.Log("이동 끝");
        await LookCamera();

        Debug.Log("회전 끝");
        _turnManager.EndPlayerTurn();
    }

    // TODO: _diceResult랑 _moveCount 의미 제대로 생각해서 분리..
    // 주사위 수에 따라 도달할 수 있는 타일 받아옴
    private void CheckGetatableTiles()
    {
        if (_moveCount >= 1)
        {
            // 위는 민영이가 작성한 코드인데, 옮기면서 아래로 바꿔주었음
            _destTilePosition = _currentTile.GetNextTilePosition();
        }
        else if(_moveCount == -1)
        {
            _destTilePosition = _currentTile.GetBackTilePosition();
            _destTilePosition.y = _controller.transform.position.y;
            _moveCount = 1;
        }
        else
        {
            Debug.Log("움직이지 않음");
        }
    }

    // 이동이 끝난 후 정면(카메라 방향) 바라보기
    private async UniTask<bool> LookCamera()
    {
        Vector3 camDir = Camera.main.transform.position - _controller.transform.position; // 카메라 보는 방향벡터
        camDir.y = _controller.transform.position.y;
        camDir = camDir.normalized;

        Quaternion start = _controller.transform.rotation;
        Quaternion end = Quaternion.LookRotation(camDir, _controller.transform.up);

        float elapsedTime = 0f;
        while(elapsedTime < _rotateTime)
        {
            elapsedTime += Time.deltaTime;
            var lerpval = Quaternion.Lerp(start, end, elapsedTime / _rotateTime);
            _controller.transform.rotation = lerpval;
            await UniTask.NextFrame();
        }

        return true;
    }

    // 이동하기 위해 목적지 타일 방향으로 회전
    private async UniTask<bool> LookNextDestTile(Vector3 dir)
    {
        Quaternion start = _controller.transform.rotation;
        Quaternion end = Quaternion.LookRotation(dir);

        float elapsedTime = 0f;
        while (elapsedTime < _rotateTime)
        {
            elapsedTime += Time.deltaTime;

            if(_controller.transform.position != _destTilePosition)
            {
                var lerpval = Quaternion.Lerp(start, end, elapsedTime / _rotateTime);
                _controller.transform.rotation = lerpval;
            }
            await UniTask.NextFrame();
        }

        return true;
    }
}
