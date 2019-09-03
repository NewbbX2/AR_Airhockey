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
    //조작중인지여부
    public bool isKeepTouching = false;
    //스틱 이동방향
    public Vector3 NormalizedStickMoveMent = new Vector3(0, 0, 0);
    //스틱스피드
    public float StickSpeed = 0;

    //보드 오브젝트
    GameObject hockeyBoard;

    //시작세팅
    void Start()
    {
        hockeyBoard = GameObject.Find("HockeyBoard");
        HockeyStickObjectSelf = GameObject.FindGameObjectsWithTag("HockeyStick")[HockeyStickUserNo];
    }


    //업데이트
    void Update()
    {
        stickCannotMoveThere();

        calculateSitckSpeedAndDirec();
        getControllerMovement();

    }
    //매 업데이트마다 스틱의 속도 및 방향 계산.
    //컨트롤러 비작동중인경우만 여기서 체크.
    //동작중일시 컨트롤러값으로 이미 연산완료.
    void calculateSitckSpeedAndDirec()
    {
        //스틱 조작후 속도 빛 방향
        if (isKeepTouching)
        {

        }
        //일단 고정된 스틱인경우의 공이 부딫칠때, 공의 이동방향 및 속도.
        else
        {
            StickSpeed = GameObject.Find("HockeyBall").GetComponent<BallInforAndMove>().BallMoveMent.magnitude;
            NormalizedStickMoveMent = GameObject.Find("HockeyBall").GetComponent<BallInforAndMove>().BallMoveMent.normalized;
            NormalizedStickMoveMent.z *= -1;
        }

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







    public int Controller_Type_touch_0_mouse_1_keyboard_2_pad_3 = 1;
    //컨트롤러 타입에 따라 입력을 받아옴.
    void getControllerMovement()
    {

        if (Controller_Type_touch_0_mouse_1_keyboard_2_pad_3 == 0)
        {

        }
        else if (Controller_Type_touch_0_mouse_1_keyboard_2_pad_3 == 1)
        {
            if(Input.GetMouseButtonDown(0))
            {
                Vector3 Vec=new Vector3(Input.GetAxis("Mouse X"),0,0);
                GameObject.Find("Controller").GetComponent<HockeyStickControl>().SetText(Input.GetAxis("Mouse X")+""+ Input.GetAxis("Mouse Y"));
                CalculateMoveStickOrNotByController(Vec);

            }
            else
            {
                isKeepTouching = false;

            }
        }
    }




    //컨트롤러 동작으로 스틱 이동여부 판정 
    public void CalculateMoveStickOrNotByController(Vector3 controllerMoveVec)
    {
        if (controllerMoveVec.magnitude == 0)
        {
            isKeepTouching = false;
        }
        else
        {
            isKeepTouching = true;
            MoveHockeyStickByControllerMove(controllerMoveVec* coefficientBetween_Controller_Stick);

        }
    }
    //컨트롤러로 지정한 이동함을 저정하고, 실제로 좌표이동.
    void MoveHockeyStickByControllerMove(Vector3 controllermoveVec)
    {
        NormalizedStickMoveMent = controllermoveVec.normalized;
        StickSpeed = controllermoveVec.magnitude * coefficientBetween_Stick_Ball;
        HockeyStickObjectSelf.transform.position += controllermoveVec;
    }
    float coefficientBetween_Controller_Stick = 10;
    float coefficientBetween_Stick_Ball = 0.1f;






}
