using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private Rigidbody rigid;
    [SerializeField] private float movePower;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigid.AddForce(Vector3.forward * movePower * Time.deltaTime, ForceMode.Impulse);
        }
    }
}
