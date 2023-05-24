using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("TileStopPoint"))
    //    {
    //        Debug.Log("Stop Point 만남!");
    //    }
    //}

    public TileManagerArray tileManager; // 테스트용

    public event Action OnPlayerTurnFinished;
    private bool isPlayerTurnFinished;
    public float moveSpeed = 30f;

    private void OnEnable()
    {
        GameManager.Instance.player = this;
    }

    private void Update()
    {
        //if (isPlayerTurnFinished) // 만약에 플레이의 턴이 끝났다면!
        //{
        //    OnPlayerTurnFinished?.Invoke();
        //}

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Tile") && collision.gameObject.CompareTag("RotationTile"))
        {
            Debug.Log("타일과 닿았음");
            RotationTile rotationTile = collision.gameObject.GetComponent<RotationTile>();
            rotationTile.GetPlayerTransform(this.transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Tile") && collision.gameObject.CompareTag("RotationTile"))
        {
            Debug.Log("타일에서 탈출");
            RotationTile rotationTile = collision.gameObject.GetComponent<RotationTile>();
            rotationTile.RemovePlayerTransform();
        }
    }
}
