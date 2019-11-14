#define MOUSE
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// 하키 Striker를 조작하기 위한 클래스
/// </summary>
/// 
enum PlayerNumber
{
    Player1,
    Player2,
}

public class MoveStriker : MonoBehaviourPunCallbacks, IPunObservable
{
    #region 공개 변수들
    public float PokeForce = 5.0f;//찌르는 듯한 물리효과의 강도
    public float Speed = 4.0f; // 움직이는 기본 속도
    //public GameObject MiddlePoint; //경기장 중앙 지점
    //[Range(0, 1)] public int Controller;
    #endregion

    #region 내부 변수
    private Vector2 TouchPos;//터치된 위치
    private Vector3 TouchVector;//Ray쏠 방향
    private RaycastHit RayHit;
    private GameObject HockeyBoard;
    private Vector3 StrikerDestination;
    //private PlayerNumber PlayerNum;
    private ARHockeyGameController GameController;
    private float MaxZ;
    private Rigidbody StrikerRigidbody;
    #endregion


    private void Start()
    {
        GameController = FindObjectOfType<ARHockeyGameController>();

        /*
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
        */
    }

    private void Update()
    {
        StrikerRigidbody = GetComponent<Rigidbody>();
        //목표점에 도달했다고 판단되면 정지
        if ((StrikerDestination - transform.position).magnitude < 0.1)
        {
            StrikerRigidbody.velocity = Vector3.zero;
        }
        if (!photonView.IsMine)
        {
            if ((transform.position - currentPos).sqrMagnitude >= 10.0f * 10.0f)
            {
                //transform.position = currentPos;
                StrikerRigidbody.MovePosition(currentPos);
            }
            else
            {
                //transform.position = Vector3.Lerp(transform.position, currentPos, Time.deltaTime * 10.0f);
                StrikerRigidbody.MovePosition(Vector3.Lerp(transform.position, currentPos, Time.deltaTime * 10.0f));
            }
            return;
        }
#if !UNITY_EDITOR || !MOUSE
        if (Input.touchCount == 0 || !photonView.IsMine)//(int)PlayerNum != Controller)
        {
            return;
        }

#else
        if (!Input.GetMouseButton(0) || !photonView.IsMine)//(int)PlayerNum != Controller)
        {
            return;
        }
#endif

        FindTouchPosition(); //터치 포지션 특정
        StartCoroutine(StrikerVelocity());//하키 채 움직이기
    }

    private void FindTouchPosition() //Raycast로 하키 채가 움직일 위치 찾기
    {
#if !UNITY_EDITOR || !MOUSE
        TouchPos = Input.GetTouch(0).position;
#else
        TouchPos = Input.mousePosition;        
#endif
        //Debug.Log(TouchPos);
        TouchVector = new Vector3(TouchPos.x, TouchPos.y, 0.0f);
        Ray TouchRay = Camera.main.ScreenPointToRay(TouchVector);//터치한 방향으로 Ray 설정

        if (Physics.Raycast(TouchRay, out RayHit, Mathf.Infinity))
        {
            if (RayHit.collider.tag == "Table")
            {
                StrikerDestination = new Vector3(RayHit.point.x, transform.position.y, MaxZ); ;//테이블 바닥에 닿으면 위치 정보 저장
                MaxZ = RayHit.point.z;
            }
            //Debug.Log(StrikerDestination);
        }
    }

    private IEnumerator StrikerVelocity() //하키 채 움직이기
    {
        Vector3 vec = (StrikerDestination - transform.position);

        if ((StrikerDestination - transform.position).magnitude < 0.2)
        {
            //목표지점이 너무 가까우면 속도적용 안함. 클릭하고 있을시 진동하는거 방지
        }
        else
        {
            float VecSize = vec.magnitude;
            // 수정된 속력 = (기본속도) + (거리에 따른 속도 보너스)
            float FizedSize = Speed + ((0.5f) * VecSize);
            StrikerRigidbody.velocity = vec * (FizedSize / VecSize);
        }
        /*
        if (vec.magnitude<=1)
        {
            Vector3.Normalize(vec);
        }
        StrikerRigidbody.velocity = vec;
        */
        yield return new WaitForSeconds(0.05f); //0.05초마다 호출
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
