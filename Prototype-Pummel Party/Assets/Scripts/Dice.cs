using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice
{
    public int Roll()
    {
        int num = Random.Range(10, 11);
        Debug.Log($"�ֻ��� ���: {num}");
        return num;
    }
}
