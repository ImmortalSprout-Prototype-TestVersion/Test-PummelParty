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

    // 캐싱
    private Color confirmedColor = new Color(0, 245, 255, 255);
    private TMP_InputField inputField;
    private Image inputFieldImage;

    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
        inputFieldImage = GetComponent<Image>();
    }

    public void InputPlayerName()
    {
        //TMP_InputField inputField = GetComponent<TMP_InputField>();
        playerInput = inputField.text;

        if (string.IsNullOrEmpty(playerInput))
        {
            Debug.Log("Player Name is null or empty");
            return;
        }

        PhotonNetwork.NickName = playerInput;
        PlayerPrefs.SetString(playerNamePrefKey, playerInput);

        Debug.Log($"플레이어 이름: {PhotonNetwork.NickName}");

        inputFieldImage.color = confirmedColor;
        //inputField.GetComponent<Image>().color = new Color32 (134, 219, 255, 255);

    }
}
