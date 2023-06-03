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
            if (roomInfo.RemovedFromList) // 방이 삭제된 것이라면
            {
                if (rooms.ContainsKey(roomInfo.Name)) // 딕셔너리에 등록된 방이라면
                {
                    Destroy(rooms[roomInfo.Name].gameObject); // UI상에서 방을 삭제한다
                    rooms.Remove(roomInfo.Name); // 딕셔너리에서 방의 Key를 삭제한다
                }
            }
            else // 삭제가 아닌 변경사항이 발생했다면
            {
                RoomData newRoom = null; // 새로운 UI 방을 저장할 변수입니다

                if (rooms.ContainsKey(roomInfo.Name)) // 해당 방이 이미 딕셔너리에 존재한다면
                {
                    return; // 아무것도 안하고 함수를 종료한다
                }

                newRoom = Instantiate(roomData, roomListPosition); 
                // roomListPosition을 부모로 하는 roomData를 생성해준다

                rooms.Add(roomInfo.Name, newRoom); // 딕셔너리에 새 방을 넣어준다

                newRoom.SetRoomInfo(roomInfo); // newRoom의 information에 roomInfo를 할당한다
                newRoom.SetRoomText(roomInfo.Name, roomInfo.PlayerCount, roomInfo.MaxPlayers); // 방이 표시해야할 텍스트를 입력해준다
            }
        }
    }

    /// <summary>
    /// 선택한 방에 들어가는 함수입니다
    /// </summary>
    public void JoinSelectedRoom()
    {
        if (selectedRoom == null)
        {
            Debug.Log("방을 먼저 선택해주세요!");
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
    /// 선택한 방을 반환하는 함수입니다
    /// </summary>
    /// <returns></returns>
    public RoomData GetSelectedRoom()
    {
        return selectedRoom;
    }

    /// <summary>
    /// 선택한 방을 지정해주는 함수입니다
    /// </summary>
    /// <param name="_selectedRoom"></param>
    public void SetSelectedRoom(RoomData _selectedRoom)
    {
        selectedRoom = _selectedRoom;
    }

    /// <summary>
    /// 선택한 방을 없애버리는 함수입니다
    /// </summary>
    public void SetRoomNull()
    {
        selectedRoom = null;
    }

    /// <summary>
    /// 룸 딕셔너리를 반환하는 함수
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, RoomData> GetRoomList()
    {
        return rooms;
    }
}
