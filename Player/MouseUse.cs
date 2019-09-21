using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseUse : MonoBehaviour
{
    #region 공개 변수들
    public GameObject Stick;//하키 채
    public float PokeForce;//찌르는 듯한 물리효과의 강도
    public Vector3 CurrentVelocity;//인스펙터에서 속도가 제대로 적용되는지 체크
    public Rigidbody StrikerRigidbody;
    #endregion
    

    #region 인스펙터에서 보기만 가능한것
    private Vector2 TouchPos;//터치된 위치
    private Vector3 TouchVector;//Ray쏠 방향
    private RaycastHit RayHit;
    private GameObject Board;
    #endregion

    #region 마우스 위치 얻어오는 변수
    private Vector3 screenPoint;
    private Vector3 offset;
    #endregion

    void Start()
    {
        GameObject Board = GameObject.FindGameObjectWithTag("Board");
        GameObject Stick = GameObject.FindGameObjectWithTag("Stick");
    }

    void Update()
    {
        if (Stick)
        {
            if (StrikerRigidbody)
            {
                MoveStickToMousePos();
            }
            else { Debug.Log("StrikerRigidbody is not exist"); }
        }
        else if (!Stick)
        {
            Debug.Log("Stick is not exist");
        }
    }

    void MoveStickToMousePos()
    {
        Vector3 CursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);//screenPoint.z
        Ray TouchRay = Camera.main.ScreenPointToRay(CursorScreenPoint);//터치한 방향으로 레이저
        if (Physics.Raycast(TouchRay, out RayHit, Mathf.Infinity))
        {
            if (RayHit.collider.tag == "Table")//보드에 닿으면 위치로 스틱 움직이기
            {
                Vector3 StickVelocity;
                StickVelocity = (RayHit.point - Stick.transform.position); //속도는 거리에 비례함
                StrikerRigidbody.velocity = StickVelocity; //속도 적용
                CurrentVelocity = StrikerRigidbody.velocity;//바디의 속도 체크
            }
            else if (RayHit.collider.tag == "Puck")
            {
                RayHit.rigidbody.AddForceAtPosition(TouchRay.direction * PokeForce, RayHit.point);//퍽에 닿으면 퍽에 poke
            }
        }
        //yield return null;최적화를 위해선 여기서 raycast제한을 설정할 것
    }
}
