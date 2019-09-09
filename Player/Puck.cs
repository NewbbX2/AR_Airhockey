using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Puck은 종합적인 공의 동작을 관리합니다.
/// </summary>
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
        _Rigidbody.AddForce(Movement);

    }

    //충돌한 오브젝트에 따라 동작.
    void OnCollisionEnter(Collision coll)
    {
        GameObject hitObject = coll.gameObject;
        Vector3 vec3 = Movement;
        if (hitObject.name == "Wall_Left" || hitObject.name == "Wall_Right")
        {
            Movement *= Elasticity;
            Movement.x *= -1;
        }
        else if (hitObject.name == "Wall_Front" || hitObject.name == "Wall_Back")
        {
            Movement *= Elasticity;
            Movement.z *= -1;
        }
        else if (hitObject.tag == "Striker")
        {
            HockeyStriker hockeyStickInfor = hitObject.GetComponent<HockeyStriker>();
            Movement = hockeyStickInfor.StickMoveBall();
        }
    }


    //골에 닿을시 작동.
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



}
