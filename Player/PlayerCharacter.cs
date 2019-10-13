using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
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
    ARHockeyGameController GameController;
    //보드위치 확인용.
    GameObject HockeyTable;

    //해당 플레이어의 Puck
    public GameObject StrikerObject;


    void Start()
    {
       // HockeyTable = GameObject.FindGameObjectWithTag("Board");
        GameController = FindObjectOfType<ARHockeyGameController>();
        HockeyTable = GameController.HockeyTable;
        StrikerStartSetting();
        Instantiate(StrikerObject, HockeyTable.transform.position + new Vector3(0f, 0f, -5f), Quaternion.identity);
    }


    //SJ. 하키스틱 설정
    /*
    public void StrikerStartSetting()
    {
        StrikerObject = GameController.StrikerList[playerNo - 1];
        StrikerObject.name = "HockeyStriker";
        StrikerObject.transform.localScale = new Vector3(1f, 0.5f, 0.1f);
        StrikerObject.AddComponent<HockeyStriker>();
        StrikerObject.GetComponent<HockeyStriker>().UserNo = playerNo;
    }
    */


    //하키스틱 찾은뒤 배치.
    public void StrikerStartSetting()
    {
        StrikerObject = GameObject.FindGameObjectsWithTag("Striker")[playerNo - 1];
        StrikerObject.name = "Striker" + playerNo;
        if (playerNo == 1)
        {
            StrikerObject.transform.localScale = new Vector3(1f, 0.5f, 0.1f);
        }
        else if (playerNo == 2)
        {
            StrikerObject.transform.localScale = new Vector3(1f, 0.5f, 0.1f);
        }

        StrikerObject.AddComponent<HockeyStriker>();
        StrikerObject.GetComponent<HockeyStriker>().UserNo = playerNo;
       // StrikerObject.GetComponent<HockeyStriker>().UserNo = playerNo;
    }




}
