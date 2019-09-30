using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveByMarker : MonoBehaviour
{
    #region 공개 변수들
    public GameObject Stick;//하키 채
    public float PokeForce;//찌르는 듯한 물리효과의 강도
    public Rigidbody StrikerRigidbody;//하키채의 물리바디
    public GameObject Marker;//이미지 마커
    #endregion

    #region 인스펙터에서 보기만 가능한것
    private Vector2 TouchPos;//터치된 위치
    private RaycastHit RayHit;
    private GameObject HockeyBoard;
    private Vector3 StickDestination;
    private bool RaycastOn;//RaycastOn가 False면 raycast비활성화
    #endregion

    private float MaxZ;

    void Start()
    {
        HockeyBoard = GameObject.FindGameObjectWithTag("Board");
        GameObject Puck = GameObject.FindGameObjectWithTag("Puck");
        RaycastOn = true;
    }

    void Update()
    {
        if (Input.touchCount == 1 && RaycastOn)
        {
            if (Stick)
            {
                if (StrikerRigidbody)
                {
                    FindTouchPosition();
                    MoveStick();
                }
                else { Debug.Log("StrikerRigidbody is not exist"); }
            }
            else if (!Stick)
            {
                Debug.Log("Stick is not exist");
            }
            
        }
    }

    private void MoveStick() //하키 채 움직이기
    {
        Vector3 StickVelocity;
        StickVelocity = (StickDestination - Stick.transform.position);
        StrikerRigidbody.velocity = StickVelocity;
        Debug.Log("MoveStick Success");
    }

    private void FindTouchPosition() //Raycast로 하키 채가 움직일 위치 찾기
    {
        Ray MarkerRayCast = Camera.main.ScreenPointToRay(Marker.transform.position); //마커(이미지 타겟)가 생성한 오브젝트 위치로 Ray쏨

        if (Physics.Raycast(MarkerRayCast, out RayHit, Mathf.Infinity))
        {
            Debug.Log("Raycast Success");
            if (RayHit.collider.tag == "Board" || RayHit.collider.tag == "Table")
            {
                if (RayHit.point.z >= 0) //하키가 중앙선 못넘게
                {
                    MaxZ = 0;
                }
                else
                {
                    MaxZ = RayHit.point.z;
                }
                StickDestination = new Vector3(RayHit.point.x, 0.05f, MaxZ); ;//보드에 닿으면 위치 정보 저장
                Debug.Log("StickDestination Success");
            }
            else if (RayHit.collider.tag == "Puck")
            {
                RayHit.rigidbody.AddForceAtPosition(MarkerRayCast.direction * PokeForce, RayHit.point);//퍽에 닿으면 퍽에 poke
            }
        }

        new WaitForSeconds(.1f); //0.1초마다 호출
    }

}
