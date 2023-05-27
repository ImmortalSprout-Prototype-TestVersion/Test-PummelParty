using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal.Internal;

public abstract class Tile : MonoBehaviour
{
    public int tileIndex; // Ÿ�� ��ȣ�� Ȯ���ϱ����� ���� �׽�Ʈ�� �ڵ�
    public PlayerController player;
    private bool isPlayerOnTile = false;

    protected Transform collidedPlayerTransform; // ���� Ÿ�ϰ� �ε��� �÷��̾��� ������ �޾ƿ��� ���� ����

    private Vector3 currentTilePosition; // ���� Ÿ���� ��ġ

    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - \n")]
    [SerializeField] private Tile defaultTile; // �⺻ Ÿ��
    [SerializeField] private Vector3 defaultTilePosition;

    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - \n")]
    [SerializeField] private Tile nextTile; // �÷��̾ �̵��� ����Ÿ���� 1���̴�
    [SerializeField] private Vector3 nextTilePosition;

    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - \n")]
    [SerializeField] private Tile backTile; // ���� Ÿ���� 1���̴�
    [SerializeField] private Vector3 backTilePosition;



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
    /// ���� Ÿ���� �⺻ ���� Ÿ���� ��ȯ�Ѵ�
    /// </summary>
    /// <returns></returns>
    public Tile GetDefaultTile()
    {
        return defaultTile;
    }

    /// <summary>
    /// ���� Ÿ���� �⺻ ���� Ÿ���� ��ġ�� ��ȯ�Ѵ�
    /// </summary>
    /// <returns></returns>
    public Vector3 GetDefaultTilePosition()
    {
        return defaultTilePosition;
    }

    /// <summary>
    /// ���� Ÿ���� ��ġ�� �����´�(�θ� ��ġ�� ������)
    /// </summary>
    /// <returns></returns>
    public Vector3 GetCurrentTilePosition()
    {
        currentTilePosition = transform.parent.position;
        return currentTilePosition;
    }

    /// <summary>
    /// �÷��̾ �̵��� ���� Ÿ���� ��ȯ�Ѵ�
    /// </summary>
    /// <returns></returns>
    public Tile GetNextTile()
    {
        if (nextTile == null)
        {
            Debug.Log("�̵��� ���� Ÿ���� null �̴�");
            return null;
        }
        return nextTile;
    }

    /// <summary>
    /// �÷��̾ �̵��� ���� Ÿ���� ��ġ�� ��ȯ�Ѵ�
    /// </summary>
    /// <returns></returns>
    public Vector3 GetNextTilePosition()
    {
        if (nextTilePosition == null)
        {
            Debug.Log("�̵��� ���� Ÿ���� ��ġ�� null�̴�");
        }

        return nextTilePosition;
    }


    /// <summary>
    /// �÷��̾ �̵��� ���� Ÿ���� ��ȯ�Ѵ�
    /// </summary>
    /// <returns></returns>
    public Tile GetBackTile()
    {
        if (backTile == null)
        {
            Debug.Log("�̵��� ���� Ÿ���� null�̴�");
        }
        return backTile;
    }


    /// <summary>
    /// �÷��̾ �̵��� ���� Ÿ���� ��ġ�� ��ȯ�Ѵ�
    /// </summary>
    /// <returns></returns>
    public Vector3 GetBackTilePosition()
    {
        if (backTilePosition == null)
        {
            Debug.Log("������ �̵��� ���� Ÿ���� ��ġ�� null�̴�");
        }
        return backTilePosition;
    }

    #endregion

    #region Set Functions

    /// <summary>
    /// (1) �÷��̾ ���� ���� Ÿ���� ����Ÿ���� defaultTile�� �Ҵ��ϸ�, (2) �� ��ġ�� defaultTilePosition�� �Ҵ��Ѵ�
    /// </summary>
    /// <param name="_defaultTile"></param>
    protected void SetDefaultTile(Tile _defaultTile)
    {
        if (_defaultTile != null)
        {
            defaultTile = _defaultTile;
            defaultTilePosition = defaultTile.transform.parent.position;
        }
    }


    /// <summary>
    /// (1) �÷��̾ ���� ���� Ÿ���� ����Ÿ���� nextTile�� �Ҵ��ϸ�, (2) �� ��ġ�� nextTilePosition�� �Ҵ��Ѵ�
    /// </summary>
    /// <param name="_nextTile"></param>
    public void SetNextTile(Tile _nextTile)
    {
        // SetNextTile�� Public�� ������, ArrowButton���� ȣ���� �ؾ��ϱ� �����̴�!
        if (_nextTile != null)
        {
            nextTile = _nextTile;
            nextTilePosition = nextTile.transform.parent.position;
        }
    }

    /// <summary>
    /// (1) �÷��̾ ���� ���� Ÿ���� ����Ÿ���� backTile�� �Ҵ��ϸ�, (2) �� ��ġ�� backTilePosition�� �Ҵ��Ѵ�
    /// </summary>
    /// <param name="_backTile"></param>
    protected void SetBackTile(Tile _backTile)
    {
        backTile = _backTile;
        backTilePosition = backTile.transform.parent.position;
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

            player = collision.gameObject.GetComponent<PlayerController>();

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
