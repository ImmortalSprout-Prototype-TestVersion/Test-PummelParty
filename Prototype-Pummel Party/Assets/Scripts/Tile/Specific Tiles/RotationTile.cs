using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTile : Tile
{
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - \n")]
    [SerializeField] private GameObject arrowSwitch;
    [SerializeField] private ArrowButton[] arrowButtons;

    private Quaternion initialRotation = Quaternion.identity;
    private Quaternion targetRotation;
    private float rotationDirection;
    private bool isAlreadySub;

    private const float minThreshold = 1.111f; // 최소감지가 1도 정도로 국한되서 1.111 넣은거임...
    private const float RightDirection = 1f;
    private const float LeftDirection = -1f;
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - \n")]
    [SerializeField][Range(1f, 4f)] private float rotationSpeed = 2f;
    [SerializeField] private float timeUntilResetRotation = 2f;
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - \n")]
    [SerializeField] private bool isIntersection;

    


    private void Start()
    {
        SetDefaultTile(GetDefaultTile());
        SetNextTile(GetNextTile());
        SetBackTile(GetBackTile());

        // 주사위를 굴리고 난 후, 이벤트에 TurnOnDirectionUI 함수를 구독해줘야함
        //OnPlayerEnterDiretionTile -= TurnOnDirectionUI;
        //OnPlayerEnterDiretionTile += TurnOnDirectionUI;
        //OnPlayerLeaveDiretionTile -= TurnOffDirectionUI;
        //OnPlayerLeaveDiretionTile += TurnOffDirectionUI;

        foreach (ArrowButton button in arrowButtons)
        {
            button.OnClickDirectionUI -= ActivateTileRotation;
            button.OnClickDirectionUI += ActivateTileRotation;
        }

        OnPlayerLeaveDiretionTile -= ResetTileRotation;
        OnPlayerLeaveDiretionTile += ResetTileRotation;
        OnPlayerLeaveDiretionTile -= ResetDefaultTile;
        OnPlayerLeaveDiretionTile += ResetDefaultTile;
        OnPlayerLeaveDiretionTile -= TurnOffDirection;
        OnPlayerLeaveDiretionTile += TurnOffDirection;
    }


    private void OnDisable()
    {
        foreach (ArrowButton button in arrowButtons)
        {
            button.OnClickDirectionUI -= ActivateTileRotation;
        }
        OnPlayerLeaveDiretionTile -= ResetTileRotation;
        OnPlayerLeaveDiretionTile -= ResetDefaultTile;
        OnPlayerLeaveDiretionTile -= TurnOffDirection;
    }



    // 좋은 접근방식이긴 했는데... 이 OncollisionEnter가 Tile의 OncollisionEnter를 먹어버리네.. ㅠㅠㅠㅠㅠ 
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        if (isAlreadySub == false) // 구독이 아직 되지 않았다면
    //        {
    //            collision.gameObject.GetComponent<PlayerController>().OnDiceRolled -= TurnOnDirectionUI;
    //            collision.gameObject.GetComponent<PlayerController>().OnDiceRolled += TurnOnDirectionUI;
    //            // 화살표를 띄워주는 함수를 OnDiceRolled 이벤츠에 구독을 해준다
    //            isAlreadySub = true; // 다시 구독을 시키지 않기 위해 isAlreadySub 을 true로 만들어준다
    //        }
    //    }
    //}

    /// <summary>
    /// 플레이어의 OnDiceRoll 이벤트에 TurnOnDirectionUI 함수를 구독시켜주기 위한 함수이다
    /// </summary>
    /// <param name="_playerController"></param>
    public void SubscribeArrowPop(PlayerController _playerController)
    {
        if (isAlreadySub == false)
        {
            _playerController.OnDiceRolled -= TurnOnDirection;
            _playerController.OnDiceRolled += TurnOnDirection;
            isAlreadySub = true;
        }
    }


    private void TurnOnDirection()
    {
        if (IsPlayerOnTile()) // 플레이어가 타일 위에 위치한다면 화살표를 띄워준다
        {
            arrowSwitch.SetActive(true);
        }
        // 플레이어가 타일위에 위치하고 있지 않다면 아무것도 하지 않는다 
    }

    private void TurnOffDirection()
    {
        arrowSwitch.SetActive(false);
    }

    private void ResetDefaultTile()
    {
        SetNextTile(GetDefaultTile());
    }

    //private IEnumerator StartActiveRotation()
    //{
    //    while(true)
    //    {
    //        while (1.111f < Quaternion.Angle(transform.rotation, targetRotation)) 
    //        {
    //            transform.Rotate(Vector3.up, rotationDirection * rotationSpeed);
    //            collidedPlayerTransform.Rotate(Vector3.up, rotationDirection * rotationSpeed);
    //            yield return null;
    //        }

    //        StopCoroutine(_StartActiveRotation); // 재사용을 위한 자식
    //        yield return null;
    //    }
    //}

    private async UniTaskVoid StartActiveRotation()
    {
        while (minThreshold < Quaternion.Angle(transform.parent.rotation, targetRotation))
        {
            transform.parent.Rotate(Vector3.up, rotationDirection * rotationSpeed);
            collidedPlayerTransform.Rotate(Vector3.up, rotationDirection * rotationSpeed);
            await UniTask.Yield();
        }
        transform.parent.rotation = targetRotation; // 1.111f 의 차이를 없애주기 위해 targetRotation 으로 변경해준다
    }
    private void ActivateTileRotation(float _rotation)
    {
        targetRotation = Quaternion.Euler(0f, _rotation, 0f);

        // Y축을 기준으로
        if (_rotation < 0) // 음수면 왼쪽으로 돌고
            rotationDirection = LeftDirection;
        else if (0 < _rotation) // 양수라면 오른쪽으로 돈다
            rotationDirection = RightDirection;

        //StartCoroutine(_StartActiveRotation);
        StartActiveRotation().Forget();
    }

    private void ResetTileRotation()
    {
        //StartCoroutine(_StartResetRotation);
        StartResetRotation().Forget();
    }

    private async UniTaskVoid StartResetRotation()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(timeUntilResetRotation));

        while (minThreshold < Quaternion.Angle(transform.parent.rotation, initialRotation))
        {
            transform.parent.Rotate(Vector3.up, -rotationDirection * rotationSpeed);
            await UniTask.Yield();
        }
        transform.parent.rotation = initialRotation; // 1.111f 의 차이를 없애주기 위해 initalRotation 으로 변경해준다
    }

    //private IEnumerator StartResetRotation()
    //{
    //    yield return new WaitForSeconds(1f);
    //    while (true)
    //    {
    //        while (1.111f < Quaternion.Angle(transform.rotation, initialRotation))
    //        {
    //            transform.Rotate(Vector3.up, -rotationDirection * rotationSpeed);
    //            yield return null;
    //        }

    //        StopCoroutine(_StartResetRotation);
    //        yield return null;
    //    }
    //}
}
