using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// Puck은 종합적인 공의 동작을 관리합니다.
/// 마찰을 넣으려면 addforce값과의 균형을 맞출것 
/// Puck의 Collider에서 Material의 Friction(마찰) 값을 0.0001로 설정했음. puck의 mass는 0.1로 설정
/// </summary>
public class Puck : MonoBehaviourPunCallbacks, IPunObservable
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
        if (!photonView.IsMine)
        {
            return;
        }
        Movement = _Rigidbody.velocity; //에디터 상에서 움직임 확인 위해서, 이제 움직임을 주는데 쓰지 않음
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
            vec3 = hockeyStickInfor.StickMoveBall() * 10f; //이정도 값을 해야 좀 속도가 났음,
            _Rigidbody.AddForce(vec3);
        }
    }


    //골에 닿을시 작동. 코너 트리거 박스면 튕기는 듯한 이펙트
    private void OnTriggerEnter(Collider trigger)
    {
        /*
        if (trigger.tag == "Goal")
        {
            if (trigger.GetComponent<GoalInf>().TeamNo == 0)
            {
                GameController gameOBJ = GameObject.Find("GameOBJ").GetComponent<GameController>();
                gameOBJ.Score[1] += 1;
                gameOBJ.SpawnNewBall(new Vector3(0, 1F, 0), new Vector3(0, 0, 10));
                RigidBody_Ball.constraints = RigidbodyConstraints.None;
            }
            else
            {
                GameController gameOBJ = GameObject.Find("GameOBJ").GetComponent<GameController>();
                gameOBJ.Score[0] += 1;
                gameOBJ.SpawnNewBall(new Vector3(0, 1F, 0), new Vector3(0, 0, -10));
                RigidBody_Ball.constraints = RigidbodyConstraints.None;

            }

        }
        */
        if (trigger.tag == "Corner")
        {
            //반사각 계산을 위한 코너 벽면의 법선 산출
            Vector3 inNormal_OfTrigger = new Vector3(Mathf.Cos(Mathf.Deg2Rad * trigger.transform.eulerAngles.y), 0, -Mathf.Sin(Mathf.Deg2Rad * trigger.transform.eulerAngles.y));
            _Rigidbody.velocity = Vector3.Reflect(_Rigidbody.velocity, inNormal_OfTrigger);//물체가 반사되는 듯한 효과
        }
        switch (trigger.tag)
        {
            case "Team1":
                _GameController.ScoreUp(2);
                _GameController.SpawnNewPuck(1);
                break;

            case "Team2":
                _GameController.ScoreUp(1);
                _GameController.SpawnNewPuck(2);
                break;
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

    #region 포톤뷰 통신부분
    private Vector3 currentPos;
    private Quaternion currentRot;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            currentPos = (Vector3)stream.ReceiveNext();
            currentRot = (Quaternion)stream.ReceiveNext();
        }
    }
    #endregion

}