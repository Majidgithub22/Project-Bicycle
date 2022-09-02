using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerListingMenu : MonoBehaviourPunCallbacks
{

    public GameObject oading;
    [SerializeField]
    private Transform _content;
    [SerializeField]
    private PlayerListing _playerListing;
    public CreateRoomMenu _createRoomMenu;
    public CustomPropertiesPlayer customProperties;
    private RoomCanvases roomCanvases;
    private PhotonView photon;
    private List<PlayerListing> _listing = new List<PlayerListing>();
    private void Awake() {
        photon = GetComponent<PhotonView>();
    }
    public void FirstInitialize(RoomCanvases canvases) {
        roomCanvases = canvases;
    }
    public void GetCurrentRoomPlayer() {
       if (!PhotonNetwork.IsConnected)
            return;
        if (PhotonNetwork.CurrentRoom == null || PhotonNetwork.CurrentRoom.Players == null)
            return;
        foreach(KeyValuePair<int, Photon.Realtime.Player> playerInfo in PhotonNetwork.CurrentRoom.Players) {
            AddPlayerToListning(playerInfo.Value);
        }
    }


    public override void OnLeftRoom() {
        _content.DestroyChildren();
        PhotonNetwork.Disconnect();
    }
    private void AddPlayerToListning(Photon.Realtime.Player player) {
        Debug.Log((int)PhotonNetwork.MasterClient.CustomProperties["GameLevel"]);
        photonView.RPC("SetLevelforAllPlayers", RpcTarget.All, (int)PhotonNetwork.MasterClient.CustomProperties["GameLevel"]);
        PlayerListing listing = Instantiate(_playerListing, _content);
        if (listing != null) {
            listing.SetPlayerInfo(player);
            _listing.Add(listing);
        }
        StartCoroutine(loading());
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer) {
        AddPlayerToListning(newPlayer);
    }


    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer) {

       // int index = _listing.FindIndex(x => x.Player.NickName == otherPlayer.NickName);

        int index = 0;
        foreach(PlayerListing playerListing in _listing)
        {
            if(playerListing.playerName == otherPlayer.NickName)
            {
                break;
            }
            index++;
        }

        if (index != -1) {
            Destroy(_listing[index].gameObject);
            _listing.RemoveAt(index);
        }
    }
    [PunRPC]
    private void SetLevelforAllPlayers(int n) {
        customProperties.SetLevel(n);
        customProperties.session.levelNo=n;
        }
    [PunRPC]
    private void ShowLoading() {
        oading.SetActive(true);
    }

    public void OnCLick_StartGame() {
        if (PhotonNetwork.IsMasterClient) {

            photon.RPC("ShowLoading", RpcTarget.AllBuffered);
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.LoadLevel(11);
            Debug.Log("loadingScene");
        }
    }
 

    public void backtoOfflineMode()
    {
     //   Instantiate(DoracScreenManager.Instance.Loading);
        PhotonNetwork.LeaveRoom();
    }
    public Image Loading;

    [PunRPC]
    public void SetBar(float b) {
        Loading.fillAmount = Loading.fillAmount + 0.01f;
    }
    IEnumerator loading() {
        while (Loading.fillAmount < 1) {
            float b = Loading.fillAmount + 0.1f;
            photonView.RPC("SetBar", RpcTarget.AllBuffered, b);


            yield return new WaitForSeconds(0.2f);
        }
        if (Loading.fillAmount >= 1) {
            StartGame();
        }

    }

    private void StartGame() {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.LoadLevel("GamePlay");
    }
}
