using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class CellSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cellObj;
    [SerializeField] private GameObject boxObj;

    private GameObject[,,] cells;

    private RaiseEventOptions eventOptions;
    private SendOptions sendOptions;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
            return;

        cells = new GameObject[Parameters.length, Parameters.length, Parameters.length];

        SpawnCells();

        eventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
        sendOptions = new SendOptions { Reliability = true };
    }

    public void SpawnCells()
    {
        GameObject box = Instantiate(boxObj, new Vector3(Parameters.length / 2f, Parameters.length / 2f, Parameters.length / 2f), Quaternion.identity);
        box.transform.localScale = new Vector3(Parameters.length, Parameters.length, Parameters.length);

        for (int x = 0; x < Parameters.length; x++)
            for (int y = 0; y < Parameters.length; y++)
                for (int z = 0; z < Parameters.length; z++)
                {
                    cells[x, y, z] = Instantiate(cellObj, new Vector3(x, y, z), Quaternion.identity);
                    cells[x, y, z].SetActive(false);
                }

        PhotonNetwork.RaiseEvent(6, null, eventOptions, sendOptions);
    }

    public void UpdateCells(List<Vector3Int> changedCells)
    {
        int edge = Parameters.length;
        foreach (Vector3Int vector in changedCells)
        {
            GameObject cell = cells[vector.x % edge, vector.y % edge, vector.z % edge];
            cell.SetActive(!cell.activeSelf);
        }

        PhotonNetwork.RaiseEvent(6, null, eventOptions, sendOptions);
    }
}
