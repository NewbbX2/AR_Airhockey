using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class NetworkTest : MonoBehaviourPunCallbacks
{
    public string gameVersion = "0.01";
    public byte maxPlayer = 2;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.NickName = Random.Range(1, 100).ToString();
        PhotonNetwork.ConnectUsingSettings();        
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to server");        
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed join room");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayer });
        Debug.Log("Create new!");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Join!");
        SceneManager.LoadScene(1);
    }    

    public void OnButtonDown()
    {
        PhotonNetwork.JoinRandomRoom();
    }
}
