using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{




    //Ball ,Goal,Score   그리고 PlayerCharacter를 관리할거임.
    //PlayerCharacter는 Stick 관리할거임.


    public GameObject BallObject;
    public int[] Score = new int[2];
    public GameObject[] goal = new GameObject[2];


    void Start()
    {
        CreatePlayer(1);        CreatePlayer(2);
        SpawnNewBall(new Vector3(0, 1F, 0), new Vector3(0, 0, 10));
        for(int i=0; i<=1; i++)
        {
            Score[i] = 0;
            if(GameObject.FindGameObjectsWithTag("Goal")[0].GetComponent<GoalInf>().TeamNo==i)
            {
                goal[i] = GameObject.FindGameObjectsWithTag("Goal")[0];
            }
            else if(GameObject.FindGameObjectsWithTag("Goal")[1].GetComponent<GoalInf>().TeamNo == i)
            {
                goal[i] = GameObject.FindGameObjectsWithTag("Goal")[1];
            }
        }
    }


    //플레이어번호 n인 플레이어 만듬.
    void CreatePlayer(int n)
    {
        GameObject player = new GameObject();
        player.AddComponent<PlayerCharacter>();
        player.GetComponent<PlayerCharacter>().playerNo = n;
    }



    //볼은 게임에서 관리.
    //퍽은 플레이어에서 관리.
    public void SpawnNewBall(Vector3 spawnPosition, Vector3 ballDirection)
    {
        BallObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        BallObject.name = "HockeyBall";
        BallObject.transform.position = spawnPosition;
        BallObject.transform.localScale = new Vector3(0.5f, 0.1f, 0.5f);
        BallObject.AddComponent<BallInforAndMove>();
        BallObject.GetComponent<BallInforAndMove>().BallMoveMent = ballDirection;
    }
}










