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

    private bool isTouched = false;


    private Vector2 TouchPos;//터치된 위치
    private Vector3 TouchVector;//Ray쏠 방향
    private RaycastHit RayHit;
    private GameObject HockeyBoard;
    private Vector3 StickDestination;
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
            if ((transform.position - currentPos).sqrMagnitude <= 10.0f * 10.0f)
            {
                transform.position = currentPos;
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, currentPos,  10.0f);
            }
            return;
        }
#if UNITY_EDITOR || MOUSE
        if (!Input.GetMouseButton(0) || (int)PlayerNum != Controller)
        {
            return;
        }
#else
        if (Input.touchCount == 0)
        {
            return;
        }
#endif
        FindTouchPosition(); //터치 포지션 특정
        StartCoroutine(TouchStick());//하키 채 움직이기
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
                 
                if ( (PlayerNum == PlayerNumber.Player1) && MaxZ >= MiddlePointZ) //하키가 중앙선 못넘게
                {
                    MaxZ = MiddlePointZ;
                } 
                else if((PlayerNum == PlayerNumber.Player2) && MaxZ <= MiddlePointZ)
                {
                    MaxZ = MiddlePointZ;
                }

                StickDestination = new Vector3(RayHit.point.x, RayHit.point.y + 0.5f, MaxZ);//테이블 바닥에 닿으면 위치 정보 저장
            }
            else
            {
            }
        }
    }

    private IEnumerator TouchStick() //하키 채 움직이기
    {
        StrikerRigidbody.velocity = (StickDestination - transform.position).normalized*10.0f;
        /*인전 움직이는 부분
        transform.position = Vector3.MoveTowards(transform.position, StickDestination, MoveSpeed * Time.deltaTime);
        _Rigidbody.AddForce(transform.position - StickDestination);
        */
        yield return new WaitForSeconds(0.1f); //0.1초마다 호출
    }

    #region 포톤뷰 통신부분
    private Vector3 currentPos;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            currentPos = (Vector3)stream.ReceiveNext();
        }
    }
    #endregion
}
