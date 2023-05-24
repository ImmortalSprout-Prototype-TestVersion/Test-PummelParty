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
    //        Debug.Log("Stop Point ����!");
    //    }
    //}

    public TileManagerArray tileManager; // �׽�Ʈ��

    public event Action OnPlayerTurnFinished;
    private bool isPlayerTurnFinished;
    public float moveSpeed = 30f;

    private void OnEnable()
    {
        GameManager.Instance.player = this;
    }

    private void Update()
    {
        //if (isPlayerTurnFinished) // ���࿡ �÷����� ���� �����ٸ�!
        //{
        //    OnPlayerTurnFinished?.Invoke();
        //}

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Tile") && collision.gameObject.CompareTag("RotationTile"))
        {
            Debug.Log("Ÿ�ϰ� �����");
            RotationTile rotationTile = collision.gameObject.GetComponent<RotationTile>();
            rotationTile.GetPlayerTransform(this.transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Tile") && collision.gameObject.CompareTag("RotationTile"))
        {
            Debug.Log("Ÿ�Ͽ��� Ż��");
            RotationTile rotationTile = collision.gameObject.GetComponent<RotationTile>();
            rotationTile.RemovePlayerTransform();
        }
    }
}
