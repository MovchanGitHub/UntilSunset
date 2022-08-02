using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.Mathematics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PhotonRoomCustomMatch : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public static PhotonRoomCustomMatch instance;
    private PhotonView photonView;

    public GameObject lobby;
    public GameObject room;
    public Transform playersPanel;
    public GameObject playerListingPrefab;
    public GameObject startButton;

    private int playersInRoom;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
            instance = this;
        }
        
        DontDestroyOnLoad(this.gameObject);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }
    
    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("We are now in a room");
        
        lobby.SetActive(false);
        room.SetActive(true);

        if (PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
        }

        ClearPlayerListings();
        ListPlayers();

        playersInRoom = PhotonNetwork.PlayerList.Length;

        Debug.Log("There are " + playersInRoom + "/3 players in the room.");

        if (playersInRoom == 3 && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
    }

    private void ClearPlayerListings()
    {
        for (int i = playersPanel.childCount - 1; i >= 0; i--)
        {
            Destroy(playersPanel.GetChild(i).gameObject);
        }
    }

    private void ListPlayers()
    {
        if (PhotonNetwork.InRoom)
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                GameObject tempListing = Instantiate(playerListingPrefab, playersPanel);
                Text tempText = tempListing.transform.GetChild(0).GetComponent<Text>();
                tempText.text = player.NickName;
            }
        }
    }

    public void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("A new player has joined the room.");
        ClearPlayerListings();
        ListPlayers();
        
        playersInRoom++;

        Debug.Log("There are " + playersInRoom + "/3 players in the room.");
        
        if (playersInRoom == 3 && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
    }

    public void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log(otherPlayer.NickName + " has left the game.");
        playersInRoom--;
        ClearPlayerListings();
        ListPlayers();
    }

    public void StartGame()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(1);
    }

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
        {
            //photonView.RPC(nameof(RPC_CreatePlayer), RpcTarget.All);
            PhotonNetwork.Instantiate(Path.Combine("Prefabs", "PNPlayer"), 
                new Vector3(0f, Random.Range(0, 2), 0f), quaternion.identity, 0);
        }
    }

    [PunRPC]
    private void RPC_CreatePlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("Prefabs", "PNPlayer"), 
            new Vector3(0f, Random.Range(0, 2), 0f), quaternion.identity, 0);
    }
}
