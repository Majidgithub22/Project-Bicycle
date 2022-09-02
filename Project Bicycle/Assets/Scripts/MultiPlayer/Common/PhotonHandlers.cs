using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PhotonHandlers : MonoBehaviourPunCallbacks {
    
    public Session session;
    public GameObject playerListing;

    [SerializeField]
    private Transform _content;
    private string roomname;

    private void Awake() {
        DontDestroyOnLoad(this.transform);
        PhotonNetwork.SendRate = 30;
        PhotonNetwork.SerializationRate = 20;
        //PhotonNetwork.NickName = session.player.GetComponent<DogStats>().name;

    }
    private void AddPlayerToListning(Player player) {
        //foreach (KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players) {
        //    Instantiate(playerListing, _content);
        //}
        foreach (Player p in PhotonNetwork.PlayerList) {
            Instantiate(playerListing, _content);
            playerListing.GetComponent<Text>().text = p.NickName;
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer) {
        AddPlayerToListning(newPlayer);

    }
    public override void OnPlayerLeftRoom(Player otherPlayer) {
        Debug.Log("player left room");
    }
    public override void OnJoinedLobby() {
        Debug.Log("In Lobby");
        //PhotonNetwork.JoinRandomOrCreateRoom();
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 8;
        PhotonNetwork.CreateRoom(roomname, options, TypedLobby.Default);
    }
    public override void OnJoinedRoom() {
       // LobbyUIManager.Instance.ActivatePlayerScreen();
        foreach (Player p in PhotonNetwork.PlayerList) {
            GameObject go = Instantiate(playerListing, _content);
            go.GetComponent<Text>().text = p.NickName;
        }
    }
    private void SpawnPlayer() {
      //  PhotonNetwork.Instantiate("" + session.player.name, transform.position, transform.rotation, 0);
    }
    public void StartRace() {
        if (PhotonNetwork.IsMasterClient) {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel(1);
            PhotonNetwork.AutomaticallySyncScene = true;
        }
    }
}
