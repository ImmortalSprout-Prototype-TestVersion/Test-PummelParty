using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private Rigidbody rigid;
    [SerializeField] private float movePower;
    PhotonView _photonView;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        if(_photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rigid.AddForce(Vector3.forward * movePower * Time.deltaTime, ForceMode.Impulse);
            }
        }
    }
}
