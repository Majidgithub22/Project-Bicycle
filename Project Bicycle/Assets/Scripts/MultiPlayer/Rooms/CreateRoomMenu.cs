using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class CreateRoomMenu : MonoBehaviourPunCallbacks 
{
    public Session session;
    [SerializeField]
    public RoomCanvases canvas;
    public PlayerListingMenu playerListingMenu;
    private Text _roomName;
    public void OnClick_CreateRoom() {
        if (!PhotonNetwork.IsConnected) return;
        RoomOptions options = new RoomOptions();
        options.MaxPlayers =4;
        PhotonNetwork.JoinOrCreateRoom("Race", options, TypedLobby.Default);
    }
    public void FirstInitialize(RoomCanvases canvases) {
        canvas = canvases;
    }
    public override void OnJoinedRoom() {
       
        if (!PhotonNetwork.IsMasterClient)
        {
            playerListingMenu.GetCurrentRoomPlayer();
        }
    }
    public override void OnCreatedRoom() {
        Debug.Log("IN rOOM");
        playerListingMenu.GetCurrentRoomPlayer();
       
    }
    public override void OnCreateRoomFailed(short returnCode, string message) {
        Debug.Log("Room Creation failed."+message, this);
    }
 

}