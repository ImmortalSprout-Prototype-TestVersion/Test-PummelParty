using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTile : Tile
{
    // Start is called before the first frame update
    void Start()
    {
        SetNextTilePosition(GetNextTile().transform.position);
        SetBackTilePosition(GetBackTile().transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
