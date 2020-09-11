using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class UIClient : MonoBehaviour
{
    [SerializeField] private GameObject[] picturies;
  
    [SerializeField] private Text bornInfo;
    [SerializeField] private Text liveInfo;
    [SerializeField] private Text cellsCount;

    [SerializeField] private Text currentFigure;
    [SerializeField] private Text figuresCount;

    [SerializeField] private Text length;

    private RaiseEventOptions eventOptions;
    private SendOptions sendOptions;

    private int currentModeNum = 0;
    private int currentFigureNum = 0;

    private string[] figures =
    {
        "Glider",
        "Invariant",
        "Oscillator"
    };

    private void Start()
    {
        Parameters.neighborsToBorn = new List<int>();
        Parameters.neighborsToLive = new List<int>();
        eventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
        sendOptions = new SendOptions { Reliability = true };
    }

    public void SwitchMode()
    {
        picturies[currentModeNum].SetActive(false);
        currentModeNum = (currentModeNum + 1) % picturies.Length;
        picturies[currentModeNum].SetActive(true);
    }

    public void NextFigure()
    {
        currentFigureNum = (currentFigureNum + 1) % figures.Length;
        currentFigure.text = figures[currentFigureNum];
    }

    public void PrevFigure()
    {
        currentFigureNum = currentFigureNum - 1 + figures.Length;
        currentFigureNum %= figures.Length;
        currentFigure.text = figures[currentFigureNum];
    }

    public void Continue()
    {
        switch (currentModeNum)
        {
            case 0:
                if (!WriteFigureData()) return;
                break;
            case 1:
                if (!WriteCustomData()) return;
                break;
        }

        SendParameters();
    }

    private bool WriteCustomData()
    {
        Parameters.mode = "Custom";

        if (!Int32.TryParse(length.text, out Parameters.length))
            return false;
        Parameters.length = Mathf.Clamp(Parameters.length, 5, 30);

        if (!Int32.TryParse(cellsCount.text, out Parameters.count))
            return false;
        Parameters.length = Mathf.Clamp(Parameters.count, 0, (int)Mathf.Pow(Parameters.length, 3));

        int[] numbersArray;

        if (!TryStringToInt(bornInfo.text, out numbersArray))
            return false;
        Parameters.neighborsToBorn.Clear();
        Parameters.neighborsToBorn.AddRange(numbersArray);

        if (!TryStringToInt(liveInfo.text, out numbersArray))
            return false ;
        Parameters.neighborsToLive.Clear();
        Parameters.neighborsToLive.AddRange(numbersArray);

        return true;
    }

    private bool WriteFigureData()
    {
        Parameters.mode = currentFigure.text;

        if (!Int32.TryParse(length.text, out Parameters.length))
            return false;
        Parameters.length = Mathf.Clamp(Parameters.length, 5, 30);

        if (!Int32.TryParse(figuresCount.text, out Parameters.count))
            return false;
        Parameters.count = Mathf.Clamp(Parameters.count, 1, 10);

        switch (currentFigureNum)
        {
            case 0:
                Parameters.neighborsToBorn = new List<int>(new int[] { 3 });
                Parameters.neighborsToLive = new List<int>(new int[] { 0 });
                break;
            case 1:
                Parameters.neighborsToBorn = new List<int>(new int[] { 3 });
                Parameters.neighborsToLive = new List<int>(new int[] { 3, 2 });
                break;
            case 2:
                Parameters.neighborsToBorn = new List<int>(new int[] { 5 });
                Parameters.neighborsToLive = new List<int>();
                break;
        }

        return true;
    }

    private void SendParameters()
    {
        PhotonNetwork.RaiseEvent(0, Parameters.mode, eventOptions, sendOptions);
        PhotonNetwork.RaiseEvent(1, Parameters.length, eventOptions, sendOptions);
        PhotonNetwork.RaiseEvent(2, Parameters.count, eventOptions, sendOptions);
        PhotonNetwork.RaiseEvent(3, Parameters.neighborsToBorn.ToArray(), eventOptions, sendOptions);
        PhotonNetwork.RaiseEvent(4, Parameters.neighborsToLive.ToArray(), eventOptions, sendOptions);
    }

    private bool TryStringToInt(string text, out int[] arr)
    {
        string[] tmpNumbers = text.Split();
        arr = new int[tmpNumbers.Length];
        for (int i = 0; i < tmpNumbers.Length; i++)
            if (!Int32.TryParse(tmpNumbers[i], out arr[i]))
                return false;
        return true;
    }
}
