using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomCustomPropertyGenerator : MonoBehaviour {
    [SerializeField]
    private Text text;
    private ExitGames.Client.Photon.Hashtable _myCustomProperties = new ExitGames.Client.Photon.Hashtable();
    private void SetCustomerNumber() {
        System.Random rnd = new System.Random();
        int result = rnd.Next(0, 99);
        text.text = result.ToString();

        _myCustomProperties["RandomNumber"] = result;
        PhotonNetwork.LocalPlayer.CustomProperties = _myCustomProperties;
    }
    public void onCLick_ButtonRandom() {
        SetCustomerNumber();
    }

}
