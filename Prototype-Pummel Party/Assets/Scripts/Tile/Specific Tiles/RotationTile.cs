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
    }


    private void OnDisable()
    {
        foreach (ArrowButton button in arrowButtons)
        {
            button.OnClickDirectionUI -= ActivateTileRotation;
        }
        OnPlayerLeaveDiretionTile -= ResetTileRotation;
    }

    //[SerializeField] private int countOfDiagnolTiles = 2;
    //private RaycastHit[] hitTilesRandom;
    //private RaycastHit[] hitTilesInOrder;
    //private bool[] compareCheck;
    //private int hitCount;
    //private void ShootRay()
    //{
    //    if (isIntersection) // 자신이 교차로 이면 ray를 쏘지 않는다
    //    {
    //        return;
    //    }

    //    hitTilesRandom = new RaycastHit[countOfDiagnolTiles];
    //    hitCount = Physics.RaycastNonAlloc(transform.position, -transform.forward, hitTilesRandom, float.MaxValue, interactableMask);
    //    // hitTiles[]에 충돌순서 상관없이 랜덤하게 할당이 된다 => 거리가 짧은 순서대로 다시 담아야 한다

    //    hitTilesInOrder = new RaycastHit[countOfDiagnolTiles];
    //    compareCheck = new bool[countOfDiagnolTiles];

    //    // 충돌된 오브젝트들을 충돌거리가 짧은 순서로 다시 담아주는 알고리즘
    //    for (int i = 0; i < countOfDiagnolTiles ;++i)
    //    {
    //        float minDistance = float.MaxValue;
    //        int selectedIndex = int.MaxValue;
    //        RaycastHit targetData = default(RaycastHit);

    //        for (int k = 0; k < countOfDiagnolTiles ;++k)
    //        {
    //            if (compareCheck[k] == false && hitTilesRandom[k].distance <= minDistance) // compareCheck를 먼저하여 단축평가 시행
    //            {
    //                minDistance= hitTilesRandom[k].distance;
    //                selectedIndex = k;
    //                targetData= hitTilesRandom[k];
    //            }
    //        }
    //        compareCheck[selectedIndex] = true;
    //        hitTilesInOrder[i] = targetData;
    //    }
    //    // distance가 작은 순서대로 잘 담김

    //    foreach (RaycastHit element in hitTilesInOrder)
    //    {
    //        Debug.Log($"이름 = {element.collider.name} / 길이 = {element.distance}");
    //    }
    //}

    //public BasicTile GetNextTileOfIntersection()
    //{
    //    RotationTile intersectionTile = null;
    //    BasicTile nextTileOfIntersection = null;

    //    for (int i = 0; i < countOfDiagnolTiles ;++i)
    //    {
    //        if (hitTilesInOrder[i].collider.gameObject.GetComponent<RotationTile>().isIntersection == true)
    //        {
    //            nextTileOfIntersection = hitTilesInOrder[i + 1].collider.gameObject.GetComponent<BasicTile>();
    //        }
    //    }

    //    Debug.Log($"{intersectionTile.name}의 다음 기본타일 = {nextTileOfIntersection.name}");
    //    return nextTileOfIntersection;
    //}


    private void Update()
    {
        //Debug.DrawLine(transform.position, -transform.forward * float.MaxValue, Color.red);
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    ShootRay();
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (isAlreadySub == false)
            {
                collision.gameObject.GetComponent<PlayerController>().OnDiceRolled -= TurnOnDirectionUI;
                collision.gameObject.GetComponent<PlayerController>().OnDiceRolled += TurnOnDirectionUI;
                isAlreadySub = true;
            }
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
