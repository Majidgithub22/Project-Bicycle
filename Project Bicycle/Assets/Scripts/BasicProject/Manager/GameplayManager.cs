using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : Singleton<GameplayManager> {
    public Session session;
    [HideInInspector]
    public GameObject Camera;
    private GameObject player;
    private int levelindex;
    public void Start() {
        player = VehicleManager.Instance.currentVehicle;
        Camera.GetComponent<camSwitcher>().cameraTarget= player.transform;
        Time.timeScale = 0;
        LevelManager.Instance.startLevel(
            index: session.levelNo,
           onCompleteCallBack: () => {
               Debug.Log("Show LevelComplete");
               UIManager.Instance.showlevelComplete();
           },
           onGameOverCallBack: () => {
               Debug.Log("Show LevelFail");
               UIManager.Instance.showLevelFail();
           }
            );

    }
    public void home() {
     //   AdsManager.Instance.ShowInterstitialAd();
        SceneManager.LoadScene("MainMenu");
        UIManager.Instance.loading.SetActive(true);
    }
    public void replay() {
    //    AdsManager.Instance.ShowInterstitialAd();
        SceneManager.LoadScene("GamePlay");
    }
    public void next() {
     //   AdsManager.Instance.ShowInterstitialAd();
        if (session.levelNo + 1 < LevelManager.Instance.Levels.Length) session.levelNo++;
        Debug.Log(session.levelNo);
        Debug.Log(session.totalLevel);

        if (session.levelNo >= PlayerPrefs.GetInt("unlocklevels") && session.levelNo < session.totalLevel) {
            Debug.Log("inside lokc");
            PlayerPrefs.SetInt("unlocklevels", PlayerPrefs.GetInt("unlocklevels") + 1);
        }
        Debug.Log("Levels" + PlayerPrefs.GetInt("unlocklevels"));
        SceneManager.LoadScene("GamePlay");
    }

}

