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

    private const float minThreshold = 1.111f; // �ּҰ����� 1�� ������ ���ѵǼ� 1.111 ��������...
    private const float RightDirection = 1f;
    private const float LeftDirection = -1f;
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - \n")]
    [SerializeField][Range(1f, 4f)] private float rotationSpeed = 2f;
    [SerializeField] private float timeUntilResetRotation = 2f;
    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - \n")]
    [SerializeField] LayerMask interactableMask;


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
    }


    private void OnDisable()
    {
        foreach (ArrowButton button in arrowButtons)
        {
            button.OnClickDirectionUI -= ActivateTileRotation;
        }
        OnPlayerLeaveDiretionTile -= ResetTileRotation;
    }

    [SerializeField] private int countOfDiagnolTiles = 2;
    private RaycastHit[] hitTiles;
    private RaycastHit[] hitTilesInOrder;
    private bool[] compareCheck;
    private int hitCount;
    private void ShootRay()
    {
        
        hitTiles = new RaycastHit[countOfDiagnolTiles];
        hitCount = Physics.RaycastNonAlloc(transform.position, -transform.forward, hitTiles, float.MaxValue, interactableMask);
        // hitTiles[]�� �浹���� ������� �����ϰ� �Ҵ��� �ȴ� => �Ÿ��� ª�� ������� �ٽ� ��ƾ� �Ѵ�

        hitTilesInOrder = new RaycastHit[countOfDiagnolTiles];
        compareCheck = new bool[countOfDiagnolTiles];

        for (int i = 0; i < countOfDiagnolTiles ;++i)
        {
            float minDistance = float.MaxValue;
            int selectedIndex = int.MaxValue;
            RaycastHit targetData = default(RaycastHit);

            for (int k = 0; k < countOfDiagnolTiles ;++k)
            {
                if (compareCheck[k] == false && hitTiles[k].distance <= minDistance)
                {
                    minDistance= hitTiles[k].distance;
                    selectedIndex = k;
                    targetData= hitTiles[k];
                }
            }

            compareCheck[selectedIndex] = true;
            hitTilesInOrder[i] = targetData;
        }

        foreach (RaycastHit element in hitTilesInOrder)
        {
            Debug.Log($"�̸� = {element.collider.name} / ���� = {element.distance}");
        }

    }

    private void Update()
    {
        Debug.DrawLine(transform.position, -transform.forward * float.MaxValue, Color.red);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootRay();
        }
    }

    private void TurnOnDirectionUI()
    {
        arrowSwitch.SetActive(true);
    }

    private void TurnOffDirectionUI()
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
