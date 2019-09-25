//OthelloAI라고 AI따로만드는중
//AI는 AI둘이서 해당턴에 가능한 모든 수 확인하고, 그중에서 (이번턴에) 가장 점수를 높게 만드는 수를 선택함. 이걸 게임종료까지 반복해서 그 수를 제외하고 다른 수를 선택하는식으로 
//탐색을 계속함, 탐색학 수는 기록해두었다가,
//실제 게임에서 AI랑 붙게 하면, 기존 탐색했던 지는 수는 두지 않음.
//이를 위해서 탐색했던 경우중에 필패구간으로는 돌입하지 않음.
//필승구간으로 들어갈 수 있으면, 필승구간으로 들어감.
//플레이어와의 게임중에 처음보는 수가 등장하면, 탐색알고리즘대로, 각 턴에 이득만을 고려해서 수를 두게 함.
//으로 만들려고 하고.
//일단 게임 알고리즘만드는중인것.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//check Team can put에서 RL부분
public class Othello : MonoBehaviour
{
    int[,] othelloBoard = new int[8, 8];
    int[] score = new int[2];

    int turn = 0;
    int[,] canPutStoneOnOthelloBoard = new int[8, 8];


    void Start()
    {
      for(int i=0; i<8; i++)
        {
            for (int j = 0; j < 8; j++)
                othelloBoard[i, j] = 0;
        }
        othelloBoard[3, 3] = othelloBoard[4,4]=1;
        othelloBoard[3, 4] = othelloBoard[4, 3] = 0;
    }


    //턴 시작전에 세팅 및 할수있는거 체크.
    //없으면 다음플레이어에 넘길것임.
    void TurnStart()
    {
        turn++;
        //턴시작시 놓을수 있는 자리 체크한다.
        for (int r = 0; r < 8; r++)
        {
            for (int l = 0; l < 8; l++)
            {
                canPutStoneOnOthelloBoard[r, l] = CheckPutNewStone(r, l, 2 - (turn % 2));
            }
        }
        //하나라도 할수있나 확인..
        bool check1 = false;
        for (int r = 0; r < 8; r++)
        {
            for (int l = 0; l < 8; l++)
            {
                if (canPutStoneOnOthelloBoard[r, l] == 1)
                {
                    check1 = true;
                }
            }
        }
        //할수있는게 없다면, 턴을 진행하지 않고 다음 플레이어에게 넘긴다.
        if (!check1)
        {
            //다음플레이어의 턴.
            turn++;
            //놓을수 있는 자리 체크한다.
            for (int r = 0; r < 8; r++)
            {
                for (int l = 0; l < 8; l++)
                {
                    canPutStoneOnOthelloBoard[r, l] = CheckPutNewStone(r, l, 2 - (turn % 2));
                }
            }
            //하나라도 할수있나 확인.
            bool check2 = false;
            for (int r = 0; r < 8; r++)
            {
                for (int l = 0; l < 8; l++)
                {
                    if (canPutStoneOnOthelloBoard[r, l] == 1)
                    {
                        check2 = true;
                    }
                }
            }
            //다음 플레이어도 할수있는게 없다면, 게임을 종료한다.
            if (!check2)
            {
                endGame();
            }
        }
    }


    //턴종료.
    //스코어 표기.
    void TurnEnd()
    {
        DisPlayScoreOnScoreBoard();
        TurnStart();
    }

    //게임엔딩.
    void endGame()
    {
        //1팀승리
        if (GetScore(1) > GetScore(2))
        {
        }
        //2팀승리
        else if (GetScore(1) < GetScore(2))
        {
        }
        //무승부.
        else
        {
        }
    }


