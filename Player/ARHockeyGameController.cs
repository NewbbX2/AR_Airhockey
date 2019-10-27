using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
//시작전 세팅
//EmptyObject의 name을 GameOBJ로 수정한뒤, 이 스크립트를 달아준다.
//tag가 Striker인 오브젝트가 2개가 있어야한다.

//UI에 ScoreText를 연결해둔다.

//BoardTag가 달린 3D오브젝트 필요.
//GoalTag달린 3D오브젝트 필요.
//1p의 점수는 score[0] 2p 는 score[1]

//Board, Puck, Goal tag 필요
//Prefab가 전부 만들어진 경우, /**/방식의 주석을 전부 해제하고, 그 이전에 지우라고 한 코드를 지우면된다.

/// <summary>
/// Puck과 Playercharacter, 스코어등 게임 전반적인 내용을 관리합니다.
/// </summary>
public class ARHockeyGameController : MonoBehaviourPunCallbacks
{

    #region Inspector용 공개 변수
    public Transform SpawnPoint1; // 1플레이어 쪽 공 소환 위치
    public Transform SpawnPoint2; // 2플레이어 쪽 공 소환 위치
    public Transform[] PlayerSpawn; // 스트라이커 소환 위치
    public TextMeshProUGUI ScoreText;    // 스코어 표시할 텍스트
    public GameObject StrikerPrefab; // 스폰할 스트라이커 프리팹
    public GameObject PuckPrefab; //스폰할 퍽 프리팹
    public GameObject HockeyTable; // 하키 테이블
    [System.NonSerialized] public GameObject[] Goal;
    public bool IsPhotonConnected = false;
    [System.NonSerialized] public GameObject Puck;
    [System.NonSerialized] public GameObject[] StrikerList;
    #endregion



    //보드 오브젝트
    //점수
    private int[] Score = new int[2]; // 팀별 점수 저장할 배열
    private int PlayerNumber; // 플레이어 넘버
    private bool GoalActive = false;
<<<<<<< Updated upstream
=======
    private Hashtable props; // 방 프로퍼티
    
    private void Awake()
    {
        if(ScoreText == null)
        {
            ScoreText = FindObjectOfType<TextMeshProUGUI>();
        }
    }
>>>>>>> Stashed changes

    void Start()
    {
        //플레이어 넘버를 정한다.
        props = PhotonNetwork.CurrentRoom.CustomProperties;
        foreach(var key in props.Keys)
        {            
            Debug.Log(key + ": " + props[key].ToString());
        }
        
        foreach(string key in props.Keys)
        {
            if (key.Equals("1") && !(bool)props[key])
            {
                props[key] = true;
                PhotonNetwork.CurrentRoom.SetCustomProperties(props);
                SetPlayerNumber(int.Parse(key));
                break;
            }
            else if (key.Equals("2") && !(bool)props[key])
            {
                props[key] = true;
                PhotonNetwork.CurrentRoom.SetCustomProperties(props);
                SetPlayerNumber(int.Parse(key));
                break;
            }

        }

        foreach (var key in props.Keys)
        {
            Debug.Log(key + ": " + props[key].ToString());
        }


        HockeyTable = GameObject.FindGameObjectWithTag("Table");
        //StrikerList = GameObject.FindGameObjectsWithTag("Striker");
        Goal = GameObject.FindGameObjectsWithTag("Goal");
<<<<<<< Updated upstream
        if(ScoreText == null)
        {
            ScoreText = FindObjectOfType<TextMeshProUGUI>();
        }
       if(Goal[0].GetComponent<GoalInf>().TeamNo==1 && Goal[1].GetComponent<GoalInf>().TeamNo==0)
=======
        
        if (Goal[0].GetComponent<GoalInf>().TeamNo==1 && Goal[1].GetComponent<GoalInf>().TeamNo==0)
>>>>>>> Stashed changes
        {
            GameObject tempobj = Goal[0];
            Goal[0] = Goal[1];
            Goal[1] = tempobj;
        }

        //스트라이커를 스폰
        InstantiateStriker();

        //첫 퍽을 스폰.
        if (PhotonNetwork.IsConnected)
        {
            IsPhotonConnected = true;
            ScoreText.text = "GAME Start";
            Debug.Log("GameStart");
            if (PhotonNetwork.IsMasterClient) SpawnNewPuck(0);
        }
        else
        {
            IsPhotonConnected = false;
            ScoreText.text = " PothonError discon";
            Debug.Log("err : PothonError discon");
            SpawnNewPuck(0);
        }

    }

