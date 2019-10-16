using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//골에 붙는 정보.
public class GoalInf : MonoBehaviour
{
    [Range(0,1)]public int TeamNo;
    private ARHockeyGameController GameController;

    private void Start()
    {
        GameController = FindObjectOfType<ARHockeyGameController>();
    }

    public void InGoal(GameObject puck)
    {
        GameController.JudgmentGoal(TeamNo, puck);
    }
}
