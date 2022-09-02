using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class RoomListing : MonoBehaviour
{
    public Session session;
    [SerializeField]
    private Text _text;
    public RoomInfo RoomInfo { get; private set; }
    public void SetRoomInfo(RoomInfo roomInfo) {
        RoomInfo = roomInfo;
        _text.text = roomInfo.Name;
    }
    public void OnClick_Button() {
        //changing the level number of player if its not same..converting string to int to get level number
        //string levelCheck = gameObject.GetComponent<RoomListing>()._text.text;
        //string levelString = levelCheck.Substring(0, 1);
        //Debug.Log("levelString" + levelString);
        //int num;
        //int.TryParse(levelString, out num);
        //if (!(num == session.level)) {
        //    Debug.Log("I am not equal");
        //    session.level = num;
        //}
        PhotonNetwork.JoinRoom(RoomInfo.Name);
    }

}
