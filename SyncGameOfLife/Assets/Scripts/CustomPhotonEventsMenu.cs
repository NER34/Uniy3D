using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPhotonEventsMenu : MonoBehaviour, IOnEventCallback
{
    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            case 0:
                Parameters.mode = (string)photonEvent.CustomData;
                break;
            case 1:
                Parameters.length = (int)photonEvent.CustomData;
                break;
            case 2:
                Parameters.count = (int)photonEvent.CustomData;
                break;
            case 3:
                Parameters.neighborsToBorn = new List<int>();
                Parameters.neighborsToBorn.AddRange((int[])photonEvent.CustomData);
                break;
            case 4:
                Parameters.neighborsToLive = new List<int>();
                Parameters.neighborsToLive.AddRange((int[])photonEvent.CustomData);
                PhotonNetwork.LoadLevel("GameScene");
                break;
            case 7:
                Application.Quit();
                break;
        }
    }

    public void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
}
