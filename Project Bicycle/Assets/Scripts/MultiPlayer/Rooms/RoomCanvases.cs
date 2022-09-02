using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomCanvases : MonoBehaviour
{
    [SerializeField]
    private JoinRoomCanvas _joinRoomCanvas;
    public GameObject loading;
    public JoinRoomCanvas JoinRoomCanvas { get { return _joinRoomCanvas; } }

    [SerializeField]
    private CurrentRoomCanvas _currentRoomCanvas;
    public CurrentRoomCanvas CurrentRoomCanvas { get { return _currentRoomCanvas; } }
    public void Back() {
        SceneManager.LoadScene(0);
        loading.SetActive(true);
    }
}
