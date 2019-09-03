using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallControl : MonoBehaviour
{

    //게임에게 달아주면 됨.
    //정확히 이거 기능은 게임에서 다루면 되는건데 그냥 나눠서 작업하니 분리.
    //역할은 1)볼만들고 배치관리. 2) 골들어가면 점수 text에 점수변경.

    public GameObject BallObject;
    public int[] Score = new int[2];
    public GameObject[] goal=new GameObject[2];
    public Text ScoreText;
    void Start()
    {
        SpawnNewBall(new Vector3(0, 1F, 0), new Vector3(0,0,10));
        Score[0] = 0;
        Score[1] = 1;
        goal[0] = GameObject.Find("Goal0");
        goal[1] = GameObject.Find("Goal1");
    }

    // Update is called once per frame
   


    //볼생성함수
    public void SpawnNewBall(Vector3 spawnPosition, Vector3 ballDirection)
    {
        BallObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        BallObject.name = "HockeyBall";
        BallObject.transform.position = spawnPosition;
        BallObject.transform.localScale = new Vector3(0.5f, 0.1f, 0.5f);
        BallObject.AddComponent<BallInforAndMove>();
        BallObject.GetComponent<BallInforAndMove>().BallMoveMent = ballDirection;
    }


    //텍스트 
    public void SetScoreText()
    {
        ScoreText.text = "Player1:" + Score[0] + "Player2:" + Score[1];
    }
    public void SetText( Text Test)
    {
        ScoreText.text = ""+Test;
    }




}
