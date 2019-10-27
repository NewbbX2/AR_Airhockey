using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class NetworkTest : MonoBehaviourPunCallbacks
{
    public string GameVersion = "0.01";
    public byte MaxPlayer = 2;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.GameVersion = GameVersion;
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
        RoomOptions roomOption = new RoomOptions();
        roomOption.MaxPlayers = 2;
        roomOption.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { {"1", false }, {"2", false} };
        PhotonNetwork.CreateRoom(null, roomOption);
        Debug.Log("Create new!");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Join!");
        PhotonNetwork.AutomaticallySyncScene = true;
        SceneManager.LoadScene(1);          
    }    

    public override void OnCreatedRoom()
    {
        
    }

    public void OnButtonDown()
    {
        PhotonNetwork.JoinRandomRoom();
    }
}
