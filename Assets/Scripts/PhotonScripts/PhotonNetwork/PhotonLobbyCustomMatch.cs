using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Random = UnityEngine.Random;

public class PhotonLobbyCustomMatch : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    public static PhotonLobbyCustomMatch instance;

    public string roomName;
    public int roomSize;
    public GameObject roomListingPrefab;
    public Transform roomsPanel;

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Debug.Log("Try to connect to the server.");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("You have successfully connected to the Photon master server.");
        PhotonNetwork.AutomaticallySyncScene = true;        
        PhotonNetwork.NickName = "Player" + Random.Range(1, 100);

    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        RemoveRoomListings();
        foreach (RoomInfo room in roomList)
        {
            ListRoom(room);
        }
    }

    private void RemoveRoomListings()
    {
        for (int i = roomsPanel.childCount - 1; i >= 0; i--)
        {
            Destroy(roomsPanel.GetChild(i).gameObject);
        }
    }

    private void ListRoom(RoomInfo room)
    {
        if (room.IsOpen && room.IsVisible)
        {
            GameObject tempListing = Instantiate(roomListingPrefab, roomsPanel);
            RoomButton tempButton = tempListing.GetComponent<RoomButton>();
            tempButton.roomName = room.Name;
            tempButton.SetRoom();
        }
    }
    
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create a room, try use another room name.");
    }

    public void CreateRoom()
    {
        Debug.Log("Create a room.");
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 3 };
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public void OnRoomNameChanged(string nameIn)
    {
        roomName = nameIn;
    }

    public void JoinLobbyOnClick()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
    }
}