    //플레이어 넘버 배정
    private void SetPlayerNumber(int playerNumber)
    {
        PlayerNumber = playerNumber;
        Debug.Log("Player " + PlayerNumber + " is Setted");
    }

    private void InstantiateStriker()
    {
        Transform trans = PlayerSpawn[PlayerNumber-1].transform;
        var striker = PhotonNetwork.Instantiate(StrikerPrefab.name, trans.position, Quaternion.identity);
        striker.transform.localScale = new Vector3(striker.transform.localScale.x, 
                                                   striker.transform.localScale.y, 
                                                   striker.transform.localScale.z*trans.localScale.z);
    }

    //스코어 표기
    private void SetScoreText()
    {
        ScoreText.text = "Team1:" + Score[0] + "  ||  " + "Team2:" + Score[1];
    }

    //게임 종료
    private void GameOver()
    {
        ScoreText.text = "GAME OVER";
    }


    //퍽 배치
    /// <summary>
    /// 매개변수는 스폰시킬 위치
    /// </summary>
    /// <param name="spawnTeamNum"></param>
    public void SpawnNewPuck(int spawnTeamNum)
    {
        //어느 팀에 배치해 줄 건지 포인트 지정
        Vector3 spawnPoint = Vector3.zero;
        switch (spawnTeamNum)
        {
            case 0:
                spawnPoint = SpawnPoint1.position;
                break;

            case 1:
                spawnPoint = SpawnPoint2.position;
                break;

            default:
                Debug.Log("Fail to spawn puck");
                return;
        }

        //프리팹을 이용하여 인스턴스화
        if (IsPhotonConnected)
        {
            if (PhotonNetwork.IsMasterClient)
                PhotonNetwork.InstantiateSceneObject(PuckPrefab.name, spawnPoint, Quaternion.identity);
        }
        else
        {
            Puck = (GameObject) Instantiate(PuckPrefab, spawnPoint, Quaternion.identity);

        }
        GoalActive = true;
    }

    /// <summary>
    /// 점수를 올리고 싶은 플레이어의 번호를 매개변수에 넣으시오
    /// </summary>
    /// <param name="playerNum"></param>
    public void ScoreUp(int playerNum)
    {
        //골 넣은쪽에 점수 올리고 넣은 사람 쪽으로 퍽 스폰
        Score[playerNum]++;
        SetScoreText();
    }

    /// <summary>
    /// 골 판정. Puck의 골 안에 들어간 코드를 여기로
    /// </summary>
    /// <param name="TeamNo"></param>
    /// <param name="trigger"></param>
    public void JudgmentGoal(int TeamNo, GameObject puck)
    {
        if (GoalActive)
        {
            switch (TeamNo)
            {
                case 0:
                    GoalActive = false;
                    ScoreUp(1);
                    SpawnNewPuck(0);
                    PhotonNetwork.Destroy(puck);
                    break;

                case 1:
                    GoalActive = false;
                    ScoreUp(0);
                    SpawnNewPuck(1);
                    PhotonNetwork.Destroy(puck);
                    break;

                default:
                    Debug.Log(TeamNo);
                    return;
            }
        }
    }

    #region 포톤 네트워크 콜백
    public override void OnLeftRoom()
    {
        props[PlayerNumber.ToString()] = false;
    }
    #endregion
}
