using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OthelloOutput : MonoBehaviour
{
    GameObject othelloGameData = GameObject.Find("OthelloGameData");
    GameObject othelloBoardOBJECT = GameObject.Find("OthelloBoardObj");
    GameObject othelloStone = GameObject.Find("othelloStoneObjExam");

    public GameObject[,] Stone;
    int[,] othelloBoardDataBoard = new int[8, 8];
    OthelloGame OGD;
    void Start()
    {
        OGD = othelloGameData.GetComponent<OthelloGame>();
        for (int r = 0; r < 8; r++)
        {
            for (int l = 0; l < 8; l++)
            {
                othelloBoardDataBoard[r, l] = 0;
            }
        }
    }


    bool isDatachanged=true;
    

    // Update is called once per frame
    void Update()
    {
        if(isDatachanged)
        {
            for (int r = 0; r < 8; r++)
            {
                for (int l = 0; l < 8; l++)
                {
                    int newStoneTeam = OGD.othelloBoard[r, l];
                    if (othelloBoardDataBoard[r, l] != newStoneTeam)
                    {
                        if (Stone[r,l]=null)
                        {
                            createStone(r, l, newStoneTeam);

                        }
                        else if(othelloBoardDataBoard[r, l] == 0)
                        {
                            createStone(r, l, newStoneTeam);

                        }
                        else
                        {
                            changeStoneTeamTo(r, l, newStoneTeam);

                        }
                    }
                }
            }
        }
        isDatachanged=false;

    }

    void changeStoneTeamTo(int r, int l, int team)
    {
        if(team==1)
        {
            Stone[r, l].GetComponent<MeshRenderer>().material.color = Color.black;
        }
        else
        {
            Stone[r, l].GetComponent<MeshRenderer>().material.color = Color.white;

        }

    }

    //보드중앙서 보드크기의 가로(r-4)/8,세로(l-4)/8만큼 떨어진곳에
    //돌생성함.
    void createStone(int r, int l, int team)
    {
        Stone[r, l] = Instantiate(othelloStone, 
            othelloBoardOBJECT.transform.position+ 
            new Vector3(othelloBoardOBJECT.transform.localScale.x/8*(r-4),
          othelloBoardOBJECT.transform.localScale.x / 8 * (l - 4), 0), Quaternion.identity );
    }
}
