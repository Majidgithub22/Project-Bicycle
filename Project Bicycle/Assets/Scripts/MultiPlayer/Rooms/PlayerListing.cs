using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListing : MonoBehaviour {
    [HideInInspector]
    public string playerName;
   // public Photon.Realtime.Player Player;
    public Text _text;
   // public Session session;
    public void SetPlayerInfo(Photon.Realtime.Player player) {
        _text.text = player.NickName;
        playerName = player.NickName;
    }
}
