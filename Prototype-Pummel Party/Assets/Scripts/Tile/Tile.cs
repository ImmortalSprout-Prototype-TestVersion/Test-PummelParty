using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal.Internal;

public abstract class Tile : MonoBehaviour
{
    public int tileIndex; // 타일 번호를 확인하기위해 만든 테스트용 코드
    private bool isPlayerOnTile = false;

    [SerializeField] private List<Tile> nextTiles = new List<Tile>(); // 다음 타일의 개수는 2개 이상일수도 있기에, 리스트로 담아준다

    protected Transform collidedPlayerTransform;
    
    [SerializeField] private Tile nextTile; // 플레이어가 이동할 다음타일은 1개이다
    [SerializeField] private Vector3 nextTilePosition;

    [SerializeField] private Tile backTile; // 이전 타일은 1개이다
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
    /// 플레이어가 현재 타일 위에 존재하는지 여부를 반환한다
    /// </summary>
    /// <returns></returns>
    public bool IsPlayerOnTile()
    {
        return isPlayerOnTile;
    }


    #endregion

    #region Get Functions
    /// <summary>
    /// 플레이어가 이동할 타음타일을 반환한다
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
    /// 플레이어가 이동할 다음타일의 위치를 반환한다
    /// </summary>
    /// <returns></returns>
    public Vector3 GetNextTilePosition()
    {
        return nextTilePosition;
    }

    /// <summary>
    /// 플레이어가 이동할 이전 타일을 반환한다
    /// </summary>
    /// <returns></returns>
    public Tile GetBackTile()
    {
        return backTile;
    }

    /// <summary>
    /// 플레이어가 이동할 이전 타일의 위치를 반환한다
    /// </summary>
    /// <returns></returns>
    public Vector3 GetBackTilePosition()
    {
        return backTilePosition;
    }



    #endregion

    #region Set Functions

    /// <summary>
    /// 플레이어가 향할 다음 타일을 현재타일의 nextTile로 지정해준다
    /// </summary>
    /// <param name="_nextTile"></param>
    public void SetNextTile(Tile _nextTile)
    {
        nextTile = _nextTile;
        SetNextTilePosition(_nextTile.gameObject.transform.position);
    }

    /// <summary>
    /// 플레이어가 향할 이전 타일을 현재타일의 backTile로 지정해준다
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
