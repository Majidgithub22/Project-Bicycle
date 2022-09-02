using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaveRoomMenu : MonoBehaviourPunCallbacks {
    public RoomCanvases roomCanvases;
    public Text roomleavetext;
    public void FirstInitialize(RoomCanvases canvases) {
        roomCanvases = canvases;
    }
    public void OnCLick_LeaveRoom() {
        PhotonNetwork.LeaveRoom(true);
        roomCanvases.CurrentRoomCanvas.Hide();
        if (PhotonNetwork.IsMasterClient) {
            roomleavetext.text = "Server Left";
            Debug.Log("Server Left");
        } else {
            roomleavetext.text = "Client Left";
            Debug.Log("Client Left");
        }
    }
}
