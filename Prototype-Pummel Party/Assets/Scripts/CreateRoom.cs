using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class CreateRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text _roomName;

    /// <summary>
    /// Create Button Ŭ�� �� ȣ��Ǵ� �̺�Ʈ
    /// </summary>
    public void OnClick_CreateButton()
    {
        if (!PhotonNetwork.IsConnected)
            return;

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;

        PhotonNetwork.JoinOrCreateRoom(_roomName.text, options, TypedLobby.Default);
        Debug.Log($"{_roomName.text} ���� ��������ϴ�");
    }




    // �� ���� ���� �� ȣ��
    public override void OnCreatedRoom()
    {
        Debug.Log("�� ���� ����");
    }

    // �� ���� ���� �� ȣ��
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"�� ���� ����, ����: {message}");
    }
}
