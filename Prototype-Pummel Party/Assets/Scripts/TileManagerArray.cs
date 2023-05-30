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
    private void GetPositions(int _numberOfTiles) // Ÿ���� ��ġ�� �޾ƿ��� �Լ�
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
        Debug.Assert(childrentCount == _numberOfTiles); // ������ ���� Ÿ�ϼ��� ������ Ȯ��

        GetPositions(_numberOfTiles);

        // Ÿ�� ������Ʈ�� �����Ͽ� ��Ʈ ������Ʈ�� �θ�� ���, ��ġ�� ����ȭ ��Ų��
        for (int i = 0; i < _numberOfTiles; ++i)
        {
            newTile = Instantiate(tilePrefab, tilePrefabs.transform); 
            newTile.transform.position = positions[i]; 
            newTile.tileIndex = i; 
            tileInformation[i] = (newTile, false);
        }

        // 0��° �ε����� Ÿ���� �⺻Ÿ���̴�, �÷��̾ ������ ���̹Ƿ� true�� �ٲ��ش�
        tileInformation[defaultPlayerPositionIndex].Item2 = true;
    }

    private void SetPlayerInitialIndexes() /// �ʹ� �÷��̾��� ��ġ �ε����� �������ִ� �Լ�
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
