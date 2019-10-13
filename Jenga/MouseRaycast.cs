using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRaycast : MonoBehaviour
{
    #region 공개 변수들
    public float PokeForce;
    #endregion

    #region 마우스 위치 얻어오는 변수
    private Vector3 MousePointPos;
    #endregion

    void Update()
    {
        Move();
    }

    void Move()
    {
        //마우스 위치를 기반으로 선 생성
        Vector3 mouse = Input.mousePosition;
        Ray CastPoint = Camera.main.ScreenPointToRay(mouse);

        RaycastHit RayHit; //Ray가 무엇에 맞았는가
        if (Input.GetMouseButtonDown(0))//마우스 왼쪽 버튼 클릭
         {
        if (Physics.Raycast(CastPoint, out RayHit, Mathf.Infinity))
        {
            if (RayHit.collider.tag == "Board")
            {
                
            }
            else if (RayHit.collider.tag == "Stick")
            {
                RayHit.rigidbody.AddForceAtPosition(CastPoint.direction * PokeForce, RayHit.point);//Ray를 쏜 방향으로 맞은 포인트에 힘을 ㄱ ㅏ함
            }
        }
         }
        new WaitForSeconds(.1f); //연달아 클릭 방지
    }
}
