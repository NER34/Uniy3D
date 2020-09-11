using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class UIConnecting : MonoBehaviourPunCallbacks
{
    [SerializeField] private List<GameObject> sceens;
    [SerializeField] private Text serverStatus;
    [SerializeField] private Text connectionStatus;

    private bool serverOnline = false;

    void Start()
    {
        PhotonNetwork.NickName = "Player " + Random.Range(1000, 9999);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Connected to Lobby");
        connectionStatus.text = "Ready";
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Server is ready");

        sceens[1].SetActive(true);
        gameObject.SetActive(false);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined to server");

        sceens[0].SetActive(true);
        gameObject.SetActive(false); 
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo room in roomList)
            if (room.Name == "Sample")
            {
                serverOnline = true;
                return;
            }
        serverOnline = false;
    }

    public void Update()
    {
        if (serverOnline)
            serverStatus.text = "Server Online";
        else
            serverStatus.text = "Server Offline";
    }

    public void CreateServer()
    {
        if (serverOnline)
            return;

        PhotonNetwork.CreateRoom("Sample");
    }

    public void JoinServer()
    {
        if (!serverOnline)
            return;

        PhotonNetwork.JoinRoom("Sample");
    }

    public void Leave()
    {
        Application.Quit();
    }
}
