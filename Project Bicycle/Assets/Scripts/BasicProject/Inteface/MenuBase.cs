using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBase :MonoBehaviour
{
   public void SceneLoad(int n) {
        //Loading.SetACtive(true);
        SceneManager.LoadScene(n);
    }
    public void ActivateScreen(GameObject curr,GameObject next) { 
        curr.SetActive(true);
        next.SetActive(true);
    }
}
