using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    //볼오브젝트.
    public GameObject BallObjectSelf;
    //충돌후 속도감소(1m/s->0.9m/s)
    public float Elasticity = 0.9f;





    //볼동작
    public Vector3 BallMoveMent;
    public Rigidbody RigidBody_Ball;

    //공 초기세팅.x, z축회전막아서 평면회전시킴.
    private void Start()
    {
        //복제공을 자신으로 인식.
        BallObjectSelf = GameObject.Find("HockeyBallCopyed");
        /*이 두개는 프리팹에서 지정해주었다면(이미 만들어두었다면)생략가능)*/
        RigidBody_Ball = BallObjectSelf.AddComponent<Rigidbody>();
        RigidBody_Ball.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }


    //매번 볼동작시킴.
    void Update()
    {
        //볼 삭제시, 새로 스폰된 공이 삭제된 공을 자신으로 인식하는 경우가 발생, 이를 대비해서,
        //삭제된 공을 자신으로 여기는 경우를 대비해 다시 공을 찾기.
        if (BallObjectSelf == null)
        {
            BallObjectSelf = GameObject.Find("HockeyBallCopyed");

        }
        /*위와 동일, 그러나 아래코드는 프리팹 완성시 필요없다.*/
        if (RigidBody_Ball == null)
        {
            RigidBody_Ball = BallObjectSelf.AddComponent<Rigidbody>();
            RigidBody_Ball.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
        //중요, 볼을 동작시킴.
        RigidBody_Ball.AddForce(BallMoveMent);

    }

    //충돌한 오브젝트에 따라 동작.
    void OnCollisionEnter(Collision coll)
    {
        GameObject colldedOBJ = coll.gameObject;
        Vector3 vec3 = BallMoveMent;
        if (colldedOBJ.name == "Wall_Left" || colldedOBJ.name == "Wall_Right")
        {
            BallMoveMent *= Elasticity;
            BallMoveMent.x *= -1;
        }
        else if (colldedOBJ.name == "Wall_Front" || colldedOBJ.name == "Wall_Back")
        {
            BallMoveMent *= Elasticity;
            BallMoveMent.z *= -1;
        }
        else if (colldedOBJ.tag == "Puck")
        {
            HockeyStickInf hockeyStickInfor = colldedOBJ.GetComponent<HockeyStickInf>();
            BallMoveMent = hockeyStickInfor.StickMoveBall();
        }
    }


    //골에 닿을시 작동.
    private void OnTriggerEnter(Collider trigger)
    {


        if (trigger.tag == "Goal")
        {
            if (trigger.GetComponent<GoalInf>().TeamNo == 0)
            {
                Game gameOBJ = GameObject.Find("GameOBJ").GetComponent<Game>();
                gameOBJ.Score[1] += 1;
                gameOBJ.SpawnNewBall(new Vector3(0, 1F, 0), new Vector3(0, 0, 10));
                RigidBody_Ball.constraints = RigidbodyConstraints.None;
            }
            else
            {
                Game gameOBJ = GameObject.Find("GameOBJ").GetComponent<Game>();
                gameOBJ.Score[0] += 1;
                gameOBJ.SpawnNewBall(new Vector3(0, 1F, 0), new Vector3(0, 0, -10));
                RigidBody_Ball.constraints = RigidbodyConstraints.None;

            }

        }
    }

    //골을 통과하면 이제 공을 삭제하고 새로 부를것이다.
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Goal")
        {
            Destroy(BallObjectSelf);
        }
    }



}
