using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OthelloBoardBluck : MonoBehaviour
{
    public int R, L, TeamNo=0;
    private GameObject StoneObject;

   

   public void SetBoardBluckColor(Color colo)
    {
        GetComponent<MeshRenderer>().material.color = colo;
    }


    public void CreateNewStone(int Team, Color colo)
    {
        TeamNo = Team;
        StoneObject = GameObject.Find("StoneObjectBase");
        GameObject temp=Instantiate(StoneObject, transform.position + new Vector3(0f, 0.5f, 0f), Quaternion.identity);
        StoneObject = temp;
        StoneObject.name = "Stone" + R + L;
        StoneObject.GetComponent<MeshRenderer>().material.color = colo;
    }

    public void ChangeStoneTeam(int Team, Color colo)
    {
        TeamNo = Team;
        StoneObject.GetComponent<MeshRenderer>().material.color = colo;
    }

}
