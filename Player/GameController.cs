using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
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
public class GameController : MonoBehaviourPunCallbacks
{

    #region Inspector용 공개 변수
    public Transform SpawnPoint1;
    public Transform SpawnPoint2;
    public TextMeshProUGUI ScoreText;    // 스코어 표시할 텍스트
    public GameObject PuckPrefab; //스폰할 퍽 프리팹
    #endregion



    //점수
    private int[] Score = new int[2];

    void Start()
    {
        ScoreText.text = "GAME Start";
        //멀티 플레이 방식에서는 필요없음 
        Debug.Log("GameStart");
        //플레이어 생성
        //SpawnPlayer(1); SpawnPlayer(2);
        //첫 퍽을 스폰.
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnNewPuck(0);
        }        
    }

    private void Update()
    {
     
    }

    //멀티 플레이 방식에서는 필요없음

    //플레이어번호 n인 플레이어 만듬.
    void SpawnPlayer(int n)
    {

        GameObject player = new GameObject();
        player.AddComponent<PlayerCharacter>();

        player.name = "player" + n;
        player.GetComponent<PlayerCharacter>().playerNo = n;

    }

    //스코어 표기
    private void SetScoreText()
    {
        ScoreText.text = "Team1:" + Score[0] + "  Team2:" + Score[1];
    }

    //게임 종료
    private void GameOver()
    {
        ScoreText.text = "GAME OVER";
    }


    //퍽 배치
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
        PhotonNetwork.Instantiate(PuckPrefab.name, spawnPoint, Quaternion.identity);

    }

    public void ScoreUp(int playerNum)
    {
        //골 넣은쪽에 점수 올리고 넣은 사람 쪽으로 퍽 스폰
        Score[playerNum]++;
        SetScoreText();
    }
}