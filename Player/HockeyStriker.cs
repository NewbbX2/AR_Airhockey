using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class HockeyStriker : MonoBehaviourPunCallbacks, IPunObservable
{
    //유저번호
    //[Range(1, 2)] public int UserNo = 1;
    
    private Vector3 movementVectorToAffectBall = new Vector3(1, 1, 1);//스틱 이동방향
    private Rigidbody StrikerRigidbody;
    private ARHockeyGameController GameController;
    private GameObject HockeyTable;

    //시작세팅
    void Start()
    {
        GameController = FindObjectOfType<ARHockeyGameController>();
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
        if (!photonView.IsMine)
        {
            StrikerRigidbody.velocity = currentVel;
            //Debug.Log(currentVel);
        }
        else
        {
            //Debug.Log(StrikerRigidbody.velocity);
        }
        if (!PhotonNetwork.IsMasterClient) return;
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
        else
        {
            //현재 이동량이 0이거나 이전에 이동했던게 너무 약할시
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
        StrikerCannotMoveOutOfTable();
    }
    Vector3 Loc_Now = new Vector3(0, 0, 0);
    Vector3 Loc_Past = new Vector3(0, 0, 0);
    void calculateSitckSpeedAndDirecWithNoMove()
    {
        //스틱 조작후 속도 빛 방향=
        //일단 고정된 스틱인경우의 공이 부딫칠때, 공의 이동방향 및 속도.
        movementVectorToAffectBall = GameController.Puck.GetComponent<Puck>().Movement;
        movementVectorToAffectBall.x *= -1; movementVectorToAffectBall.z *= -1;
        
        
        
    }
    //스틱 이동불가 지역 판정
    void StrikerCannotMoveOutOfTable()
    {

        RaycastHit hit;
        if (Physics.Raycast(transform.position, new Vector3(0, 1, 0), out hit, 1000000))
        {
            if (hit.transform.CompareTag("Table"))
            {
                return;
            }

        }
        if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), out hit, 1000000))
        {
            if (hit.transform.CompareTag("Table"))
            {
                return;
            }

        }
        transform.position = new Vector3(0, transform.position.y, 0);

    }

    #region 포톤뷰 통신부분
    private Vector3 currentVel;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(StrikerRigidbody.velocity);
            //Debug.Log("send : " + StrikerRigidbody.velocity);
        }
        else
        {
            currentVel = (Vector3)stream.ReceiveNext();
            //Debug.Log("receive : " + currentVel);
        }
    }
    #endregion

}
