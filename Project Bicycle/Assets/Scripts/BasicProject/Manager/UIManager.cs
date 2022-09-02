using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public GameObject
        levelComplete,
        levelFail,
        pauseDlg,
        startDlg,
        loading
;
  public void showlevelComplete() {
        levelComplete.SetActive(true);
    }
    public void showLevelFail() {
        levelFail.SetActive(true);
    }
    public void pause() {
        Time.timeScale = 0;
       // AdsManager.Instance.ShowInterstitialAd();
       pauseDlg.SetActive(true);
    }
    public void resume() {
        Time.timeScale = 1;
       // AdsManager.Instance.ShowInterstitialAd();
        pauseDlg.SetActive(false);
    }
    public void start() {
       // GameplayManager.Instance.sounds.SetActive(true);
        Time.timeScale = 1;
        startDlg.SetActive(false);
      //  AdsManager.Instance.ShowInterstitialAd();
    }

}
