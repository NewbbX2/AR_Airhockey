using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 하키 Striker를 조작하기 위한 클래스
/// </summary>
public class MoveStriker : MonoBehaviour
{
    #region 공개 변수들
    public float PokeForce = 5.0f;//찌르는 듯한 물리효과의 강도
    public float MoveSpeed = 20.0f; // 이동속도
    public GameObject MiddlePoint; //경기장 중앙 지점
    public Rigidbody StrikerRigidbody;
    #endregion

    #region 내부 변수
    private Vector2 TouchPos;//터치된 위치
    private Vector3 TouchVector;//Ray쏠 방향
    private RaycastHit RayHit;
    private GameObject HockeyBoard;
    private Vector3 StickDestination;
    private float MaxZ;
    private float MiddlePointZ; //경기장 중앙 지점의 Z좌표값
    #endregion



    void Start()
    {
        GameObject MiddlePoint = GameObject.FindGameObjectWithTag("MiddlePoint");
        MiddlePointZ = MiddlePoint.transform.position.z;
    }

    void Update()
    {
#if UNITY_EDITOR
        if (!Input.GetMouseButton(0))
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
#if UNITY_EDITOR
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
                if (MaxZ >= MiddlePointZ) //하키가 중앙선 못넘게
                {
                    MaxZ = MiddlePointZ;
                }
                StickDestination = new Vector3(RayHit.point.x, RayHit.point.y + 0.05f, MaxZ);//테이블 바닥에 닿으면 위치 정보 저장
            }
            else if (RayHit.collider.tag == "Puck")
            {
                //퍽에 닿으면 퍽에 poke
                //RayHit.rigidbody.AddForceAtPosition(TouchRay.direction * PokeForce, RayHit.point);
            }
            //Debug.Log(StickDestination);
        }
        yield return new WaitForSeconds(0.1f); //0.1초마다 호출
    }

    private IEnumerator TouchStick() //하키 채 움직이기
    {
        StrikerRigidbody.velocity = StickDestination - transform.position ;
        /*인전 움직이는 부분
        transform.position = Vector3.MoveTowards(transform.position, StickDestination, MoveSpeed * Time.deltaTime);
        _Rigidbody.AddForce(transform.position - StickDestination);
        */
        yield return new WaitForSeconds(0.1f); //0.1초마다 호출
    }



}
