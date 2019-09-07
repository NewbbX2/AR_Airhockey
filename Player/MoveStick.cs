using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStick : MonoBehaviour
{
    #region 공개 변수들
    public float PokeForce = 5.0f;//찌르는 듯한 물리효과의 강도
    #endregion

    #region 내부 변수
    private Vector2 TouchPos;//터치된 위치
    private Vector3 TouchVector;//Ray쏠 방향
    private RaycastHit RayHit;
    private GameObject HockeyBoard;
    private Vector3 StickDestination;
    private bool RaycastOn;//RaycastOn가 False면 raycast비활성화
    private float MaxZ;
    #endregion



    void Start()
    {
        RaycastOn = true;
    }

    void Update()
    {
        if (Input.touchCount == 1 && RaycastOn)
        {
            FindTouchPosition();
            TouchStick();//하키 채 움직이기
        }
    }

    private void TouchStick() //하키 채 움직이기
    {
        MaxZ = RayHit.point.z;
        if (MaxZ >= 0) //하키가 중앙선 못넘게
        {
            MaxZ = 0;
        }
        transform.position = Vector3.MoveTowards(transform.position, StickDestination, 3 * Time.deltaTime);
    }

    private void FindTouchPosition() //Raycast로 하키 채가 움직일 위치 찾기
    {
        TouchPos = Input.GetTouch(0).position;
        TouchVector = new Vector3(TouchPos.x, TouchPos.y, 0.0f);
        Ray TouchRay = Camera.main.ScreenPointToRay(TouchVector);//터치한 방향으로 레이저

        if (Physics.Raycast(TouchRay, out RayHit, Mathf.Infinity))
        {
            if (RayHit.collider.tag == "Table")
            {
                StickDestination = new Vector3(RayHit.point.x, 0.05f, MaxZ); ;//보드에 닿으면 위치 정보 저장
                MaxZ = RayHit.point.z;
            }
            else if (RayHit.collider.tag == "Puck")
            {
                RayHit.rigidbody.AddForceAtPosition(TouchRay.direction * PokeForce, RayHit.point);//퍽에 닿으면 퍽에 poke
            }
        }

        new WaitForSeconds(.1f); //0.1초마다 호출
    }

}
