using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class RoomListingMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform _content;
    [SerializeField]
    private RoomListing _roomListing;
    private List<RoomListing> _listing = new List<RoomListing>();

    public RoomCanvases RoomCanvases;
    public void FirstInitialize(RoomCanvases canvases) {
        RoomCanvases = canvases;
    }
    public override void OnJoinedRoom() {
        RoomCanvases.CurrentRoomCanvas.Show();
      //  _content.DestroyChildren();
      //  PhotonNetwork.Instantiate("Player", new Vector3(0, 0, 0), Quaternion.identity);
        //RoomCanvases.gameObject.SetActive(false);
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList) {
        //Debug.Log("rom");
        foreach(RoomInfo info in roomList) {
            if (info.RemovedFromList) {
                int index = _listing.FindIndex(x => x.RoomInfo.Name == info.Name);
                    if (index != -1) {
                        Destroy(_listing[index].gameObject);
                        _listing.RemoveAt(index);
                }
            } else {
      //  Debug.Log("rom");
                RoomListing listing = Instantiate(_roomListing, _content);
                if (listing != null) {
                    listing.SetRoomInfo(info);
                    _listing.Add(listing);
                } 
            }
        }
    }
}
