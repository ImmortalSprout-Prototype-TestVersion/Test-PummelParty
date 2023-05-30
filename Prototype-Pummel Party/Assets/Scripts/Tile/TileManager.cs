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
        //SetTiles();
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



}
