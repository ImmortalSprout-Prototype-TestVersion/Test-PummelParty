using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTile : Tile
{
    [SerializeField] private GameObject arrowSwitch;
    [SerializeField] private ArrowButton[] arrowButtons;

    private Quaternion initialRotation = Quaternion.identity;
    private Quaternion targetRotation;
    private float rotationDirection;

    private const float minThreshold = 1.111f; // �ּҰ����� 1�� ������ ���ѵǼ� 1.111 ��������...
    private const float RightDirection = 1f;
    private const float LeftDirection = -1f;

    [SerializeField][Range(1f, 4f)] private float rotationSpeed = 2f;
    [SerializeField] private float timeUntilResetRotation = 2f;

    //private IEnumerator _StartActiveRotation;
    //private IEnumerator _StartResetRotation;

    void Start()
    {
        //_StartActiveRotation = StartActiveRotation();
        //_StartResetRotation = StartResetRotation();

        //SetBackTile(GetBackTile());

        OnPlayerEnterDiretionTile -= TurnOnDirectionUI;
        OnPlayerEnterDiretionTile += TurnOnDirectionUI;
        OnPlayerLeaveDiretionTile -= TurnOffDirectionUI;
        OnPlayerLeaveDiretionTile += TurnOffDirectionUI;

        foreach (ArrowButton button in arrowButtons)
        {
            button.OnClickDirectionUI -= ActivateTileRotation;
            button.OnClickDirectionUI += ActivateTileRotation;
        }

        OnPlayerLeaveDiretionTile -= ResetTileRotation;
        OnPlayerLeaveDiretionTile += ResetTileRotation;
    }

    private void TurnOnDirectionUI()
    {
        arrowSwitch.SetActive(true);
    }

    private void TurnOffDirectionUI()
    {
        arrowSwitch.SetActive(false);
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
    }
    private void ActivateTileRotation(float _rotation)
    {
        targetRotation = Quaternion.Euler(0f, _rotation, 0f);

        // Y���� ��������
        if (_rotation < 0) // ����� ���������� ����
            rotationDirection = LeftDirection;
        else if (0 < _rotation) // ������ �������� ����
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
