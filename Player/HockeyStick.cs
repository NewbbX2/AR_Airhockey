


using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class HockeyStickInf : MonoBehaviour
{
    //유저번호
    public int HockeyStickUserNo = 0;
    //스틱오브젝트 셀프
    GameObject HockeyStickObjectSelf;
    //스틱 이동방향
    Vector3 movementVectorToAffectBall = new Vector3(0, 0, 0);

    //보드 오브젝트
    GameObject hockeyBoard;




    //시작세팅
    void Start()
    {
        hockeyBoard = GameObject.FindGameObjectWithTag("Board");
        HockeyStickObjectSelf = GameObject.FindGameObjectsWithTag("Puck")[HockeyStickUserNo - 1];
    }




    public Vector3 StickMoveBall()
    {
        return movementVectorToAffectBall;
    }

    DateTime T_Now;
    DateTime T_Past;
    bool checkisStickMovedNow = false;
    void Update()
    {

        T_Past = T_Now;
        T_Now = DateTime.Now;
        Loc_Past = Loc_Now;
        Loc_Now = HockeyStickObjectSelf.transform.position;


        if (Loc_Now == Loc_Past)
        {
            checkisStickMovedNow = false;
        }
        else
        {
            checkisStickMovedNow = true;
        }

        //움직였을경우 현재속도 측정.
        if (checkisStickMovedNow)
        {
            TimeSpan TS = T_Now - T_Past;
            movementVectorToAffectBall=(Loc_Now- Loc_Past)/TS.Seconds;
        }
        //현재 이동량이 0이거나
        //이전에 이동했던게 너무 약할시
        else
        {
            //너무 약해진경우 벽처럼
            if (movementVectorToAffectBall.sqrMagnitude <= 0.1)
            {
                calculateSitckSpeedAndDirecWithNoMove();
            }
            //아직 덜약해진경우 동작스틱처럼(단, 시간당 파워감소)
            else
            {
                movementVectorToAffectBall *= 0.99f;
            }
        }

    }


    Vector3 Loc_Now=new Vector3(0,0,0);
    Vector3 Loc_Past = new Vector3(0, 0, 0);


    void calculateSitckSpeedAndDirecWithNoMove()
    {
        //스틱 조작후 속도 빛 방향=
        //일단 고정된 스틱인경우의 공이 부딫칠때, 공의 이동방향 및 속도.
        movementVectorToAffectBall = GameObject.Find("HockeyBall").GetComponent<Ball>().BallMoveMent;
        movementVectorToAffectBall.z *= -1;

    }
    //스틱 이동불가 지역 판정
    void stickCannotMoveThere()
    {
        //짝수팀(아래)가
        if (HockeyStickUserNo % 2 == 0)
        {
            //공위치:테이블 절반이상일시.
            if (HockeyStickObjectSelf.transform.position.z > 0)
            {
                HockeyStickObjectSelf.transform.position = new Vector3(HockeyStickObjectSelf.transform.position.x, HockeyStickObjectSelf.transform.position.y, 0);

            }
            //공위치:테이블 아래일시
            //공위치:테이블위일시.
            else if (HockeyStickObjectSelf.transform.position.z < hockeyBoard.transform.localScale.z)
            {
                HockeyStickObjectSelf.transform.position = new Vector3(HockeyStickObjectSelf.transform.position.x, HockeyStickObjectSelf.transform.position.y, -hockeyBoard.transform.localScale.z / 2);
            }

        }
        //홀수팀(위)가 
        else if (HockeyStickUserNo % 2 == 1)
        {
            //공위치:테이블 절반아래일떄
            if (HockeyStickObjectSelf.transform.position.z < 0)
            {
                HockeyStickObjectSelf.transform.position = new Vector3(HockeyStickObjectSelf.transform.position.x, HockeyStickObjectSelf.transform.position.y, 0);
            }
            //공위치:테이블위일시.
            else if (HockeyStickObjectSelf.transform.position.z > hockeyBoard.transform.localScale.z)
            {
                HockeyStickObjectSelf.transform.position = new Vector3(HockeyStickObjectSelf.transform.position.x, HockeyStickObjectSelf.transform.position.y, hockeyBoard.transform.localScale.z / 2);
            }
        }
        //좌우로 벗어난 경우, 테이블 끝으로 가져옴.
        //벽이 어떤식인지 몰라서 일단 좌우로 나눔.
        if (HockeyStickObjectSelf.transform.position.x > hockeyBoard.transform.localScale.x / 2)
        {
            HockeyStickObjectSelf.transform.position = new Vector3(hockeyBoard.transform.localScale.x / 2, HockeyStickObjectSelf.transform.position.y, HockeyStickObjectSelf.transform.position.z);
        }
        else if (HockeyStickObjectSelf.transform.position.x < -hockeyBoard.transform.localScale.x / 2)
        {
            HockeyStickObjectSelf.transform.position = new Vector3(hockeyBoard.transform.localScale.x / 2, HockeyStickObjectSelf.transform.position.y, HockeyStickObjectSelf.transform.position.z);
        }
    }






}
