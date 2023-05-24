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
    [SerializeField] private Tile backTile; // 어차피 이전 타일은 무조건 1개이다


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
    /// 다음 타일의 위치가 담긴 리스트를 반환한다
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
    /// 이전 타일의 위치를 반환한다
    /// </summary>
    /// <returns></returns>
    public Vector3 GetBackTilePosition()
    {
        return backTile.transform.position; 
    }

    #endregion

    #region Add Tiles
    
    /// <summary>
    /// 현재 타일의 다음 타일리스트와 이전타일을 _newTile로 지정한다
    /// </summary>
    /// <param name="_newTile"></param>
    public void AddBothTiles(Tile _newTile)
    {
        SetForwardTile(_newTile);
        _newTile.SetBackwardTile(this);
    }

    /// <summary>
    /// 현재 타일의 다음 타일리스트에 _newTile을 추가한다
    /// </summary>
    /// <param name="_newTile"></param>
    public void SetForwardTile(Tile _newTile)
    {
        nextTiles.Add(_newTile);
    }

    /// <summary>
    /// 현재 타일의 이전 타일을 _newTile로 지정한다
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
