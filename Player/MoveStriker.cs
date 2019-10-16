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
    public GameObject MiddlePoint; //경기장 중앙 지점
    [Range(0, 1)] public int Controller;
    #endregion

    #region 내부 변수
    private Vector2 TouchPos;//터치된 위치
    private Vector3 TouchVector;//Ray쏠 방향
    private RaycastHit RayHit;
    private GameObject HockeyBoard;
    private Vector3 StrikerDestination;
    private PlayerNumber PlayerNum;
    private ARHockeyGameController GameController;
    private float MaxZ;
    private Rigidbody StrikerRigidbody;
    #endregion


    private void Start()
    {
        GameController = FindObjectOfType<ARHockeyGameController>();

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

    private void Update()
    {
        StrikerRigidbody = GetComponent<Rigidbody>();
        if (!photonView.IsMine && GameController.IsPhotonConnected)
        {
            if ((transform.position - currentPos).sqrMagnitude >= 10.0f * 10.0f)
            {
                transform.position = currentPos;
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, currentPos, Time.deltaTime * 10.0f);
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
                StrikerDestination = new Vector3(RayHit.point.x, RayHit.point.y, MaxZ); ;//테이블 바닥에 닿으면 위치 정보 저장
                MaxZ = RayHit.point.z;
            }
            //Debug.Log(StrikerDestination);
        }
    }

    private IEnumerator TouchStriker() //하키 채 움직이기
    {
        Vector3 vec = (StrikerDestination - transform.position);
        if(vec.magnitude<=1)
        {
            vec = vec.normalized;
        }
        StrikerRigidbody.velocity = vec;
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
