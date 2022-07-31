using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Random = UnityEngine.Random;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public static PhotonLobby instance;

    public GameObject quickGameButton;
    public GameObject cancelButton;

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
        Debug.Log("You have successfully connected to master.");
        quickGameButton.SetActive(true);
    }

    public void OnQuickGameButtonClicked()
    {
        Debug.Log("QuickGame Button was clicked.");
        quickGameButton.SetActive(false);
        cancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnCancelButtonClicked()
    {
        Debug.Log("Cancel Button was clicked.");
        cancelButton.SetActive(false);
        quickGameButton.SetActive(true);
        Debug.Log("You have disconnected the server.");
        PhotonNetwork.LeaveRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join a random room.");
        CreateRoom();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create a room.");
        CreateRoom();
    }

    private void CreateRoom()
    {
        Debug.Log("Create a room.");
        int randomRoomNumber = Random.Range(0, 100000);
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 2 };
        PhotonNetwork.CreateRoom("Room" + randomRoomNumber, roomOptions);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("We are in a room now.");
    }
}
