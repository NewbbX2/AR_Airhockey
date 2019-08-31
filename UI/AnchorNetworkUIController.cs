using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using UnityEngine.UI;

public class AnchorNetworkUIController : MonoBehaviour
{
    #region 1.Public parameter for Inspector
    public Canvas LobbyScreen; // 로비 스크린
    public Text SnackbarText; // 스낵바 문구
    public GameObject CurrentRoomLabel; // 현재 룸 보여줄 라벨
    public GameObject temp; // 하키 경기장 컨트롤러 자리
    public GameObject RoomListPanel; // 입장 가능 룸 보여줄 패널
    public Text NoPreviousRoomsText; // 룸이 없다는 메세지
    public GameObject JoinRoomListRowPrefab; // 룸리스트 띄울 한줄짜리 프리팹
    #endregion

    #region 2.Private parameter
    private const int PageSize = 5; // 페이지 사이즈
    private string CurrentRoomNum; // 현재 룸 번호
    private List<GameObject> JoinRoomButtonPool = new List<GameObject>(); // 버튼 풀
    private AnchorNetworkManager NetworkManager;
    #endregion

    #region Touch Event
    private void Awake()
    {
        //방 참가 버튼 초기화
        for(int i = 0; i < PageSize; i++)
        {
            GameObject button = Instantiate(JoinRoomListRowPrefab);
            button.transform.SetParent(RoomListPanel.transform, false);
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -100 -(200 * i));
            button.GetComponentInChildren<Text>().text = string.Empty;
            JoinRoomButtonPool.Add(button);
        }

        NetworkManager = GetComponent<AnchorNetworkManager>();
        NetworkManager.StartMatchMaker();
        NetworkManager.matchMaker.ListMatches(
            startPageNumber: 0,
            resultPageSize: PageSize,
            matchNameFilter: string.Empty,
            filterOutPrivateMatchesFromResults: false,
            eloScoreTarget: 0,
            requestDomain: 0,
            callback: _OnMatchList
            ) ;
        _ChangeLobbyUIVisibility(true);
    }

    public void OnCreateRoomClicked()
    {
        NetworkManager.matchMaker.CreateMatch(NetworkManager.matchName, NetworkManager.matchSize, true, string.Empty, string.Empty, string.Empty, 0, 0, _OnMatchCreate);
    }
    
    public void OnRefreshRoomListClicked()
    {
        NetworkManager.matchMaker.ListMatches(
            startPageNumber: 0,
            resultPageSize: PageSize,
            matchNameFilter: string.Empty,
            filterOutPrivateMatchesFromResults: false,
            eloScoreTarget: 0,
            requestDomain: 0,
            callback: _OnMatchList
            );
    }

    public void OnAnchorInstantiated(bool isHost)
    {
        if (!isHost)
        {
            SnackbarText.text = "Hosting Cloud Anchor...";
        }
        else
        {
            SnackbarText.text = "Cloud Anchor added to session. Try to resolve anchor.";
        }
    }

    public void OnAnchorHosted(bool success, string response)
    {
        if (success)
        {
            SnackbarText.text = "Cloud Anchor successfully hosted! Tap striker to controll!";
        }
        else
        {
            SnackbarText.text = "Cloud Anchor could not be hosted. " + response;
        }
    }

    public void OnAnchorResolved(bool success, string response)
    {
        if (success)
        {
            SnackbarText.text = "CloudAnchor successfully resolved! Tap striker to controll";
        }
        else
        {
            SnackbarText.text = "Could Anchor could not be resolve. Will attempt again." + response;
        }
    }
    #endregion


    #region Network Event
#pragma warning disable 618
    private void _OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
#pragma warning restore 618
    {
        NetworkManager.OnMatchCreate(success, extendedInfo, matchInfo);
        if (!success)
        {
            SnackbarText.text = "Could not create match : " + extendedInfo;
            return;
        }

        CurrentRoomNum = _GetRoomNumberFromNetworkId(matchInfo.networkId);
        SnackbarText.text = "Connecting to server...";
        _ChangeLobbyUIVisibility(false);
        CurrentRoomLabel.GetComponentInChildren<Text>().text = "Room: " + CurrentRoomNum;
    }

#pragma warning disable 618
    private void _OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
#pragma warning restore 618
    {
        NetworkManager.OnMatchJoined(success, extendedInfo, matchInfo);
        if (!success)
        {
            SnackbarText.text = "Could not join to match: " + extendedInfo;
            return;
        }

        CurrentRoomNum = _GetRoomNumberFromNetworkId(matchInfo.networkId);
        SnackbarText.text = "Connectiong to server...";
        _ChangeLobbyUIVisibility(false);
        CurrentRoomLabel.GetComponentInChildren<Text>().text = "Romm: " + CurrentRoomNum;
    }


#pragma warning disable 618
    private void _OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
#pragma warning restore 618
    {
        NetworkManager.OnMatchList(success, extendedInfo, matches);
        if (!success)
        {
            SnackbarText.text = "Could not list matches: " + extendedInfo;
            return;
        }

        //매칭 성공하면
        if(NetworkManager.matches != null)
        {
            //버튼 풀에 있는 모든 요소 리셋
            foreach(GameObject button in JoinRoomButtonPool)
            {
                button.SetActive(false);
                button.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
                button.GetComponentInChildren<Text>().text = string.Empty;
            }

            NoPreviousRoomsText.gameObject.SetActive(NetworkManager.matches.Count == 0);

            //존재하는 매치에 대해서 버튼 추가하기
            int i = 0;
            foreach(var match in NetworkManager.matches)
            {
                if(i >= PageSize)
                {
                    break;
                }

                var text = "Room" + _GetRoomNumberFromNetworkId(match.networkId);
                GameObject button = JoinRoomButtonPool[i++];
                button.GetComponentInChildren<Text>().text = text;
                button.GetComponentInChildren<Button>().onClick.AddListener(() => _OnJoinRoomClicked(match));
                //resolve 클릭 이벤트 들어왔을때 리스너 부여
                //button.GetComponentInChildren<Button>().onClick.AddListener(temp);
                button.SetActive(true);
            }
        }        
    }

#pragma warning disable 618
    private void _OnJoinRoomClicked(MatchInfoSnapshot match)
#pragma warning restore 618
    {
        NetworkManager.matchName = match.name;
        NetworkManager.matchMaker.JoinMatch(match.networkId, string.Empty, string.Empty, string.Empty, 0, 0, _OnMatchJoined);
    }


        
    private void _ChangeLobbyUIVisibility(bool visible)
    {
        LobbyScreen.gameObject.SetActive(visible);
        CurrentRoomLabel.gameObject.SetActive(!visible);
        foreach(GameObject button in JoinRoomButtonPool)
        {
            bool active = visible && button.GetComponentInChildren<Text>().text != string.Empty;
            button.SetActive(active);
        }
    }

    private string _GetRoomNumberFromNetworkId(NetworkID networkId)
    {
        return (System.Convert.ToInt64(networkId.ToString()) % 10000).ToString();
    }
    #endregion

    public void ShowDebugMessage(string debugMessage)
    {
        SnackbarText.text = debugMessage;
    }
}
