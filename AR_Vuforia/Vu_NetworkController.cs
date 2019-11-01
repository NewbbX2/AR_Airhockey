using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Vu_NetworkController : MonoBehaviourPunCallbacks
{
    #region 외부변수
    public string GameVersion = "1.0"; // 게임 버전
    public GameObject TablePrefab; // 게임 테이블 프리팹
    #endregion

    #region 내부변수
    private Vu_UIController UICon;
    private BackgroundSound BGM;
    private RoomOptions RoomOps = new RoomOptions();
    #endregion
    private void Awake()
    {
        //룸 옵션
        RoomOps.MaxPlayers = 2;
        RoomOps.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "1", false }, { "2", false }};

        PhotonNetwork.GameVersion = GameVersion;
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        UICon = FindObjectOfType<Vu_UIController>();
        UICon.MessagePrint("Connecting to server...");
        BGM = FindObjectOfType<BackgroundSound>();

        if (!PhotonNetwork.ConnectUsingSettings()) UICon.MessagePrint("Fail to connect to server. Plese check internet");
    }
    
    //포톤 연결시
    public override void OnConnectedToMaster()
    {
        UICon.MessagePrint("Connected to server!");
        PhotonNetwork.JoinLobby();
    }


    //로비 입장시
    public override void OnJoinedLobby()
    {
        UICon.MessagePrint("Welcome! Capture Image to start!");
    }

    //방 입장시
    public override void OnJoinedRoom()
    {
        UICon.RoomLabelText.text = PhotonNetwork.CurrentRoom.Name;
        UICon.RoomLabelText.GetComponentInParent<Image>().enabled = true;
        UICon.MessagePrint("Entered");
        
        InstantiateTable();
    }

    //룸 입장 실패시 방생성
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        UICon.MessagePrint("No room Available. Create New room");
        string roomNum = "Room " + Random.Range(1, 9999);
        PhotonNetwork.CreateRoom(roomNum, RoomOps);
    }

    //룸 입장
    public bool EnterToRoom()
    {
        if (!PhotonNetwork.IsConnectedAndReady) return true;
        UICon.MessagePrint("Matching...");
        PhotonNetwork.JoinRandomRoom();
        BGM.PlayPlayMusic();
        return false;
    }

    public void InstantiateTable()
    {
        Instantiate(TablePrefab, Vector3.zero, Quaternion.identity);
        //PhotonNetwork.Instantiate(TablePrefab.name, Vector3.zero, Quaternion.identity);        
    }
}
