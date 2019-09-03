using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HockeyStickControl : MonoBehaviour
{



    //게임에게 달아주면 됨.
    //정확히 이거 기능은 게임에서 다루면 되는건데 그냥 나눠서 작업하니 분리.
    //역할은 1)스틱 배치관리. 

    public GameObject StickObjectPlayer0;
    public GameObject StickObjectPlayer1;



    GameObject hockeyBoard;
    void Start()
    {
        hockeyBoard = GameObject.Find("HockeyBoard");
        SpawnStick();
    }









    //볼생성함수
    public void SpawnStick()
    {
        StickObjectPlayer0 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        StickObjectPlayer0.name = "HockeyStick";
        StickObjectPlayer0.tag = "HockeyStick";
        StickObjectPlayer0.transform.position = hockeyBoard.transform.position + new Vector3(0, 0.25f, -hockeyBoard.transform.localScale.z / 2);
        StickObjectPlayer0.transform.localScale = new Vector3(1f, 0.5f, 0.1f);
        StickObjectPlayer0.AddComponent<HockeyStickInf>();
        StickObjectPlayer0.GetComponent<HockeyStickInf>().HockeyStickUserNo = 0;
        StickObjectPlayer1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        StickObjectPlayer1.name = "HockeyStick";
        StickObjectPlayer1.tag = "HockeyStick";
        StickObjectPlayer1.transform.position = hockeyBoard.transform.position + new Vector3(0, 0.25f, hockeyBoard.transform.localScale.z / 2);
        StickObjectPlayer1.transform.localScale = new Vector3(1f, 0.5f, 0.1f);
        StickObjectPlayer1.AddComponent<HockeyStickInf>();
        StickObjectPlayer1.GetComponent< HockeyStickInf>().HockeyStickUserNo = 1;

    }


    public Text TextExam;
    public void SetText(string Test)
    {
        TextExam.text = "" + Test;
    }
}
