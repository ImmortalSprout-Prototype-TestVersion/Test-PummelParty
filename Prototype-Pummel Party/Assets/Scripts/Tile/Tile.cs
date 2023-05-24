using System;
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

    protected Transform collidedPlayerTransform;
    
    [SerializeField] private Tile nextTile; // �÷��̾ �̵��� ����Ÿ���� 1���̴�
    [SerializeField] private Vector3 nextTilePosition;

    [SerializeField] private Tile backTile; // ���� Ÿ���� 1���̴�
    [SerializeField] private Vector3 backTilePosition;

    private void Start()
    {
        
        
    }

    protected void SetNextTilePosition(Vector3 _nextTilePosition)
    {
        if (_nextTilePosition != null)
        {
            nextTilePosition = _nextTilePosition;
        }
    }
    protected void SetBackTilePosition(Vector3 _backTilePosition)
    {
        if (_backTilePosition != null)
        {
            backTilePosition = _backTilePosition;
        }
    }

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
    /// �÷��̾ �̵��� Ÿ��Ÿ���� ��ȯ�Ѵ�
    /// </summary>
    /// <returns></returns>
    public Tile GetNextTile()
    {
        if (nextTile == null)
        {
            return null;
        }
        return nextTile;
    }

    /// <summary>
    /// �÷��̾ �̵��� ����Ÿ���� ��ġ�� ��ȯ�Ѵ�
    /// </summary>
    /// <returns></returns>
    public Vector3 GetNextTilePosition()
    {
        return nextTilePosition;
    }

    /// <summary>
    /// �÷��̾ �̵��� ���� Ÿ���� ��ȯ�Ѵ�
    /// </summary>
    /// <returns></returns>
    public Tile GetBackTile()
    {
        return backTile;
    }

    /// <summary>
    /// �÷��̾ �̵��� ���� Ÿ���� ��ġ�� ��ȯ�Ѵ�
    /// </summary>
    /// <returns></returns>
    public Vector3 GetBackTilePosition()
    {
        return backTilePosition;
    }



    #endregion

    #region Set Functions

    /// <summary>
    /// �÷��̾ ���� ���� Ÿ���� ����Ÿ���� nextTile�� �������ش�
    /// </summary>
    /// <param name="_nextTile"></param>
    public void SetNextTile(Tile _nextTile)
    {
        nextTile = _nextTile;
        SetNextTilePosition(_nextTile.gameObject.transform.position);
    }

    /// <summary>
    /// �÷��̾ ���� ���� Ÿ���� ����Ÿ���� backTile�� �������ش�
    /// </summary>
    /// <param name="_backTile"></param>
    public void SetBackTile(Tile _backTile)
    {
        backTile = _backTile;
        SetBackTilePosition(_backTile.gameObject.transform.position);
    }

    #endregion

    protected event Action OnPlayerEnterDiretionTile;
    protected event Action OnPlayerLeaveDiretionTile;

    #region Other Functions

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnTile = true;
            OnPlayerEnterDiretionTile?.Invoke();
            collidedPlayerTransform = collision.transform;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnTile = false;
            OnPlayerLeaveDiretionTile?.Invoke();
            collidedPlayerTransform = null;
        }
    }

    #endregion
}
