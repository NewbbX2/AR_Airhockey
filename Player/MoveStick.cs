using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStick : MonoBehaviour
{
    #region 공개 변수들
    public GameObject Stick;//하키 채
    public float PokeForce;//찌르는 듯한 물리효과의 강도
    #endregion

    #region 인스펙터에서 보기만 가능한것
    private Vector2 TouchPos;//터치된 위치
    private Vector3 TouchVector;//Ray쏠 방향
    private RaycastHit RayHit;
    private GameObject Board;
    #endregion

    void Start()
    {
        GameObject Board = GameObject.FindGameObjectWithTag("Board");
    }

    void Update()
    {
        #region 화면 한곳에만 터치 되었을 때 작동
        if (Input.touchCount == 1)
        {
            TouchPos = Input.GetTouch(0).position;   
            TouchVector = new Vector3(TouchPos.x, TouchPos.y, 0.0f);   
            Ray TouchRay = Camera.main.ScreenPointToRay(TouchVector);//터치한 방향으로 레이저

            if (Physics.Raycast(TouchRay, out RayHit, Mathf.Infinity))
            {
                if (RayHit.collider.tag == "Board")
                {
                    TouchStick(RayHit.point);//보드에 닿으면 위치로 스틱 움직이기
                }
                else if(RayHit.collider.tag == "Puck")
                {
                    RayHit.rigidbody.AddForceAtPosition(TouchRay.direction * PokeForce, RayHit.point);//퍽에 닿으면 퍽에 poke
                }
            }
        }
        #endregion
    }

    private void TouchStick(Vector3 HitPoint) //하키 채 움직이기
    {
        transform.position = new Vector3(HitPoint.x, HitPoint.y, HitPoint.z);
    }
}
