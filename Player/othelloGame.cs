using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class OthelloGame : MonoBehaviour
{

    const int MaxR = 8; const int MaxL = 8;
    public int[,] othelloBoard = new int[MaxR, MaxL];
    int[] score = new int[2];
    int turn = 0;
    bool playtingTurn = false;
    public Text ScoreText;
    public Text Wincheck;

    public void SetScoreText()
    {
        ScoreText.text = "Team1:" + score[0] + "Team2:" + score[1];
    }


    void Start()
    {
        gameSettingBeforStartNewGame();
        startNextTurn(false);
    }



    //
    void gameSettingBeforStartNewGame()
    {
        othelloBoard = makeBoardToDefaultSetting(othelloBoard);







        int[,] makeBoardToDefaultSetting(int[,] gameBoard)
        {
            for (int i = 0; i < MaxR; i++)
            {
                for (int j = 0; j < MaxL; j++)
                {
                    gameBoard[i, j] = 0;
                }
            }
            gameBoard[3, 3] = gameBoard[4, 4] = 1;
            gameBoard[3, 4] = gameBoard[4, 3] = 0;
            return gameBoard;
        }
    }


    //보드 초기세팅
    


    //이번턴 할수있는게 없을때
            //이전턴 스킵한경우 엔딩
            //스킵 안한경우 다음턴 진행.
    //그외경우 이번턴 진행.
    void startNextTurn(bool skippedPreTurn)
    {
        turn++;
        if (check_CanDoAnyThingInThisTrun())
        {
            playtingTurn = true;
        }
        else
        {
            if(skippedPreTurn)
            {
                endGame();
            }
            else
            {
                startNextTurn(true);
            }
        }
    }



    //한곳이라도 둘수있는곳이 있으면 t, 모두아니면 f
    bool check_CanDoAnyThingInThisTrun()
    {
        for (int r = 0; r < 8; r++)
        {
            for (int l = 0; l < 8; l++)
            {
                if (checkCanPutNewStoneHere(r, l, othelloBoard))
                {
                    return true;
                }
            }
        }
        return false;
    }




    //이 장소에 둘수있는지 체크.
    bool checkCanPutNewStoneHere(int r, int l, int[,] board)
    {
        if (othelloBoard[r, l] == 0)
        {
            int playingTeamNo = 2 - (turn % 2);
            if (r >= 2)
            {
                if (board[r - 1, l] == 2 - playingTeamNo) {//왼쪽에 반대색
                    if (IsStoneWhoseTeamIs_teamNo_Here(r - 2, l, -1, 0, playingTeamNo, board)) { return true; }}//그걸 감싸는돌있음
            }
            if (r <= 5)
            {
                if (board[r + 1, l] == 2 - playingTeamNo) {
                    if (IsStoneWhoseTeamIs_teamNo_Here(r + 2, l, 1, 0, playingTeamNo, board)) { return true; }}
            }

            if (l >= 2)
            {
                if (board[r, l - 1] == 2 - playingTeamNo){
                    if (IsStoneWhoseTeamIs_teamNo_Here(r, l - 2, 0, -1, playingTeamNo, board)) { return true; } }
            }
            if (l <= 5)
            {
                if (board[r, l + 1] == 2 - playingTeamNo){
                    if (IsStoneWhoseTeamIs_teamNo_Here(r, l + 2, 0, 1, playingTeamNo, board)) { return true; }}
            }

            if (r >= 2 && l >= 2)
            {
                if (board[r - 1, l - 1] == 2 - playingTeamNo){
                    if (IsStoneWhoseTeamIs_teamNo_Here(r - 2, l - 2, -1, -1, playingTeamNo, board)) { return true; }}
            }

            if (r >= 2 && l <= 5)
            {
                if (board[r - 1, l + 1] == 2 - playingTeamNo){
                    if (IsStoneWhoseTeamIs_teamNo_Here(r - 2, l + 2, -1, +1, playingTeamNo, board)) { return true; } }
            }

            if (r <= 5 && l >= 2)
            {
                if (board[r + 1, l - 1] == 2 - playingTeamNo) {
                    if (IsStoneWhoseTeamIs_teamNo_Here(r + 2, l - 2, +1, -1, playingTeamNo, board)) { return true; } }
            }

            if (r <= 5 && l <= 5)
            {
                if (board[r + 1, l + 1] == 2 - playingTeamNo){
                    if (IsStoneWhoseTeamIs_teamNo_Here(r + 2, l + 2, +1, +1, playingTeamNo, board)) { return true; } }
            }
        }
        return false;








        bool IsStoneWhoseTeamIs_teamNo_Here(int rr, int ll, int rIncreases, int lIncreases, int teamNo, int[,] B)
        {
            for (; rr < MaxR && rr >= 0 && ll < MaxL && ll >= 0; rr += rIncreases, ll += lIncreases)
            {
                if (B[rr, ll] == teamNo) { return true; }
            }
            return false;
        }

    }











    //게임엔딩.
    void endGame()
    {
        //1팀승리
        if (GetScore(1) > GetScore(2))
        {
            Wincheck.text = "team1 Win";
        }
        //2팀승리
        else if (GetScore(1) < GetScore(2))
        {
            Wincheck.text = "team2 Win";
        }
        //무승부.
        else
        {
            Wincheck.text = "Draw Win";
        }
    }

















    public int inputNo = -1;


    void update()
    {

        //턴 진행중일때만 체크함.
        if (playtingTurn)
        {
            if ((inputNo >= 0) && (inputNo < 64))
                tryToPutNweStone(inputNo / 8, inputNo % 8);
        }else{}

    }



    void tryToPutNweStone(int r, int l)
    {
        //그자리 둘수있으면 둔다.
        if (checkCanPutNewStoneHere(r, l, othelloBoard))
        {
            PutStone(2 - (turn % 2), r, l);
        }
        else
        {
            inputNo = -1;
        }
    }

    //턴종료.
    //스코어 표기.
    void endThisTurn()
    {
        score[0] = GetScore(1);
        score[1] = GetScore(2);
        startNextTurn(false);
    }

    //보드의 해당 위치에 둘수있는지 체크.
    int CheckPutNewStone(int Team, int R, int L)
    {
        //빈칸에만 둘수있음
        if (othelloBoard[R, L] != 0)
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
            if (othelloBoard[R - 1, L] == (3 - Team))
            {
                //그보다 왼쪽에             아군이 있을시 허용함.
                for (int r = R - 2, l = L; r >= 0; r--)
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
            if (othelloBoard[R + 1, L] == (3 - Team))
            {
                //그보다 아래에             아군이 있을시 허용함.
                for (int r = R + 2, l = L; r <= 7; r++)
                {
                    if (othelloBoard[r, l] == (3 - Team))
                    {
                        return 1;
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
        if (R <= 5 && L <= 5)
        {
            //바로우하가 적팀.
            if (othelloBoard[R + 1, L + 1] == (3 - Team))
            {
                //그보다 우하에             아군이 있을시 허용함.
                for (int r = R + 2, l = L + 2; r <= 7 && l <= 7; r++, l++)
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
                for (int r = R - 2, l = L + 2; r >= 0 && l <= 7; r--, l++)
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
                for (int r = R + 2, l = L - 2; r <= 7 && l >= 0; r++, l--)
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
        if (check == 0)
        {
        }
        //둘수있는곳
        else if (check == 1)
        {
            PutStone(Team, R, L);
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





        //F.
        //위에서 확인한 돌의 팀을 바꿈.
        //R1L1서 R2L2까지 하나씩 변하면서 이동
        //R1,R2,L1,L1경계면이다.
        //경계면은 바꾸지 않는다.
        void ChangeStoneTeam(int T, int R1, int R2, int L1, int L2)
        {
            for (int r = R1, l = L2; ;)
            {
                if (R1 > R2) { r -= 1; }
                else if (R1 < R2) { r += 1; }
                if (L1 > L2) { l -= 1; }
                else if (L1 < L2) { l += 1; }
                if (r == R2 && l == L2)
                {
                    return;
                }
                othelloBoard[r, l] = T;
            }
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
}
