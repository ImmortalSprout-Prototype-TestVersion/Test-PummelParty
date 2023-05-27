using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowButton : MonoBehaviour
{
    [SerializeField] private Tile currentTile;
    [SerializeField] private Tile nextTile;
    [SerializeField] private bool isIntersectionArrow; // 교차로 타일인지 아닌지
    [SerializeField] private RotationTile intersectionTile; // 교차로 타일을 정적으로 할당해준다
    [SerializeField] private BasicTile nextTileOfIntersection; // 해당방향에서 교차로 타일의 다음 타일

    // 현재타일에서 교차로 타일까지의 거리
    [SerializeField] private int DistanceToIntersection = 1;

    [SerializeField] private float rotationValue;
    
    public event Action<float> OnClickDirectionUI;

    private void OnMouseUpAsButton()
    {
        // 현재 타일의 NextTile을 설정해준다
        currentTile.SetNextTile(nextTile);
        SetIntersectionNextTile();
        // 화살표 오브젝트를 비활성화시킨다
        transform.parent.gameObject.SetActive(false);
        // 타일을 회전시키는 함수를 실행한다
        OnClickDirectionUI?.Invoke(rotationValue);
    }

    // 교차로 방향이고 주사위 값이 DistanceToIntersection을 초과하고 Rotation Tile에서 플레이어가 대각선 방향을 선택한 경우 
    // intersectionTile의 NextTile을 nextTileOfIntersection으로 설정한다
    private void SetIntersectionNextTile()
    {
        if (isIntersectionArrow == true && currentTile.player.GetDiceResult() > DistanceToIntersection)
        {
            intersectionTile.SetNextTile(nextTileOfIntersection);
        }
    }
}
