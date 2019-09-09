using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using UnityEngine.Networking;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

using TMPro;

#if UNITY_EDITOR
using Input = GoogleARCore.InstantPreviewInput;
#endif

/// <summary>
/// 플레이어가 네트워크 플레이가 가능하게 연결하기 위한 클래스입니다. Cloud Anchor의 네트워크는 CloudAnchorController를 참고하십시오.
/// </summary>

/*
#pragma warning disable 618
public class AnchorNetworkManager : NetworkManager
#pragma warning restore 618

{
    #region 액션 선언 
    public event Action OnClientConnected; // 서버에 연결되면 호출
    public event Action OnClientDisconnected; // 서버에서 연결 해제되면 호출
    #endregion

#pragma warning disable 618
    public override void OnClientConnect(NetworkConnection conn)
#pragma warning restore 618
    {
        base.OnClientConnect(conn);
        Debug.Log("Connected to Server : " + conn.lastError);
        if(OnClientConnected != null)
        {
            OnClientConnected();
        }
    }

#pragma warning disable 618
    public override void OnClientDisconnect(NetworkConnection conn)
#pragma warning restore 618
    {
        base.OnClientDisconnect(conn);
        Debug.Log("Disconnected from Server" + conn.lastError);
        if(OnClientDisconnected != null)
        {
            OnClientDisconnected();
        }
    }

}*/

public class AnchorNetworkManager : MonoBehaviourPunCallbacks
{
    #region 외부 파라미터(Inspector용)
    public string GameVersion = "1.0"; // 포톤 게임 버전
    public Text SnackbarText; // 스낵바 텍스트
    public Text NoRoomMessage; // 룸 없음 메세지
    public Button CreateRoomButton; // 방 생성 버튼
    public GameObject RoomButton; // 룸 버튼
    public Transform GridTransform; // 룸버튼 올려놓을 그리드 트랜스폼
    #endregion

    #region 내부 파라미터
    private RoomOptions _RoomOptions = new RoomOptions();
    #endregion

    #region 포톤 옵션
    private void Awake()
    {
        // 룸 옵션
        _RoomOptions.MaxPlayers = 2;
        _RoomOptions.PublishUserId = true;

        //포톤 서버 옵션
        PhotonNetwork.GameVersion = GameVersion;
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    #endregion

    private void Start()
    {
        //포톤 연결
        PhotonNetwork.ConnectUsingSettings();
    }

    private void Update()
    {
        //앱 생명주기
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
        var _SleepTimeout = SleepTimeout.SystemSetting;
        Screen.sleepTimeout = _SleepTimeout;        
    }

    #region 포톤 콜백
    public override void OnConnectedToMaster()
    {
        ShowDebugMessage("Connected to server");
        PhotonNetwork.JoinLobby();
    }


    public override void OnJoinedLobby()
    {
        ShowDebugMessage("Welcome!");
        CreateRoomButton.interactable = true;
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.CurrentRoom);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log(PhotonNetwork.CurrentRoom);
        SceneManager.LoadScene(1);
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left room");
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        ShowDebugMessage("Master Client leave room. Exit room in 5 sec...");
        Invoke("ExitRoom", 5.0f);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Room list is updated");
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Room"))
        {
            NoRoomMessage.enabled = true;
            Destroy(obj);
        }

        foreach (RoomInfo roomInfo in roomList)
        {
            NoRoomMessage.enabled = false;
            GameObject _Room = Instantiate(RoomButton, GridTransform);
            //Debug.Log("Room Button added");
            _Room.GetComponentInChildren<TextMeshProUGUI>().text = roomInfo.Name;
            _Room.GetComponent<Button>().onClick.AddListener
                (
                    delegate
                    {
                        OnClickRoom(roomInfo.Name);
                    }
                );

        }
    }

    
    #endregion
    

    public void OnClickCreateRoom()
    {
        string Roomname = "Room " + Random.Range(1, 9999);
        PhotonNetwork.CreateRoom(Roomname, _RoomOptions);
    }

    public void OnClickRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName, null);
        ShowDebugMessage("Enter to room...");
    }

    public void ExitRoom()
    {
        SnackbarText.text = string.Empty;
        PhotonNetwork.LeaveRoom();
    }

    public void ShowDebugMessage(string debugMessage)
    {
        SnackbarText.text = debugMessage;
    }
}
