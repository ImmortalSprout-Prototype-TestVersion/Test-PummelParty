using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManagerArray : MonoBehaviour
{
    [SerializeField] private int numberOfTiles = 10;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] GameObject tilePositions;
    [SerializeField] GameObject tilePrefabs;
    [SerializeField] private Vector3[] positions;

    private (Tile, bool)[] tileInformation;
    private Tile newTile;

    private const int defaultPlayerPositionIndex = 0;

    public (int, int)[] playerPositionIndex;

    [SerializeField] int numberOfCrossTiles = 3;
    private int[] crossTileIndex;


    private void Start()
    {
        crossTileIndex= new int[numberOfCrossTiles];
        crossTileIndex[0] = 3;
        crossTileIndex[1] = 5;
        crossTileIndex[3] = 9;

        CreateTiles(numberOfTiles);
        SetPlayerInitialIndexes();
    }

    #region Private Functions
    private void GetPositions(int _numberOfTiles) // 타일의 위치를 받아오는 함수
    {
        positions = new Vector3[_numberOfTiles];
        for (int i = 0; i < _numberOfTiles ;++i)
        {
            positions[i] = tilePositions.transform.GetChild(i).position;
        }
    }

    private void CreateTiles(int _numberOfTiles)
    {
        tileInformation = new (Tile, bool)[_numberOfTiles];

        int childrentCount = tilePositions.transform.childCount;
        Debug.Assert(childrentCount == _numberOfTiles); // 포지션 수와 타일수가 같은지 확인

        GetPositions(_numberOfTiles);

        // 타일 오브젝트를 생성하여 루트 오브젝트를 부모로 삼고, 위치를 동기화 시킨다
        for (int i = 0; i < _numberOfTiles; ++i)
        {
            newTile = Instantiate(tilePrefab, tilePrefabs.transform); 
            newTile.transform.position = positions[i]; 
            newTile.tileIndex = i; 
            tileInformation[i] = (newTile, false);
        }

        // 0번째 인덱스의 타일은 기본타일이니, 플레이어가 존재할 것이므로 true로 바꿔준다
        tileInformation[defaultPlayerPositionIndex].Item2 = true;
    }

    private void SetPlayerInitialIndexes() /// 초반 플레이어의 위치 인덱스를 설정해주는 함수
    {
        int playerCount = GameManager.Instance.playerCount;
        playerPositionIndex = new (int, int)[playerCount];
    }

    private Vector3 GetPlayerCurrentPosition(int _playerID)
    {
        return positions[playerPositionIndex[_playerID].Item1];
    }

    //public List<Vector3> GetPlayerNextPosition(int _playerID)
    //{

    //}

    private void UpdatePlayerPositionIndex()
    {
        int playerCount = GameManager.Instance.playerCount;
        for (int i = 0; i < playerCount;++i)
        {
            playerPositionIndex[i].Item1 = playerPositionIndex[i].Item2;
        }
        
    }

    #endregion

    #region Public Functions

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_playerID"></param>
    /// <param name="_diceValue"></param>
    public void AddPlayerPositionIndex(int _playerID, int _diceValue)
    {
        playerPositionIndex[_playerID].Item2 += playerPositionIndex[_playerID].Item1 + _diceValue;
    }

    #endregion
}
