using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class InputNickNameField : MonoBehaviour
{
    // [SerializeField] private TMP_Text playerName;

    private const string playerNamePrefKey = "PlayerName";
    private string playerInput = null;

    public void InputPlayerName()
    {
        TMP_InputField inputField = GetComponent<TMP_InputField>();
        playerInput = inputField.text;

        if (string.IsNullOrEmpty(playerInput))
        {
            Debug.Log("Player Name is null or empty");
            return;
        }

        PhotonNetwork.NickName = playerInput;
        PlayerPrefs.SetString(playerNamePrefKey, playerInput);

        Debug.Log($"플레이어 이름: {PhotonNetwork.NickName}");
    }
}
