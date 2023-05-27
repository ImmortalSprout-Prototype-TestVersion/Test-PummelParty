using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal.Internal;

public abstract class Tile : MonoBehaviour
{
    public int tileIndex; // 타일 번호를 확인하기위해 만든 테스트용 코드
    public PlayerController player;
    private bool isPlayerOnTile = false;

    protected Transform collidedPlayerTransform; // 현재 타일과 부딪힌 플레이어의 참조를 받아오기 위한 변수

    private Vector3 currentTilePosition; // 현재 타일의 위치

    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - \n")]
    [SerializeField] private Tile defaultTile; // 기본 타일
    [SerializeField] private Vector3 defaultTilePosition;

    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - \n")]
    [SerializeField] private Tile nextTile; // 플레이어가 이동할 다음타일은 1개이다
    [SerializeField] private Vector3 nextTilePosition;

    [Header("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - \n")]
    [SerializeField] private Tile backTile; // 이전 타일은 1개이다
    [SerializeField] private Vector3 backTilePosition;



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
    /// 현재 타일의 기본 다음 타일을 반환한다
    /// </summary>
    /// <returns></returns>
    public Tile GetDefaultTile()
    {
        return defaultTile;
    }

    /// <summary>
    /// 현재 타일의 기본 다음 타일의 위치를 반환한다
    /// </summary>
    /// <returns></returns>
    public Vector3 GetDefaultTilePosition()
    {
        return defaultTilePosition;
    }

    /// <summary>
    /// 현재 타일의 위치를 가져온다(부모 위치를 가져옴)
    /// </summary>
    /// <returns></returns>
    public Vector3 GetCurrentTilePosition()
    {
        currentTilePosition = transform.parent.position;
        return currentTilePosition;
    }

    /// <summary>
    /// 플레이어가 이동할 다음 타일을 반환한다
    /// </summary>
    /// <returns></returns>
    public Tile GetNextTile()
    {
        if (nextTile == null)
        {
            Debug.Log("이동할 다음 타일은 null 이다");
            return null;
        }
        return nextTile;
    }

    /// <summary>
    /// 플레이어가 이동할 다음 타일의 위치를 반환한다
    /// </summary>
    /// <returns></returns>
    public Vector3 GetNextTilePosition()
    {
        if (nextTilePosition == null)
        {
            Debug.Log("이동할 다음 타일의 위치는 null이다");
        }

        return nextTilePosition;
    }


    /// <summary>
    /// 플레이어가 이동할 빽도 타일을 반환한다
    /// </summary>
    /// <returns></returns>
    public Tile GetBackTile()
    {
        if (backTile == null)
        {
            Debug.Log("이동할 빽도 타일은 null이다");
        }
        return backTile;
    }


    /// <summary>
    /// 플레이어가 이동할 빽도 타일의 위치를 반환한다
    /// </summary>
    /// <returns></returns>
    public Vector3 GetBackTilePosition()
    {
        if (backTilePosition == null)
        {
            Debug.Log("다음에 이동할 빽도 타일의 위치는 null이다");
        }
        return backTilePosition;
    }

    #endregion

    #region Set Functions

    /// <summary>
    /// (1) 플레이어가 향할 다음 타일을 현재타일의 defaultTile로 할당하며, (2) 그 위치를 defaultTilePosition에 할당한다
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
    /// (1) 플레이어가 향할 다음 타일을 현재타일의 nextTile로 할당하며, (2) 그 위치를 nextTilePosition에 할당한다
    /// </summary>
    /// <param name="_nextTile"></param>
    public void SetNextTile(Tile _nextTile)
    {
        // SetNextTile만 Public인 이유는, ArrowButton에서 호출을 해야하기 때문이다!
        if (_nextTile != null)
        {
            nextTile = _nextTile;
            nextTilePosition = nextTile.transform.parent.position;
        }
    }

    /// <summary>
    /// (1) 플레이어가 향할 빽도 타일을 현재타일의 backTile로 할당하며, (2) 그 위치를 backTilePosition에 할당한다
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
