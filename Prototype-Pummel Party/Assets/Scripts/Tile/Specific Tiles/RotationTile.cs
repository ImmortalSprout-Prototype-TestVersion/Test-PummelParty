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

    private const float minThreshold = 1.111f; // �ּҰ����� 1�� ������ ���ѵǼ� 1.111 ��������...
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

        // �ֻ����� ������ �� ��, �̺�Ʈ�� TurnOnDirectionUI �Լ��� �����������
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



    // ���� ���ٹ���̱� �ߴµ�... �� OncollisionEnter�� Tile�� OncollisionEnter�� �Ծ������.. �ФФФФ� 
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        if (isAlreadySub == false) // ������ ���� ���� �ʾҴٸ�
    //        {
    //            collision.gameObject.GetComponent<PlayerController>().OnDiceRolled -= TurnOnDirectionUI;
    //            collision.gameObject.GetComponent<PlayerController>().OnDiceRolled += TurnOnDirectionUI;
    //            // ȭ��ǥ�� ����ִ� �Լ��� OnDiceRolled �̺����� ������ ���ش�
    //            isAlreadySub = true; // �ٽ� ������ ��Ű�� �ʱ� ���� isAlreadySub �� true�� ������ش�
    //        }
    //    }
    //}

    /// <summary>
    /// �÷��̾��� OnDiceRoll �̺�Ʈ�� TurnOnDirectionUI �Լ��� ���������ֱ� ���� �Լ��̴�
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
        if (IsPlayerOnTile()) // �÷��̾ Ÿ�� ���� ��ġ�Ѵٸ� ȭ��ǥ�� ����ش�
        {
            arrowSwitch.SetActive(true);
        }
        // �÷��̾ Ÿ������ ��ġ�ϰ� ���� �ʴٸ� �ƹ��͵� ���� �ʴ´� 
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

    //        StopCoroutine(_StartActiveRotation); // ������ ���� �ڽ�
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
        transform.parent.rotation = targetRotation; // 1.111f �� ���̸� �����ֱ� ���� targetRotation ���� �������ش�
    }
    private void ActivateTileRotation(float _rotation)
    {
        targetRotation = Quaternion.Euler(0f, _rotation, 0f);

        // Y���� ��������
        if (_rotation < 0) // ������ �������� ����
            rotationDirection = LeftDirection;
        else if (0 < _rotation) // ������ ���������� ����
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
        transform.parent.rotation = initialRotation; // 1.111f �� ���̸� �����ֱ� ���� initalRotation ���� �������ش�
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
