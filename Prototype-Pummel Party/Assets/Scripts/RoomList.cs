using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class RoomList : MonoBehaviourPunCallbacks
{
    public UnityEvent setting;

    [SerializeField] private RoomData roomData;
    [SerializeField] private Transform roomListPosition;

    private Dictionary<string, RoomData> rooms= new Dictionary<string, RoomData>();

    [SerializeField] private RoomData selectedRoom;

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList) // ���� ������ ���̶��
            {
                if (rooms.ContainsKey(roomInfo.Name)) // ��ųʸ��� ��ϵ� ���̶��
                {
                    Destroy(rooms[roomInfo.Name].gameObject); // UI�󿡼� ���� �����Ѵ�
                    rooms.Remove(roomInfo.Name); // ��ųʸ����� ���� Key�� �����Ѵ�
                }
            }
            else // ������ �ƴ� ��������� �߻��ߴٸ�
            {
                RoomData newRoom = null; // ���ο� UI ���� ������ �����Դϴ�

                if (rooms.ContainsKey(roomInfo.Name)) // �ش� ���� �̹� ��ųʸ��� �����Ѵٸ�
                {
                    return; // �ƹ��͵� ���ϰ� �Լ��� �����Ѵ�
                }

                newRoom = Instantiate(roomData, roomListPosition); 
                // roomListPosition�� �θ�� �ϴ� roomData�� �������ش�

                rooms.Add(roomInfo.Name, newRoom); // ��ųʸ��� �� ���� �־��ش�

                newRoom.SetRoomInfo(roomInfo); // newRoom�� information�� roomInfo�� �Ҵ��Ѵ�
                newRoom.SetRoomText(roomInfo.Name, roomInfo.PlayerCount, roomInfo.MaxPlayers); // ���� ǥ���ؾ��� �ؽ�Ʈ�� �Է����ش�
            }
        }
    }

    /// <summary>
    /// ������ �濡 ���� �Լ��Դϴ�
    /// </summary>
    public void JoinSelectedRoom()
    {
        if (selectedRoom == null)
        {
            Debug.Log("���� ���� �������ּ���!");
        }
        else
        {
            PhotonNetwork.JoinRoom(selectedRoom.roomName);
            SceneManager.LoadScene(1);
        }
        
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// ������ ���� ��ȯ�ϴ� �Լ��Դϴ�
    /// </summary>
    /// <returns></returns>
    public RoomData GetSelectedRoom()
    {
        return selectedRoom;
    }

    /// <summary>
    /// ������ ���� �������ִ� �Լ��Դϴ�
    /// </summary>
    /// <param name="_selectedRoom"></param>
    public void SetSelectedRoom(RoomData _selectedRoom)
    {
        selectedRoom = _selectedRoom;
    }

    /// <summary>
    /// ������ ���� ���ֹ����� �Լ��Դϴ�
    /// </summary>
    public void SetRoomNull()
    {
        selectedRoom = null;
    }

    /// <summary>
    /// �� ��ųʸ��� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, RoomData> GetRoomList()
    {
        return rooms;
    }
}
