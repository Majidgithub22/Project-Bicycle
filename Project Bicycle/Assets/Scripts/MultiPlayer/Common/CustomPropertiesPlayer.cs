using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomPropertiesPlayer : MonoBehaviour
{
    public Session session;
    private ExitGames.Client.Photon.Hashtable _myCustomProperties =
        new ExitGames.Client.Photon.Hashtable(); 
    public void SetCustomPropertyies() {
        _myCustomProperties["PlayerOwnerName"] = PlayerPrefs.GetString("OwnerName");
        PhotonNetwork.LocalPlayer.CustomProperties =_myCustomProperties;
    }
    public void SetLevel(int levelNo) {
        _myCustomProperties["GameLevel"] = levelNo;
        PhotonNetwork.LocalPlayer.CustomProperties = _myCustomProperties;
    }
   
    public void SetStats(string stats,float s2) {
        _myCustomProperties[stats] = s2;
        PhotonNetwork.LocalPlayer.CustomProperties = _myCustomProperties;
    }

}
