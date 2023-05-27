using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowButton : MonoBehaviour
{
    [SerializeField] private Tile currentTile;
    [SerializeField] private Tile nextTile;
    [SerializeField] private bool isIntersectionArrow; // ������ Ÿ������ �ƴ���
    [SerializeField] private RotationTile intersectionTile; // ������ Ÿ���� �������� �Ҵ����ش�
    [SerializeField] private BasicTile nextTileOfIntersection; // �ش���⿡�� ������ Ÿ���� ���� Ÿ��

    // ����Ÿ�Ͽ��� ������ Ÿ�ϱ����� �Ÿ�
    [SerializeField] private int DistanceToIntersection = 1;

    [SerializeField] private float rotationValue;
    
    public event Action<float> OnClickDirectionUI;

    private void OnMouseUpAsButton()
    {
        // ���� Ÿ���� NextTile�� �������ش�
        currentTile.SetNextTile(nextTile);
        SetIntersectionNextTile();
        // ȭ��ǥ ������Ʈ�� ��Ȱ��ȭ��Ų��
        transform.parent.gameObject.SetActive(false);
        // Ÿ���� ȸ����Ű�� �Լ��� �����Ѵ�
        OnClickDirectionUI?.Invoke(rotationValue);
    }

    // ������ �����̰� �ֻ��� ���� DistanceToIntersection�� �ʰ��ϰ� Rotation Tile���� �÷��̾ �밢�� ������ ������ ��� 
    // intersectionTile�� NextTile�� nextTileOfIntersection���� �����Ѵ�
    private void SetIntersectionNextTile()
    {
        if (isIntersectionArrow == true && currentTile.player.GetDiceResult() > DistanceToIntersection)
        {
            intersectionTile.SetNextTile(nextTileOfIntersection);
        }
    }
}
