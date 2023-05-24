using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal.Internal;

public abstract class Tile : MonoBehaviour
{
    public int tileIndex; // Ÿ�� ��ȣ�� Ȯ���ϱ����� ���� �׽�Ʈ�� �ڵ�
    private bool isPlayerOnTile = false;

    [SerializeField] private List<Tile> nextTiles = new List<Tile>(); // ���� Ÿ���� ������ 2�� �̻��ϼ��� �ֱ⿡, ����Ʈ�� ����ش�
    [SerializeField] private Tile backTile; // ������ ���� Ÿ���� ������ 1���̴�


    #region Check Functions

    /// <summary>
    /// �÷��̾ ���� Ÿ�� ���� �����ϴ��� ���θ� ��ȯ�Ѵ�
    /// </summary>
    /// <returns></returns>
    public bool IsPlayerOnTile()
    {
        return isPlayerOnTile;
    }

    #endregion

    #region Get Functions

    /// <summary>
    /// ���� Ÿ���� ��ġ�� ��� ����Ʈ�� ��ȯ�Ѵ�
    /// </summary>
    /// <returns></returns>
    public List<Vector3> GetNextTilePositions()
    {
        List<Vector3> nextTilePositions = new List<Vector3>();

        foreach (Tile element in nextTiles)
        {
            nextTilePositions.Add(element.transform.position);
        }
        return nextTilePositions;
    }

    /// <summary>
    /// ���� Ÿ���� ��ġ�� ��ȯ�Ѵ�
    /// </summary>
    /// <returns></returns>
    public Vector3 GetBackTilePosition()
    {
        return backTile.transform.position; 
    }

    #endregion

    #region Add Tiles
    
    /// <summary>
    /// ���� Ÿ���� ���� Ÿ�ϸ���Ʈ�� ����Ÿ���� _newTile�� �����Ѵ�
    /// </summary>
    /// <param name="_newTile"></param>
    public void AddBothTiles(Tile _newTile)
    {
        SetForwardTile(_newTile);
        _newTile.SetBackwardTile(this);
    }

    /// <summary>
    /// ���� Ÿ���� ���� Ÿ�ϸ���Ʈ�� _newTile�� �߰��Ѵ�
    /// </summary>
    /// <param name="_newTile"></param>
    public void SetForwardTile(Tile _newTile)
    {
        nextTiles.Add(_newTile);
    }

    /// <summary>
    /// ���� Ÿ���� ���� Ÿ���� _newTile�� �����Ѵ�
    /// </summary>
    /// <param name="_newTile"></param>
    public void SetBackwardTile(Tile _newTile)
    {
        backTile = _newTile;
    }

    #endregion

    #region Other Functions

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnTile = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnTile = false;
        }
    }

    #endregion
}
