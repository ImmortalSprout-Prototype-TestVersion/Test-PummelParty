using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class CreateRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text _roomName;

    /// <summary>
    /// Create Button 클릭 시 호출되는 이벤트
    /// </summary>
    public void OnClick_CreateButton()
    {
        if (!PhotonNetwork.IsConnected)
            return;

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;

        PhotonNetwork.JoinOrCreateRoom(_roomName.text, options, TypedLobby.Default);
        Debug.Log($"{_roomName.text} 방을 만들었습니다");
    }




    // 룸 생성 성공 시 호출
    public override void OnCreatedRoom()
    {
        Debug.Log("룸 생성 성공");
    }

    // 룸 생성 실패 시 호출
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"룸 생성 실패, 원인: {message}");
    }
}
