using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPosition : MonoBehaviour
{
    private void Awake()
    {
        transform.SetParent(GameManager.Instance.transform);
    }
}
