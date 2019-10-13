using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
public class HockeyStriker : MonoBehaviour
{
    //유저번호
    [Range(1, 2)] public int UserNo = 1;
    Vector3 movementVectorToAffectBall = new Vector3(0, 0, 0);//스틱 이동방향
    private Rigidbody StrikerRigidbody;
    private ARHockeyGameController GameController;
    private GameObject HockeyTable;

    //시작세팅
    void Start()
    {
        GameController = GameObject.Find("GameController").GetComponent<ARHockeyGameController>();
        HockeyTable = GameController.HockeyTable;
        StrikerRigidbody = GetComponent<Rigidbody>();
    }
    public Vector3 StrikerMoveBall()
    {
        return movementVectorToAffectBall;
    }
    DateTime T_Now;
    DateTime T_Past;
    bool checkisStrikerMovedNow = false;
    void Update()
    {
        T_Past = T_Now;
        T_Now = DateTime.Now;
        Loc_Past = Loc_Now;
        Loc_Now = transform.position;
        if (Loc_Now == Loc_Past)
        {
            checkisStrikerMovedNow = false;
        }
        else
        {
            checkisStrikerMovedNow = true;
        }
        //움직였을경우 현재속도 측정.
        if (checkisStrikerMovedNow)
        {
            TimeSpan TS = T_Now - T_Past;
            //movementVectorToAffectBall = (Loc_Now - Loc_Past) / TS.Seconds;
            movementVectorToAffectBall = StrikerRigidbody.velocity;
        }
        //현재 이동량이 0이거나
        //이전에 이동했던게 너무 약할시
        else
        {
            //너무 약해진경우 벽처럼
            if (movementVectorToAffectBall.sqrMagnitude <= 0.1)
            {
                //calculateSitckSpeedAndDirecWithNoMove();
            }
            //아직 덜약해진경우 동작스틱처럼(단, 시간당 파워감소)
            else
            {
                movementVectorToAffectBall *= 0.99f;
            }
        }
    }
    Vector3 Loc_Now = new Vector3(0, 0, 0);
    Vector3 Loc_Past = new Vector3(0, 0, 0);
    void calculateSitckSpeedAndDirecWithNoMove()
    {
        //스틱 조작후 속도 빛 방향=
        //일단 고정된 스틱인경우의 공이 부딫칠때, 공의 이동방향 및 속도.

        movementVectorToAffectBall = GameController.Puck.GetComponent<Puck>().Movement;
        movementVectorToAffectBall.z *= -1;
    }
    //스틱 이동불가 지역 판정
    void strikerCannotMoveThere()
    {
        float zDistanceToGoal0 = Mathf.Abs(GameController.Goal[0].transform.position.z - transform.position.z);

        float zDistanceToGoal1 = Mathf.Abs(GameController.Goal[1].transform.position.z - transform.position.z);

        //짝수팀이 
        if (UserNo % 2 == 0)
        {
            //홀수팀골대쪽에 퍽가져가면
            if (zDistanceToGoal0<zDistanceToGoal1)
            {
                transform.position=new Vector3(transform.position.x, transform.position.y,0);
            }
        }
        //홀수팀이 
        else if (UserNo % 2 == 1)
        {
            //짝수팀골대쪽에 퍽가져가면 (홀수팀 골대에서의 거리가 더가까우면)
            if (zDistanceToGoal0 > zDistanceToGoal1)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            }
        //좌우로 벗어난 경우, 테이블 끝으로 가져옴.
        //벽이 어떤식인지 몰라서 일단 좌우로 나눔.
        if (transform.position.x > HockeyTable.transform.localScale.x / 2)
        {
            transform.position = new Vector3(HockeyTable.transform.localScale.x / 2, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -HockeyTable.transform.localScale.x / 2)
        {
            transform.position = new Vector3(HockeyTable.transform.localScale.x / 2, transform.position.y, transform.position.z);
        }
    }
}
