using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun.Demo.PunBasics;
using System;

public class CustomPhotonEventsGame : MonoBehaviour, IOnEventCallback
{
    [SerializeField] private GameObject gameManager;

    private void Awake()
    {
        PhotonPeer.RegisterType(typeof(Vector3Int), 221, SerializeVector3Int, DeserializeVector3Int);
    }

    public void OnEvent(EventData photonEvent)
    {
        List<Vector3Int> changedCells;
        switch (photonEvent.Code)
        {
            case 6:
                gameManager.GetComponent<CellGenerator>().Ready();
                break;
            case 7:
                Application.Quit();
                break;
            case 8:
                changedCells = new List<Vector3Int>((Vector3Int[])photonEvent.CustomData);
                gameManager.GetComponent<CellSpawner>().UpdateCells(changedCells);
                break;
        }
    }

    public static byte[] SerializeVector3Int(object obj)
    {
        Vector3Int vector = (Vector3Int)obj;
        byte[] data = new byte[12];

        BitConverter.GetBytes(vector.x).CopyTo(data, 0);
        BitConverter.GetBytes(vector.y).CopyTo(data, 4);
        BitConverter.GetBytes(vector.z).CopyTo(data, 8);

        return data;
    }

    public static object DeserializeVector3Int(byte[] data)
    {
        Vector3Int vector = new Vector3Int();

        vector.x = BitConverter.ToInt32(data, 0);
        vector.y = BitConverter.ToInt32(data, 4);
        vector.z = BitConverter.ToInt32(data, 8);

        return vector;
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
