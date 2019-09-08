using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{


    //시작전 세팅
    //EmptyObject의 name을 GameOBJ로 수정한뒤, 이 스크립트를 달아준다.




    /*
    //이 스크립트의 역할
       //Ball과 Playercharacter를 관리할 것이다.
           //Playercharacter는 스틱을 관리할 것이다.
       //Ball은 종합적인 공의 동작을 관리한다.
    //Score를 관리한다.
         //정확히는 저장하는 것이며 Palyer에서 이 저장된 점수를 표기할것이다.
    */
    


    //공
    public GameObject BallObject;
    //점수
    public int[] Score = new int[2];

    void Start()
    {
        //플레이어 생성
        CreatePlayer(1); CreatePlayer(2);
        //첫 골을 스폰.
        SpawnNewBall(new Vector3(0, 1F, 0), new Vector3(0, 0, 10));
    }


    //플레이어번호 n인 플레이어 만듬.
    void CreatePlayer(int n)
    {
        GameObject player = new GameObject();
        player.AddComponent<PlayerCharacter>();
        player.name = "player" + n;
        player.GetComponent<PlayerCharacter>().playerNo = n;
    }


    //일단은 스피어(납작) 만드는식인데
    //실제 디자인 끝나면 주석으로 대체
    public void SpawnNewBall(Vector3 spawnPosition, Vector3 ballDirection)
    {
        //볼을 스폰하는 방식.(볼 제작 완료시 아래방식 사용)
        BallObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        BallObject.transform.position = spawnPosition;
        BallObject.transform.localScale = new Vector3(0.5f, 0.1f, 0.5f);
        BallObject.AddComponent<Ball>();
        BallObject.name = "HockeyBall";
        BallObject.tag = "Ball";


        //볼을 복제하는 방식.
        BallObject = Instantiate(GameObject.Find("Ball"), spawnPosition, Quaternion.identity);


        //공통, 방향지정해서 움직이기.
        BallObject.GetComponent<Ball>().BallMoveMent = ballDirection;
    }
}










