#define MOUSE
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// 하키 Striker를 조작하기 위한 클래스
/// </summary>

enum PlayerNumber
{
    Player1,
    Player2,
}
public class MoveStriker : MonoBehaviourPunCallbacks, IPunObservable
{
    #region 공개 변수들
    public float PokeForce = 5.0f;//찌르는 듯한 물리효과의 강도
    public float MoveSpeed = 20.0f; // 이동속도
    public GameObject MiddlePoint; //경기장 중앙 지점
    [Range(0, 1)] public int Controller;
    #endregion

    #region 내부 변수
    private Vector2 TouchPos;//터치된 위치
    private Vector3 TouchVector;//Ray쏠 방향
    private RaycastHit RayHit;
    private GameObject HockeyBoard;
    private Vector3 StrikerDestination;
    private float MaxZ;
    private float MiddlePointZ; //경기장 중앙 지점의 Z좌표값
    private Rigidbody StrikerRigidbody;
    private PlayerNumber PlayerNum;
    #endregion



    void Start()
    {
        StrikerRigidbody = GetComponent<Rigidbody>();
        MiddlePointZ = MiddlePoint.transform.position.z;
        if (PhotonNetwork.IsMasterClient)
        {
            PlayerNum = PlayerNumber.Player1;
        }
        else
        {
            PlayerNum = PlayerNumber.Player2;
        }
        if ((int)PlayerNum == Controller && !photonView.IsMine)
        {
            photonView.RequestOwnership();
        }
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            if ((transform.position - currentPos).sqrMagnitude >= 10.0f * 10.0f)
            {
                transform.position = currentPos;
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, currentPos, Time.deltaTime * 10.0f);
            }
            //Debug.Log("is Not Mine");
            return;
        }
#if UNITY_EDITOR || MOUSE
        if (!Input.GetMouseButton(0) || (int)PlayerNum != Controller)
        {
            //Debug.Log("Player Num = " + PlayerNum);
            return;
        }
        //Debug.Log("Mouse Mode");
#else
        if (Input.touchCount == 0)
        {
            return;
        }
#endif
        FindTouchPosition(); //터치 포지션 특정
        StartCoroutine(TouchStriker());//하키 채 움직이기
    }

    private void FindTouchPosition() //Raycast로 하키 채가 움직일 위치 찾기
    {
#if UNITY_EDITOR || MOUSE
        TouchPos = Input.mousePosition;
#else
        TouchPos = Input.GetTouch(0).position;
#endif
        TouchVector = new Vector3(TouchPos.x, TouchPos.y, 0.0f);
        Ray TouchRay = Camera.main.ScreenPointToRay(TouchVector);//터치한 방향으로 Ray 설정

        if (Physics.Raycast(TouchRay, out RayHit, Mathf.Infinity))
        {
            if (RayHit.collider.tag == "Table")
            {

                MaxZ = RayHit.point.z;

                float zDistanceToGoal0 = Mathf.Abs(Goal[0].transform.position.z - MaxZ);

                float zDistanceToGoal1 = Mathf.Abs(Goal[1].transform.position.z - MaxZ);

                //상대팀 벽에서 더 가까우면==자기벽에서 거리가 멀면 중앙으로 고정(z축기준).
                if (PlayerNum == PlayerNumber.Player1 && zDistanceToGoal1 < zDistanceToGoal0)
                {
                    MaxZ = 0;
                }
                else if (PlayerNum == PlayerNumber.Player2 && zDistanceToGoal0 < zDistanceToGoal1)
                {
                    MaxZ = 0;
                }
                StrikerDestination = new Vector3(RayHit.point.x, RayHit.point.y + 0.05f, MaxZ);//테이블 바닥에 닿으면 위치 정보 저장
                //Debug.Log(StrikerDestination);
            }
            else if (RayHit.collider.tag == "Puck")
            {
                //퍽에 닿으면 퍽에 poke
                //RayHit.rigidbody.AddForceAtPosition(TouchRay.direction * PokeForce, RayHit.point);
            }
            //Debug.Log(StrikerDestination);
        }
        //yield return new WaitForSeconds(0.1f); //0.1초마다 호출
    }

    private IEnumerator TouchStriker() //하키 채 움직이기
    {
        StrikerRigidbody.velocity = StrikerDestination - transform.position;
        /*인전 움직이는 부분
        transform.position = Vector3.MoveTowards(transform.position, StrikerDestination, MoveSpeed * Time.deltaTime);
        _Rigidbody.AddForce(transform.position - StrikerDestination);
        */
        yield return new WaitForSeconds(0.1f); //0.1초마다 호출
    }

    #region 포톤뷰 통신부분
    private Vector3 currentPos;
    private Quaternion currentRot;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            currentPos = (Vector3)stream.ReceiveNext();
            currentRot = (Quaternion)stream.ReceiveNext();
        }
    }
    #endregion
}
