using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomData : MonoBehaviour
{
    [SerializeField] private TMP_Text roomButtonText;
    [SerializeField] private Image roomButtonBackground;
    private RoomInfo information;
    public string roomName { get; private set; }

    private Color defaultColor = Color.white;
    private Color selectedColor = Color.green;

    private RoomList rooms;

    private void Start()
    {
        rooms = transform.GetComponentInParent<RoomList>();
    }

    /// <summary>
    /// 매개변수로 받는 roomInformation을 방 정보로 세팅하겠다 라는 함수입니다
    /// </summary>
    /// <param name="roomInformation"></param>
    public void SetRoomInfo(RoomInfo roomInformation)
    {
        information = roomInformation;
        roomName = roomInformation.Name;
    }

    /// <summary>
    /// 방의 제목을 설정하는 함수입니다
    /// </summary>
    /// <param name="roomName"></param>
    /// <param name="currentPlayerCount"></param>
    /// <param name="maxPlayerCount"></param>
    public void SetRoomText(string roomName, int currentPlayerCount, int maxPlayerCount)
    {
        roomButtonText.text = roomName + $" ({currentPlayerCount} / {maxPlayerCount})";
    }

    public void SelectRoom()
    {
        if (rooms.GetSelectedRoom() == null) // 이미 선택된 방이 없다면
        {
            rooms.SetSelectedRoom(this); // 자기 자신을 선택된 방으로 만든다
            SetColorSelected(); // 자기자신을 선택된 색깔로 칠한다
        }
        else // 이미 선택된 방이 있다면
        {
            if (rooms.GetSelectedRoom() == this) // 근데 그 방이 지 자신이라면
            {
                rooms.SetRoomNull(); // 선택된 방을 null 로 만든다
                SetColorDefault(); // 자기자신을 디폴트 색으로 칠한다
            }
            else // 근데 그게 다른 방이라면
            {
                rooms.GetSelectedRoom().SetColorDefault(); // 다른 방의 색을 디폴트색으로 칠한다
                rooms.SetSelectedRoom(this); // 지 자신을 선택된 방으로 만든다
                SetColorSelected(); // 자신을 석탠된 색깔로 칠한다
            }
        }
    }

    /// <summary>
    /// 방버튼의 배경색을 디폴트 색으로 칠한다
    /// </summary>
    public void SetColorDefault()
    {
        roomButtonBackground.color = defaultColor;
    }

    /// <summary>
    /// 방 버튼의 배경색을 선택한 색으로 칠한다
    /// </summary>
    public void SetColorSelected()
    {
        roomButtonBackground.color = selectedColor;
    }
}
