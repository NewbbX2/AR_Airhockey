using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    public GameObject BallObjectSelf;
    public Vector3 BallMoveMent;
    public Rigidbody RigidBody_Ball;
    public float Elasticity = 0.9f;

    //공 초기세팅. z축회전막아서 평면회전시킴.
    private void Start()
    {
        BallObjectSelf = GameObject.Find("HockeyBall");
        BallObjectSelf.tag = "Ball";
        RigidBody_Ball = BallObjectSelf.AddComponent<Rigidbody>();
        RigidBody_Ball.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }


    //매번 볼동작시킴.
    void Update()
    {
        if (BallObjectSelf == null) { BallObjectSelf = GameObject.Find("HockeyBall"); }
        if (RigidBody_Ball == null)
        {
            RigidBody_Ball = BallObjectSelf.AddComponent<Rigidbody>();
            RigidBody_Ball.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        RigidBody_Ball.AddForce(BallMoveMent);

    }
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

    private void OnTriggerEnter(Collider trigger)
    {


        if (trigger.tag == "Goal")
        {
            if (trigger.GetComponent<GoalInf>().TeamNo == 0)
            {
                Game gameOBJ = GameObject.Find("Controller").GetComponent<Game>();
                gameOBJ.Score[1] += 1;
                gameOBJ.SpawnNewBall(new Vector3(0, 1F, 0), new Vector3(0, 0, 10));
                RigidBody_Ball.constraints = RigidbodyConstraints.None;
            }
            else
            {
                Game gameOBJ = GameObject.Find("Controller").GetComponent<Game>();
                gameOBJ.Score[0] += 1;
                gameOBJ.SpawnNewBall(new Vector3(0, 1F, 0), new Vector3(0, 0, -10));
                RigidBody_Ball.constraints = RigidbodyConstraints.None;

            }

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Goal")
        {
            DestroyBall();

        }
    }


    public void DestroyBall()
    {
        Destroy(BallObjectSelf);
    }



}
