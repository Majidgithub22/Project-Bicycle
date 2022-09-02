using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestConnect : MonoBehaviourPunCallbacks {

    public CreateRoomMenu createRoom;
    public CustomPropertiesPlayer customProperties;
    public Session session;
    public ServerSettings serverSettings;
    public GameObject RefreshUI;
    public GameObject TestConnectScreen;
    public Text displayInfo;
    private void Start() {
        RefreshUI.SetActive(false);
        if (!PhotonNetwork.IsConnected) {
            displayInfo.text = "Wait! Connecting to Server";
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.GameVersion = "1.0";
            displayInfo.text= serverSettings.DevRegion;
            PhotonNetwork.NickName = "zunaira";
            session.mode = true;
            customProperties.SetLevel(session.levelNo);
           // serverSettings.DevRegion = "us;";
            // session.PlayerName = PlayerPrefs.GetString("DogName" + GameConstants.GetContant("FOR_RACE"));
            PhotonNetwork.ConnectUsingSettings();
        }
        if (Application.internetReachability == NetworkReachability.NotReachable) {
            displayInfo.text ="Error. Check internet connection!";
            RefreshUI.SetActive(true);
        }
        if (PhotonNetwork.InLobby) {
            createRoom.OnClick_CreateRoom(); 
        }
        
       
    }
    public void RefreshButton() {
        Start();
    }
    public override void OnConnectedToMaster() {
        if (!PhotonNetwork.InLobby) {
            displayInfo.text = "Joining Lobby........ ";
            PhotonNetwork.ConnectToRegion("us;");
            PhotonNetwork.JoinLobby();
        }
    }
    public override void OnJoinedLobby() {
        displayInfo.text = "Connected to Server ";
        createRoom.OnClick_CreateRoom();
        TestConnectScreen.SetActive(false);
    }
    public override void OnDisconnected(DisconnectCause cause) {
        // info.text = "disconnceted from server for reason " + cause.ToString();
        // Debug.Log("disconnceted from server for reason " + cause.ToString());
    }
   
}
