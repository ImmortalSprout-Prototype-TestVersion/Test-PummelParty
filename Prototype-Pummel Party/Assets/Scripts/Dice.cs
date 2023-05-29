using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice
{
    public int Roll()
    {
        int num = Random.Range(-1, 4);
        Debug.Log($"주사위 결과: {num}");
        return 3;
    }
}
