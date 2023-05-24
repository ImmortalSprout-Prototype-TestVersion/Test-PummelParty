using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowButton : MonoBehaviour
{
    [SerializeField] private Tile currentTile;
    [SerializeField] private Tile nextTile;

    [SerializeField] private float rotationValue;
    
    public event Action<float> OnClickDirectionUI;

    private void OnMouseUpAsButton()
    {
        currentTile.SetNextTile(nextTile);

        transform.parent.gameObject.SetActive(false);
        OnClickDirectionUI?.Invoke(rotationValue);
    }
}
