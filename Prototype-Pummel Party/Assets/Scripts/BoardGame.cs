using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BoardGame : ScriptableObject
{
    public Queue<int> result;
    public bool asdf;
    public void Init()
    {
        result = new Queue<int>();
    }
}
