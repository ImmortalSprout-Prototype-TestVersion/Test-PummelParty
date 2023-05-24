using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTile : Tile
{
    private Transform playerTransform;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            RotateTile(_playerTransform : playerTransform);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {

        }
    }

    private void RotateTile(Tile currentTile = null, Tile nextTile = null, Transform _playerTransform = null)
    {
        //float x
        //float angle = Mathf.Atan2();
        transform.Rotate(Vector3.up, 50f * Time.deltaTime);
        if (_playerTransform != null)
        {
            _playerTransform.Rotate(Vector3.up, 50f * Time.deltaTime);
        }

    }

    public void GetPlayerTransform(Transform _playerTransform)
    {
        playerTransform = _playerTransform;
    }

    public void RemovePlayerTransform()
    {
        playerTransform = null;
    }
}
