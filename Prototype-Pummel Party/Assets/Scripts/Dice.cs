using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice
{
    public int Roll()
    {
        int num = Random.Range(-1, 4);
        Debug.Log($"나온 주사위값 =  {num}");
        return num;
    }
}
