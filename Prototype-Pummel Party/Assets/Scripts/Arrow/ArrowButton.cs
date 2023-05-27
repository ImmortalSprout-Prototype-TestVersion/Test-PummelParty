using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowButton : MonoBehaviour
{
    [SerializeField] private Tile currentTile;
    [SerializeField] private Tile nextTile;
    [SerializeField] private bool isIntersectionArrow; // ������ Ÿ�Ϲ����� ȭ��ǥ���� �ƴ���
    [SerializeField] private RotationTile intersectionTile; // ������ Ÿ���� �������� �Ҵ����ش�
    [SerializeField] private BasicTile nextTileOfIntersection; // �ش���⿡�� ������ Ÿ���� ���� Ÿ��

    // ����Ÿ�Ͽ��� ������ Ÿ�ϱ����� �Ÿ�
    [SerializeField] private int DistanceToIntersection = 1;

    [SerializeField] private float rotationValue;
    
    public event Action<float> OnClickDirectionUI;

    private void OnMouseUpAsButton() // �ش� ������Ʈ�� Ŭ������ �� �����ϴ� �Լ�
    {
        currentTile.SetNextTile(nextTile); // ���� Ÿ���� NextTile�� �������ش�
        SetIntersectionNextTile(); // ������ Ÿ���� ���� Ÿ���� �������ش�
        transform.parent.gameObject.SetActive(false); // ȭ��ǥ ������Ʈ�� ��Ȱ��ȭ��Ų��
        OnClickDirectionUI?.Invoke(rotationValue); // Ÿ���� ȸ����Ű�� �Լ��� �����Ѵ�
        currentTile.player.EnablePlayerMoveOnDirectionTile(); // �÷��̾ ������ �� �ְ� ������ش�
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
