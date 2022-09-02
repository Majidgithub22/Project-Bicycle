using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveRoomGameplay :MonoBehaviourPunCallbacks
{
    public static int levelNumer;
    public GameObject loading;
    public void Leave() {
        //Debug.Log("hi, i am laving");


        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();

    }



    public override void OnLeftRoom() // when the current player leaves the room

    {

    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        PhotonNetwork.LoadLevel(1);
        loading.SetActive(true);
    }
}
