using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTile : Tile
{
    [SerializeField] private GameObject arrowSwitch;
    [SerializeField] private ArrowButton[] arrowButtons;
    private const int firstIndex = 0;
    private const int secondIndex = 1;

    private Quaternion initialRotation = Quaternion.identity;
    private Quaternion targetRotation;

    private const float RightDirection = 1f;
    private const float LeftDirection = -1f;
    private float rotationDirection;
    [SerializeField] [Range(1f, 4f)] private float rotationSpeed = 2f;

    private IEnumerator _StartActiveRotation;
    private IEnumerator _StartResetRotation;

    void Start()
    {
        _StartActiveRotation = StartActiveRotation();
        _StartResetRotation = StartResetRotation();

        //SetNextTilePosition(GetNextTile().transform.position);
        SetBackTilePosition(GetBackTile().transform.position);

        OnPlayerEnterDiretionTile -= TurnOnDirectionUI;
        OnPlayerEnterDiretionTile += TurnOnDirectionUI;
        OnPlayerLeaveDiretionTile -= TurnOffDirectionUI;
        OnPlayerLeaveDiretionTile += TurnOffDirectionUI;


        OnPlayerLeaveDiretionTile -= ResetTileRotation;
        OnPlayerLeaveDiretionTile += ResetTileRotation;

        arrowButtons[firstIndex].OnClickDirectionUI -= ActivateTileRotation;
        arrowButtons[firstIndex].OnClickDirectionUI += ActivateTileRotation;
        arrowButtons[secondIndex].OnClickDirectionUI -= ActivateTileRotation;
        arrowButtons[secondIndex].OnClickDirectionUI += ActivateTileRotation;
    }

    void Update()
    {
        //if (Input.GetKey(KeyCode.Space))
        //{
        //    RotateTile(_playerTransform : playerTransform);
        //}
        //else if (Input.GetKeyDown(KeyCode.Space))
        //{

        //}

    }

    public void TurnOnDirectionUI()
    {
        arrowSwitch.SetActive(true);
    }

    public void TurnOffDirectionUI()
    {
        arrowSwitch.SetActive(false);
    }


    private IEnumerator StartActiveRotation()
    {
        while(true)
        {
            while (1.111f < Quaternion.Angle(transform.rotation, targetRotation))
            {
                transform.Rotate(Vector3.up, rotationDirection * rotationSpeed);
                collidedPlayerTransform.Rotate(Vector3.up, rotationDirection * rotationSpeed);
                yield return null;
            }

            StopCoroutine(_StartActiveRotation);
            yield return null;
        }
    }
    

    private void ActivateTileRotation(float _rotation)
    {
        targetRotation = Quaternion.Euler(0f, _rotation, 0f);
        
        if(_rotation < 0)
        {
            rotationDirection = LeftDirection;
        }
        else if (0 < _rotation)
        {
            rotationDirection = RightDirection;
        }
        StartCoroutine(_StartActiveRotation);
    }

    private void ResetTileRotation()
    {
        StartCoroutine(_StartResetRotation);
    }

    private IEnumerator StartResetRotation()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            while (1.111f < Quaternion.Angle(transform.rotation, initialRotation))
            {
                transform.Rotate(Vector3.up, -rotationDirection * rotationSpeed);
                yield return null;
            }

            StopCoroutine(_StartResetRotation);
            yield return null;
        }
    }
}
