using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerCharacter : MonoBehaviour
{

    //네트워크 환경에서는 의미 없음. 플레이어 컨트롤러 없이 바로 striker 스크립트에서 조정할 것
    //자동처리되는 스크립트.


    //이것은 오브젝트에 달아놓는게 아니라
    //생성되어 자동으로 달림.
    //따라서 따로 관리할필요없다.



    //플레이어번호. 이전에 지정해주었을것이다.
    public int playerNo = 0;

    //점수관리때문에 게임다시 가져옴.
    GameController _GameController;
    //보드위치 확인용.
    GameObject hockeyBoard;

    //해당 플레이어의 Puck
    public GameObject StickObject;


    void Start()
    {
        _GameController = FindObjectOfType<GameController>();
        hockeyBoard = GameObject.FindGameObjectWithTag("Board");
        PuckStartSetting();

        //SJ. 세팅하고 세팅한 오브젝트 설치
        StickStartSetting();
        Instantiate(StickObject, hockeyBoard.transform.position + new Vector3(0f, 0f, -5f), Quaternion.identity);
    }


    //SJ. 하키스틱 설정
    public void StickStartSetting()
    {
        StickObject = GameObject.FindGameObjectsWithTag("Puck")[playerNo - 1];
        StickObject.name = "HockeyPuck";
        StickObject.transform.localScale = new Vector3(1f, 0.5f, 0.1f);
        StickObject.AddComponent<HockeyStriker>();
        StickObject.GetComponent<HockeyStriker>().UserNo = playerNo;
    }


    //하키스틱 찾은뒤 배치.
    public void PuckStartSetting()
    {
        StickObject = GameObject.FindGameObjectsWithTag("Puck")[playerNo - 1];
        StickObject.name = "HockeyPuck" + playerNo;
        if (playerNo == 1)
        {
            StickObject.transform.localScale = new Vector3(1f, 0.5f, 0.1f);
        }
        else if(playerNo==2)
        {
            StickObject.transform.localScale = new Vector3(1f, 0.5f, 0.1f);
        }

        StickObject.AddComponent<HockeyStriker>();
        StickObject.GetComponent<HockeyStriker>().UserNo = playerNo;
        StickObject.GetComponent<HockeyStriker>().UserNo = playerNo;
        }




}

