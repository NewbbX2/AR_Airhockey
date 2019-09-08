using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerCharacter : MonoBehaviour
{

    //자동처리되는 스크립트.


    //이것은 오브젝트에 달아놓는게 아니라
    //생성되어 자동으로 달림.
    //따라서 따로 관리할필요없다.



    //플레이어번호. 이전에 지정해주었을것이다.
    public int playerNo = 0;

    //점수관리때문에 게임다시 가져옴.
    Game gameOBJ;
    //보드위치 확인용.
    GameObject hockeyBoard;

    //해당 플레이어의 Puck
    public GameObject StickObject;


    void Start()
    {
        gameOBJ = GameObject.Find("GameOBJ").GetComponent<Game>();
        hockeyBoard = GameObject.FindGameObjectWithTag("Board");
        PuckStartSetting();
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

        StickObject.AddComponent<HockeyStickInf>();
        StickObject.GetComponent<HockeyStickInf>().HockeyStickUserNo = playerNo;
        StickObject.GetComponent<HockeyStickInf>().HockeyStickUserNo = playerNo;
        }




}