    //보드의 해당 위치에 둘수있는지 체크.
    int CheckPutNewStone(int Team, int R, int L )
    {
        //빈칸에만 둘수있음
        if(othelloBoard[R,L]!=0)
        {
            return -1;
        }
        //오른쪽 빈칸에 둬서 왼쪽을 감쌈
        if (L >= 2)
        {
            //바로왼쪽이 적팀.
            if (othelloBoard[R, L - 1] == (3 - Team))
            {
                //그보다 왼쪽에             아군이 있을시 허용함.
                for (int r = R, l = L - 2; l >= 0; l--)
                {
                    if (othelloBoard[r, l] == (3 - Team))
                    {
                        return 1;
                    }
                }
            }
        }
        //아래쪽 빈칸에 둬서 왼쪽을 감쌈
        if (R >= 2)
        {
            //바로 위쪽이 적팀.
            if (othelloBoard[R-1, L ] == (3 - Team))
            {
                //그보다 왼쪽에             아군이 있을시 허용함.
                for (int r = R-2, l = L ; r >= 0; r--)
                {
                    if (othelloBoard[r, l] == (3 - Team))
                    {
                        return 1;
                    }
                }
            }
        }

        //왼쪽 빈칸에 둬서 오른쪽을 감쌈
        if (L <= 5)
        {
            //바로오른쪽이 적팀.
            if (othelloBoard[R, L + 1] == (3 - Team))
            {
                //그보다 오른쪽에             아군이 있을시 허용함.
                for (int r = R, l = L + 2; l <= 7; l++)
                {
                    if (othelloBoard[r, l] == (3 - Team))
                    {
                        return 1;
                    }
                }
            }
        }
        //위쪽 빈칸에 둬서 아래을 감쌈
        if (R <= 5)
        {
            //바로아래이 적팀.
            if (othelloBoard[R+1, L] == (3 - Team))
            {
                //그보다 아래에             아군이 있을시 허용함.
                for (int r = R+2, l = L; r <= 7; r++)
                {
                    if (othelloBoard[r, l] == (3 - Team))
                    {
                        return 1;
                    }
                }
            }
        }
        //오른쪽 아래 빈칸에 둬서 왼쪽 위를 감쌈
        if ( R>=2&& L >= 2)
        {
            //바로왼쪽이 적팀.
            if (othelloBoard[R-1, L - 1] == (3 - Team))
            {
                //그보다 왼쪽에 위            아군이 있을시 허용함.
                for (int r = R - 2, l = L - 2; r >= 0 && l >= 0; l--, r--) 
                {
                    if (othelloBoard[r, l] == (3 - Team))
                    {
                        return 1;
                    }
                }
            }
        }
        //좌상 빈칸에 둬서 우하를 감쌈
        if ( R <= 5&& L <= 5)
        {
            //바로우하가 적팀.
            if (othelloBoard[R + 1, L + 1] == (3 - Team))
            {
                //그보다 우하에             아군이 있을시 허용함.
                for (int r = R + 2, l = L + 2; r <= 7 && l <= 7;r++, l++) 
                {
                    if (othelloBoard[r, l] == (3 - Team))
                    {
                        return 1;
                    }
                }
            }
        }
        if (R >= 2 && L <= 5) 
        {
            if (othelloBoard[R - 1, L + 1] == (3 - Team))
            {
                for (int r = R - 2, l = L + 2;  r >=0 && l <= 7 ; r--,l++ )
                {
                    if (othelloBoard[r, l] == (3 - Team))
                    {
                        return 1;
                    }
                }
            }
        }
        if (R <= 5 && L >= 2) 
        {
            if (othelloBoard[R + 1, L - 1] == (3 - Team))
            {
                for (int  r = R + 2, l = L - 2;  r <= 7&& l >= 0 ;  r++, l--) 
                {
                    if (othelloBoard[r, l] == (3 - Team))
                    {
                        return 1;
                    }
                }
            }
        }
        return 0;
    }


