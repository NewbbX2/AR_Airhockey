using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Puck은 종합적인 공의 동작을 관리합니다.
/// 줄 36과 45을 주석처리함. 이는 생성한 벽이 좌우앞뒤 구분이 없기 때문이고, addforce를 퍽에 매번 적용시키는 것보다 마찰을
/// 0으로 만드는게 당장 에디터에서 할땐 나아서. 만약 마찰을 넣으려면 addforce값과의 균형을 맞출것 
/// Puck의 Collider에서 Material의 Friction(마찰) 값을 0.0001로 설정했음
/// </summary>
/// 
public class Puck : MonoBehaviour
{
    //볼오브젝트.
    //충돌후 속도감소(1m/s->0.9m/s)
    public float Elasticity = 0.9f;


    //볼동작
    public Vector3 Movement;
    public Rigidbody _Rigidbody;

    private GameController _GameController;

    //공 초기세팅.x, z축회전막아서 평면회전시킴.
    private void Start()
    {
        _GameController = FindObjectOfType<GameController>();

        _Rigidbody = GetComponent<Rigidbody>();
    }


    //매번 볼동작시킴.
    void Update()
    {
        //Rigidbody에 힘을 작용시켜서 볼을 동작시킴.
        //_Rigidbody.AddForce(Movement); 
        Movement = _Rigidbody.velocity;
    }

    //충돌한 오브젝트에 따라 동작.
    void OnCollisionEnter(Collision coll)
    {
        GameObject hitObject = coll.gameObject;
        Vector3 vec3;
        if (hitObject.name == "Wall_Left" || hitObject.name == "Wall_Right")
        {
            Vector3 CurrentVelocity = _Rigidbody.velocity;
            CurrentVelocity.x *= -1;
        }
        /*
        else if (hitObject.name == "Wall_Front" || hitObject.name == "Wall_Back")
        {
            Movement *= Elasticity;
            Movement.z *= -1;
        }
        */
        if (hitObject.tag == "Striker")
        {
            HockeyStriker hockeyStickInfor = hitObject.GetComponent<HockeyStriker>();
            vec3 = hockeyStickInfor.StickMoveBall() * 10f; //이정도 값을 해야 좀 속도가 났음
            _Rigidbody.AddForce(vec3);
        }
    }


    //골에 닿을시 작동. 코너 트리거 박스면 튕기는 듯한 이펙트
    private void OnTriggerEnter(Collider trigger)
    {
        if (trigger.tag == "Goal")
        {
            switch (trigger.GetComponent<GoalInf>().TeamNo)
            {
                case 1:
                    _GameController.ScoreUp(2);
                    _GameController.SpawnNewPuck(1);
                    Destroy(this.gameObject);
                    break;

                case 2:
                    _GameController.ScoreUp(1);
                    _GameController.SpawnNewPuck(2);
                    Destroy(this.gameObject);
                    break;
            }

        }
        if (trigger.tag == "Corner")
        {
            //Vector3 CurrentVelocity = _Rigidbody.velocity;
            //_Rigidbody.velocity = new Vector3(-CurrentVelocity.z, 0, -CurrentVelocity.x); //벽에 튕기는 것처럼 벡터 변경
            //Debug.Log("Hit Corner");
            //Debug.Log(trigger.transform.eulerAngles.y);
            //반사각 계산을 위한 코너 벽면의 법선 산출
            Vector3 inNormal_OfTrigger = new Vector3(Mathf.Cos(Mathf.Deg2Rad * trigger.transform.eulerAngles.y), 0, -Mathf.Sin(Mathf.Deg2Rad * trigger.transform.eulerAngles.y));
            //Debug.Log(inNormal_OfTrigger);
            //Debug.Log(_Rigidbody.velocity.ToString());
            _Rigidbody.velocity = Vector3.Reflect(_Rigidbody.velocity, inNormal_OfTrigger);//transform.position
            //Debug.Log(_Rigidbody.velocity.ToString());
        }
    }

    //골을 통과하면 공을 삭제
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Team1" || other.tag == "Team2")
        {
            Destroy(gameObject);
        }
    }



}
