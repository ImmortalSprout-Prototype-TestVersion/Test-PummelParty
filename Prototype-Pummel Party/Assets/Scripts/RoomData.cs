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
    /// �Ű������� �޴� roomInformation�� �� ������ �����ϰڴ� ��� �Լ��Դϴ�
    /// </summary>
    /// <param name="roomInformation"></param>
    public void SetRoomInfo(RoomInfo roomInformation)
    {
        information = roomInformation;
        roomName = roomInformation.Name;
    }

    /// <summary>
    /// ���� ������ �����ϴ� �Լ��Դϴ�
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
        if (rooms.GetSelectedRoom() == null) // �̹� ���õ� ���� ���ٸ�
        {
            rooms.SetSelectedRoom(this); // �ڱ� �ڽ��� ���õ� ������ �����
            SetColorSelected(); // �ڱ��ڽ��� ���õ� ����� ĥ�Ѵ�
        }
        else // �̹� ���õ� ���� �ִٸ�
        {
            if (rooms.GetSelectedRoom() == this) // �ٵ� �� ���� �� �ڽ��̶��
            {
                rooms.SetRoomNull(); // ���õ� ���� null �� �����
                SetColorDefault(); // �ڱ��ڽ��� ����Ʈ ������ ĥ�Ѵ�
            }
            else // �ٵ� �װ� �ٸ� ���̶��
            {
                rooms.GetSelectedRoom().SetColorDefault(); // �ٸ� ���� ���� ����Ʈ������ ĥ�Ѵ�
                rooms.SetSelectedRoom(this); // �� �ڽ��� ���õ� ������ �����
                SetColorSelected(); // �ڽ��� ���ĵ� ����� ĥ�Ѵ�
            }
        }
    }

    /// <summary>
    /// ���ư�� ������ ����Ʈ ������ ĥ�Ѵ�
    /// </summary>
    public void SetColorDefault()
    {
        roomButtonBackground.color = defaultColor;
    }

    /// <summary>
    /// �� ��ư�� ������ ������ ������ ĥ�Ѵ�
    /// </summary>
    public void SetColorSelected()
    {
        roomButtonBackground.color = selectedColor;
    }
}
