using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class InRoomPlayerCount : MonoBehaviourPunCallbacks
{
    RoomOptions roomOptions = new RoomOptions();

    private int maxPlayers = 4;

    private void playerCount()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            // print(player.)
        }
        
    }
}
