using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] private int numberOfTiles = 10;
    //[SerializeField] private Tile tilePrefab;
    //[SerializeField] GameObject TilePrefabsRoot;
    //[SerializeField] private Transform[] tilePositions;
    [SerializeField] private Tile[] tiles;

    private void Awake()
    {
        //CreateTilePrefabs(numberOfTiles);
        SetTiles();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }


    //private void CreateTilePrefabs(int _numberOfTiles)
    //{
    //    tiles= new Tile[_numberOfTiles];

    //    for (int i = 0; i < _numberOfTiles ;++i)
    //    {
    //        tiles[i] = Instantiate(tilePrefab, TilePrefabsRoot.transform);
    //        tiles[i].transform.position = tilePositions[i].position;
    //        tiles[i].tileIndex= i;
    //    }
    //}

    private void SetTiles()
    {
        tiles[0].SetForwardTile(tiles[1]);

        tiles[1].AddBothTiles(tiles[2]);
        tiles[1].SetBackwardTile(tiles[8]);

        tiles[2].AddBothTiles(tiles[3]);

        tiles[3].AddBothTiles(tiles[4]);

        tiles[4].AddBothTiles(tiles[5]);

        tiles[5].AddBothTiles(tiles[6]);

        tiles[6].AddBothTiles(tiles[7]);

        tiles[7].AddBothTiles(tiles[8]);

        tiles[8].SetForwardTile(tiles[1]);

        tiles[3].AddBothTiles(tiles[9]);
        tiles[5].SetForwardTile(tiles[9]);
        tiles[9].SetForwardTile(tiles[7]);
        tiles[9].SetForwardTile(tiles[1]);
    }

}
