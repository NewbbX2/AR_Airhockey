using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OthelloGameController : MonoBehaviour
{
    public const int MaxR = 8, MaxL = 8;
    public bool IsOthelloGameActivated = false;
    public bool doOnceAtStart = true;
    public Color Color1 = Color.blue;
    public Color Color2 = Color.red;
    public Color[] StoneColor = new Color[2];

    private void Start()
    {
        StoneColor[0] = Color.black;
        StoneColor[1] = Color.white; Color1 = Color.blue; Color2 = Color.red;
    }

    //보드의 해당 칸이며, 그 위의 말을 관리하는 OthelloboardBlockOBJ
    OthelloBoardBluck[,] __OthelloBoardBlock = new OthelloBoardBluck[MaxR, MaxL];
    int[] Score = new int[2];
    int Turn = 0;
    bool SkipTurn = false;



    void Update()
    {
        if (IsOthelloGameActivated)
        {

            if (doOnceAtStart)
            {
                doOnceAtStart = false;
                OthelloGameStart();

            }







        }
    }

    void OthelloGameStart()
    {
        SetBoardBlucks();
        startNextTurn(SkipTurn);






        void SetBoardBlucks()
        {

            GameObject[] obo = GameObject.FindGameObjectsWithTag("BoardBluck");

            for (int rr = 0; rr < 8; rr++)
            {
                for (int ll = 0; ll < 8; ll++)
                {
                    OthelloBoardBluck obbCS;
                    for (int N = 0; N < 64; N++)
                    {
                        obbCS = obo[N].GetComponent<OthelloBoardBluck>();
                        if ((obbCS.R == rr) && (obbCS.L == ll))
                        {
                            __OthelloBoardBlock[rr, ll] = obbCS;
                            __OthelloBoardBlock[rr, ll].TeamNo = 0;
                            if ((rr + ll) % 2 == 0) { __OthelloBoardBlock[rr, ll].SetBoardBluckColor(Color1); }
                            else { __OthelloBoardBlock[rr, ll].SetBoardBluckColor(Color2); }
                        }
                    }
                }
            }
            __OthelloBoardBlock[3, 3].CreateNewStone(1, StoneColor[0]);
            __OthelloBoardBlock[4, 4].CreateNewStone(1, StoneColor[0]);
            __OthelloBoardBlock[4, 3].CreateNewStone(2, StoneColor[1]);
            __OthelloBoardBlock[3, 4].CreateNewStone(2, StoneColor[1]);

        }



        //클릭시 해당좌표에서 

    }


    void startNextTurn(bool skippedPreTurn)
    {
        Turn++;
        if (Check_CanDoAnyThingInThisTrun())
        {
            SkipTurn = true;
        }
        else
        {
            if (skippedPreTurn)
            {
                //  endGame();
            }
            else
            {
                startNextTurn(true);
                SkipTurn = false;
            }
        }
    }

    bool Check_CanDoAnyThingInThisTrun()
    {
        for (int r = 0; r < 8; r++)
        {
            for (int l = 0; l < 8; l++)
            {
                if (checkCanPutAnyNewStoneOnRL(r, l))
                {
                    return true;
                }
            }
        }
        return false;
    }




    //이 장소에 둘수있는지 체크.
    bool checkCanPutAnyNewStoneOnRL(int r, int l)
    {
        if (__OthelloBoardBlock[r, l].TeamNo == 0)
        {
            int playingTeamNo = 2 - (Turn % 2);
            if (r >= 2)
            {
                if (__OthelloBoardBlock[r - 1, l].TeamNo == 2 - playingTeamNo)
                {//왼쪽에 반대색
                    if (ChecKStoneTeamFromRLToRL(r - 2, l, -1, 0, playingTeamNo)) { return true; }
                }//그걸 감싸는돌있음
            }
            if (r <= 5)
            {
                if (__OthelloBoardBlock[r + 1, l].TeamNo == 2 - playingTeamNo)
                {
                    if (ChecKStoneTeamFromRLToRL(r + 2, l, 1, 0, playingTeamNo)) { return true; }
                }
            }

            if (l >= 2)
            {
                if (__OthelloBoardBlock[r, l - 1].TeamNo == 2 - playingTeamNo)
                {
                    if (ChecKStoneTeamFromRLToRL(r, l - 2, 0, -1, playingTeamNo)) { return true; }
                }
            }
            if (l <= 5)
            {
                if (__OthelloBoardBlock[r, l + 1].TeamNo == 2 - playingTeamNo)
                {
                    if (ChecKStoneTeamFromRLToRL(r, l + 2, 0, 1, playingTeamNo)) { return true; }
                }
            }

            if (r >= 2 && l >= 2)
            {
                if (__OthelloBoardBlock[r - 1, l - 1].TeamNo == 2 - playingTeamNo)
                {
                    if (ChecKStoneTeamFromRLToRL(r - 2, l - 2, -1, -1, playingTeamNo)) { return true; }
                }
            }

            if (r >= 2 && l <= 5)
            {
                if (__OthelloBoardBlock[r - 1, l + 1].TeamNo == 2 - playingTeamNo)
                {
                    if (ChecKStoneTeamFromRLToRL(r - 2, l + 2, -1, +1, playingTeamNo)) { return true; }
                }
            }

            if (r <= 5 && l >= 2)
            {
                if (__OthelloBoardBlock[r + 1, l - 1].TeamNo == 2 - playingTeamNo)
                {
                    if (ChecKStoneTeamFromRLToRL(r + 2, l - 2, +1, -1, playingTeamNo)) { return true; }
                }
            }

            if (r <= 5 && l <= 5)
            {
                if (__OthelloBoardBlock[r + 1, l + 1].TeamNo == 2 - playingTeamNo)
                {
                    if (ChecKStoneTeamFromRLToRL(r + 2, l + 2, +1, +1, playingTeamNo)) { return true; }
                }
            }
        }
        return false;








        bool ChecKStoneTeamFromRLToRL(int rr, int ll, int rIncreases, int lIncreases, int teamNo)
        {
            for (; rr < MaxR && rr >= 0 && ll < MaxL && ll >= 0; rr += rIncreases, ll += lIncreases)
            {
                if (__OthelloBoardBlock[rr, ll].TeamNo == teamNo) { return true; }
            }
            return false;
        }

    }







    public void GetInputOn(OthelloBoardBluck obb)
    {
        if (obb.TeamNo == 0)
        {
            TryToPutNewStone(obb);
        }
    }

    bool check = false;
    int turn = 2;
    void TryToPutNewStone(OthelloBoardBluck obb)
    {
        int r = obb.R, l = obb.L;
        if (true
            // checkCanPutAnyNewStoneOnRL(r, l)
            )
        {
            TransTeamAfterPutNewStone(turn, r, l);
        }
    }

    public Text ScoreText;

    //생성시, 바뀌는돌의 팀을 바꿈.
    void TransTeamAfterPutNewStone(int Team, int R, int L)
    {
        if (MaxR - R > 2)
        {
            if (__OthelloBoardBlock[R + 1, L].TeamNo == (3 - Team))
            {
                //그보다 아래에             아군이 있을시 감싸서 팀병경..
                for (int r = R + 2, l = L; r < MaxR; r++)
                {
                    if (__OthelloBoardBlock[r, l].TeamNo == Team)
                    {
                        //바로 왼쪽부터, 아군전까지 팀변경
                        //경계 : 기존 내돌, 새로둘돌.
                        //r,R,l,L 경계제외하고 바꿀것이다
                        ChangeStoneTeam(Team, r, R, l, L);
                        //for문 탈출
                        r = MaxR;
                         check = true;
                    }
                }
            }
        }
        if (MaxL - L > 2)
        {
            if (__OthelloBoardBlock[R, L + 1].TeamNo == (3 - Team))
            {
                //그보다 아래에             아군이 있을시 감싸서 팀병경..
                for (int r = R, l = L + 2; l < MaxL; l++)
                {
                    if (__OthelloBoardBlock[r, l].TeamNo == Team)
                    {
                        //바로 왼쪽부터, 아군전까지 팀변경
                        //경계 : 기존 내돌, 새로둘돌.
                        //r,R,l,L 경계제외하고 바꿀것이다
                        ChangeStoneTeam(Team, r, R, l, L);
                        //for문 탈출
                        l = MaxL;
                        check = true;
                    }
                }
            }
        }
        if (R > 2)
        {
            if (__OthelloBoardBlock[R - 1, L].TeamNo == (3 - Team))
            {
                //그보다 아래에             아군이 있을시 감싸서 팀병경..
                for (int r = R - 2, l = L; r >= 0; r--)
                {
                    if (__OthelloBoardBlock[r, l].TeamNo == Team)
                    {
                        //바로 왼쪽부터, 아군전까지 팀변경
                        //경계 : 기존 내돌, 새로둘돌.
                        //r,R,l,L 경계제외하고 바꿀것이다
                        ChangeStoneTeam(Team, r, R, l, L);
                        //for문 탈출
                        r = -1;
                        check = true;
                    }
                }
            }
        }
        if (L > 2)
        {
            if (__OthelloBoardBlock[R , L-1].TeamNo == (3 - Team))
            {
                //그보다 아래에             아군이 있을시 감싸서 팀병경..
                for (int r = R, l = L-2; l >= 0; l--)
                {
                    if (__OthelloBoardBlock[r, l].TeamNo == Team)
                    {
                        //바로 왼쪽부터, 아군전까지 팀변경
                        //경계 : 기존 내돌, 새로둘돌.
                        //r,R,l,L 경계제외하고 바꿀것이다
                        ChangeStoneTeam(Team, r, R, l, L);
                        //for문 탈출
                        l = -1;
                        check = true;
                    }
                }
            }
        }
       
        if (MaxR - R > 2 && MaxL - L > 2)
        {
            if (__OthelloBoardBlock[R + 1, L + 1].TeamNo == (3 - Team))
            {
                //그보다 아래에             아군이 있을시 감싸서 팀병경..
                for (int r = R + 2, l = L + 2; r < MaxR && l < MaxL; r++, l++)
                {
                    if (__OthelloBoardBlock[r, l].TeamNo == Team)
                    {
                        //바로 왼쪽부터, 아군전까지 팀변경
                        //경계 : 기존 내돌, 새로둘돌.
                        //r,R,l,L 경계제외하고 바꿀것이다
                        ChangeStoneTeam(Team, r, R, l, L);
                        //for문 탈출
                        r = MaxR; l = MaxL;
                        check = true;
                    }
                }
            }
        }

        if (MaxR - R > 2 && L > 2)
        {
            if (__OthelloBoardBlock[R + 1, L - 1].TeamNo == (3 - Team))
            {
                //그보다 아래에             아군이 있을시 감싸서 팀병경..
                for (int r = R + 2, l = L - 2; r < MaxR && l >= 0; r++, l--)
                {
                    if (__OthelloBoardBlock[r, l].TeamNo == Team)
                    {
                        //바로 왼쪽부터, 아군전까지 팀변경
                        //경계 : 기존 내돌, 새로둘돌.
                        //r,R,l,L 경계제외하고 바꿀것이다
                        ChangeStoneTeam(Team, r, R, l, L);
                        //for문 탈출
                        r = MaxR; l = -1;
                        check = true;
                    }
                }
            }
        }
        if (MaxL - L > 2 && R > 2)
        {
            if (__OthelloBoardBlock[R - 1, L + 1].TeamNo == (3 - Team))
            {
                //그보다 아래에             아군이 있을시 감싸서 팀병경..
                for (int r = R - 2, l = L + 2; l < MaxL && r >= 0; l++, r--)
                {
                    if (__OthelloBoardBlock[r, l].TeamNo == Team)
                    {
                        //바로 왼쪽부터, 아군전까지 팀변경
                        //경계 : 기존 내돌, 새로둘돌.
                        //r,R,l,L 경계제외하고 바꿀것이다
                        ChangeStoneTeam(Team, r, R, l, L);
                        //for문 탈출
                        l = MaxL; r = -1;
                        check = true;
                    }
                }
            }
        }
        if (R > 2 && L > 2)
        {
            if (__OthelloBoardBlock[R - 1, L - 1].TeamNo == (3 - Team))
            {
                //그보다 아래에             아군이 있을시 감싸서 팀병경..
                for (int r = R - 2, l = L - 2; r >= 0 && l >= 0; r--, l--)
                {
                    if (__OthelloBoardBlock[r, l].TeamNo == Team)
                    {
                        //바로 왼쪽부터, 아군전까지 팀변경
                        //경계 : 기존 내돌, 새로둘돌.
                        //r,R,l,L 경계제외하고 바꿀것이다
                        ChangeStoneTeam(Team, r, R, l, L);
                        //for문 탈출
                        r = -1; l = -1;
                        check = true;
                    }
                }
            }
        }
        if (check)
        {
            __OthelloBoardBlock[R, L].CreateNewStone(Team, StoneColor[Team - 1]);
            turn = 3 - turn;

        }
        check = false;


        //F.
        //위에서 확인한 돌의 팀을 바꿈.
        //R1L1서 R2L2까지 하나씩 변하면서 이동
        //R1,R2,L1,L1경계면이다.



    }

    //경계면은 바꾸지 않는다.
    void ChangeStoneTeam(int T, int R1, int R2, int L1, int L2)
    {
        ScoreText.text = R1+""+L1+"bbbb"+R2+""+L2;
        for (int r = R1, l = L1; ;)
        {
            if (R1 > R2) { r -= 1; }
            else if (R1 < R2) { r += 1; }
            if (L1 > L2) { l -= 1; }
            else if (L1 < L2) { l += 1; }
            if (r == R2 && l == L2)
            {
                return;
            }
            __OthelloBoardBlock[r, l].ChangeStoneTeam(T, StoneColor[T - 1]);

        }
    }
}
