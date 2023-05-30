using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private Rigidbody rigid;
    [SerializeField] private float movePower;
    public RaceGame raceGame; // ���� �� �ٸ� ������ üũ�ϱ� ���� public
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        raceGame = new RaceGame();
    }

    void Update()
    {
        if (raceGame.isRace)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rigid.AddForce(Vector3.forward * movePower * Time.deltaTime, ForceMode.Impulse);
            }
        }
    }
}
