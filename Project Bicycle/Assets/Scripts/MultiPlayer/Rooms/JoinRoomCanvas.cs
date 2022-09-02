using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinRoomCanvas : MonoBehaviour
{
    [SerializeField]
    private CreateRoomMenu createRoomMenu;
    [SerializeField]
    private RoomListingMenu roomListingMenu;
    private RoomCanvases roomCanvases;
    public void FirstInitialize(RoomCanvases canvases) {
        roomCanvases = canvases;
        createRoomMenu.FirstInitialize(roomCanvases);
        roomListingMenu.FirstInitialize(canvases);
    }
   
}
