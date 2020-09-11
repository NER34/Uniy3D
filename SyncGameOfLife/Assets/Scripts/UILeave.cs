using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class UILeave : MonoBehaviour
{
    public void Leave()
    {
        PhotonNetwork.RaiseEvent
            (
                7, null, new RaiseEventOptions { Receivers = ReceiverGroup.All },
                new SendOptions { Reliability = true }
            );
    }
}
