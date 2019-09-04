using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerCharacter : MonoBehaviour
{

    public int playerNo = 0;
    public GameObject StickObject;
    public Text ScoreText;



    Game gameOBJ;
    GameObject hockeyBoard;



    void Start()
    {
        gameOBJ = GameObject.Find("GameOBJ").GetComponent<Game>();
        hockeyBoard = GameObject.FindGameObjectWithTag("Board");
        PuckStartSetting();
    }
















    //하키스틱 찾은뒤 배치.
    public void PuckStartSetting()
    {
        StickObject = GameObject.FindGameObjectsWithTag("Puck")[playerNo-1];
        StickObject.name = "HockeyPuck";
        StickObject.transform.localScale = new Vector3(1f, 0.5f, 0.1f);
        StickObject.AddComponent<HockeyStickInf>();
        StickObject.GetComponent<HockeyStickInf>().HockeyStickUserNo = playerNo;

    }



    //스코어 
    public void SetScoreText()
    {
        ScoreText.text = "Team1:" + gameOBJ.Score[0] + "Team2:" + gameOBJ.Score[1];
    }




}