    //터치한 장소에 두려고 시도.
    void TryPutNewStone(int Team, int R, int L)
    {
        int check = CheckPutNewStone(Team, R, L);
        //돌이 있는곳에 둘수는 없음
        if(check==0)
        { 
        }
        //둘수있는곳
        else if(check==1)
        {
            PutStone(Team,R,L);
        }
        //감싸지 않아서 못둠.
        else
        {
        }

    }
    //해당위치에 돌을 둔다.
    void PutStone(int Team, int R, int L)
    {
        TryChangeStoneTeam(Team, R, L);
        CreateNewStone(Team, R, L);
    }

    //돌을 놓을때 감싼돌확인.
    void TryChangeStoneTeam(int Team, int R, int L)
    { 
        //오른쪽 빈칸에 둬서 왼쪽을 감쌈
        if (L >= 2)
        {
            //바로왼쪽이 적팀.
            if (othelloBoard[R, L - 1] == (3 - Team))
            {
                //그보다 왼쪽에             아군이 있을시 
                for (int r = R, l = L - 2; l >= 0; l--)
                {
                    if (othelloBoard[r, l] == (3 - Team))
                    {
                        //바로 왼쪽부터, 아군전까지 팀변경
                        //경계 : 기존 내돌, 새로둘돌.
                        //r,R,l,L 경계제외하고 바꿀것이다
                        ChangeStoneTeam(Team, r, R, l, L);
                        //for문 탈출
                        l = -1;
                    }
                }
            }
        }

//동일알고리즘
        //아래쪽 빈칸에 둬서 왼쪽을 감쌈
        if (R >= 2)
        {
            //바로 위쪽이 적팀.
            if (othelloBoard[R - 1, L] == (3 - Team))
            {
                //그보다 왼쪽에             아군이 있을시 감싸서 팀병경.
                for (int r = R - 2, l = L; r >= 0; r--)
                {
                    if (othelloBoard[r, l] == (3 - Team))
                    {
                        //바로 왼쪽부터, 아군전까지 팀변경
                        //경계 : 기존 내돌, 새로둘돌.
                        //r,R,l,L 경계제외하고 바꿀것이다
                        ChangeStoneTeam(Team, r, R, l, L);
                        //for문 탈출
                        r = -1;
                    }
                }
            }
        }

        //왼쪽 빈칸에 둬서 오른쪽을 감쌈
        if (L <= 5)
        {
            //바로오른쪽이 적팀.
            if (othelloBoard[R, L + 1] == (3 - Team))
            {
                //그보다 오른쪽에             아군이 있을시 감싸서 팀병경..
                for (int r = R, l = L + 2; l <= 7; l++)
                {
                    if (othelloBoard[r, l] == (3 - Team))
                    {
                        //바로 왼쪽부터, 아군전까지 팀변경
                        //경계 : 기존 내돌, 새로둘돌.
                        //r,R,l,L 경계제외하고 바꿀것이다
                        ChangeStoneTeam(Team, r, R, l, L);
                        //for문 탈출
                        l = 8;
                    }
                }
            }
        }
        //위쪽 빈칸에 둬서 아래을 감쌈
        if (R <= 5)
        {
            //바로아래이 적팀.
            if (othelloBoard[R + 1, L] == (3 - Team))
            {
                //그보다 아래에             아군이 있을시 감싸서 팀병경..
                for (int r = R + 2, l = L; r <= 7; r++)
                {
                    if (othelloBoard[r, l] == (3 - Team))
                    {
                        //바로 왼쪽부터, 아군전까지 팀변경
                        //경계 : 기존 내돌, 새로둘돌.
                        //r,R,l,L 경계제외하고 바꿀것이다
                        ChangeStoneTeam(Team, r, R, l, L);
                        //for문 탈출
                        r = 8;
                    }
                }
            }
        }
        //오른쪽 아래 빈칸에 둬서 왼쪽 위를 감쌈
        if (R >= 2 && L >= 2)
        {
            //바로왼쪽이 적팀.
            if (othelloBoard[R - 1, L - 1] == (3 - Team))
            {
                //그보다 왼쪽에 위            아군이 있을시 감싸서 팀병경..
                for (int r = R - 2, l = L - 2; r >= 0 && l >= 0; l--, r--)
                {
                    if (othelloBoard[r, l] == (3 - Team))
                    {
                        //바로 왼쪽부터, 아군전까지 팀변경
                        //경계 : 기존 내돌, 새로둘돌.
                        //r,R,l,L 경계제외하고 바꿀것이다
                        ChangeStoneTeam(Team, r, R, l, L);
                        //for문 탈출
                        r = -1;
                        l = -1;
                    }
                }
            }
        }
        //좌상 빈칸에 둬서 우하를 감쌈
        if (R <= 5 && L <= 5)
        {
            //바로우하가 적팀.
            if (othelloBoard[R + 1, L + 1] == (3 - Team))
            {
                //그보다 우하에             아군이 있을시 감싸서 팀병경..
                for (int r = R + 2, l = L + 2; r <= 7 && l <= 7; r++, l++)
                {
                    if (othelloBoard[r, l] == (3 - Team))
                    {
                        //바로 왼쪽부터, 아군전까지 팀변경
                        //경계 : 기존 내돌, 새로둘돌.
                        //r,R,l,L 경계제외하고 바꿀것이다
                        ChangeStoneTeam(Team, r, R, l, L);
                        //for문 탈출
                        r = 8;
                        l = 8;
                    }
                }
            }
        }
        if (R >= 2 && L <= 5)
        {
            if (othelloBoard[R - 1, L + 1] == (3 - Team))
            {
                for (int r = R - 2, l = L + 2; r >= 0 && l <= 7; r--, l++)
                {
                    if (othelloBoard[r, l] == (3 - Team))
                    {
                        //바로 왼쪽부터, 아군전까지 팀변경
                        //경계 : 기존 내돌, 새로둘돌.
                        //r,R,l,L 경계제외하고 바꿀것이다
                        ChangeStoneTeam(Team, r, R, l, L);
                        //for문 탈출
                        r = -1;
                        l = 8;
                    }
                }
            }
        }
        if (R <= 5 && L >= 2)
        {
            if (othelloBoard[R + 1, L - 1] == (3 - Team))
            {
                for (int r = R + 2, l = L - 2; r <= 7 && l >= 0; r++, l--)
                {
                    if (othelloBoard[r, l] == (3 - Team))
                    {
                        //바로 왼쪽부터, 아군전까지 팀변경
                        //경계 : 기존 내돌, 새로둘돌.
                        //r,R,l,L 경계제외하고 바꿀것이다
                        ChangeStoneTeam(Team, r, R, l, L);
                        //for문 탈출
                        r = 8;
                        l = -1; 
                    }
                }
            }
        }
//여기부터 확인.
    }


//F.
    //위에서 확인한 돌의 팀을 바꿈.
    //R1L1서 R2L2까지 하나씩 변하면서 이동
    //R1,R2,L1,L1경계면이다.
    //경계면은 바꾸지 않는다.
    void ChangeStoneTeam(int Team, int R1, int R2, int L1,int L2)
    {
        for(int r=R1, l=L2 ;; )
        {
            if (R1 != R2){ r += ((R2 - R1) / (R2 - R1)); }
            if(L1!=L2){ l += ((L2 - L1) / (L2 - L1));}
            if (r == R2 && l == L2)
            {
                return;
            }
            othelloBoard[r, l] = Team;
        }
    }


    //해당위치에 돌을 새로이 놓음.
    void CreateNewStone(int Team, int R, int L)
    {
        //우선 데이터시트 변경.
        othelloBoard[R, L] = Team;
    }

//F, 팀의 점수가져오기
    int GetScore(int team)
    {
        int score = 0;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
                if (othelloBoard[i, j] == team)
                    score++;
        }
        return score;
    }
    void DisPlayScoreOnScoreBoard()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
}
