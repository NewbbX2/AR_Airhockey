// int GetRealScoreByPuttingNewStoneHere(int r, int l)작성중.
//오류 점검중.
//일단 AI가 다음수 서치하는 알고리즘부터 만드는중.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    //8*8보드
    const int MaxR = 8; const int MaxL = 8;
    //실제 보드에서 돌이 놓인 상태.
    //0 : 빈칸, 1 : 1번플레이어돌, 2 : 2번플레이어돌
    int[,] othelloBoard = new int[MaxR, MaxL];

    //AI가 테스트중인 돌을 놓는 순서
    //-1 : 빈칸. 0 : 기본4개, 1,2,턴당 증가한다. 1개 건너띤 경우, 한턴쉼이다.
    int[,] puttiingOrder = new int[MaxR, MaxL];
    int turn = 0;


    //해당 배열순서대로 놓았을시 어느팀이 이겼는가.
    //아직 두지 않은 수의 배열자리는 0으로 취급되고, 가운데 4개도 0으로 취급된다.
 string[,,,,,,,,
,,,,,,,,
,,,,,,,,
,,,,,,,,
,,,,,,,,
,,,,,,,,
,,,,,,,,
,,,,,,,] wincheck = new
        string[120, 120, 120, 120, 120, 120, 120, 120,
        120, 120, 120, 120, 120, 120, 120, 120,
        120, 120, 120, 120, 120, 120, 120, 120,
        120, 120, 120, 0, 0, 120, 120, 120,
        120, 120, 120, 0, 0, 120, 120, 120,
        120, 120, 120, 120, 120, 120, 120, 120,
        120, 120, 120, 120, 120, 120, 120, 120,
        120, 120, 120, 120, 120, 120, 120, 120];
    //초기 돌 & 보드세팅.
    void Start()
    {
        othelloBoard= makeBoardToDefaultSetting(othelloBoard);
        puttiingOrder=makePuttingOrderToDefaultSetting(puttiingOrder);
    }


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



    int[,] makePuttingOrderToDefaultSetting(int[,] order)
    {

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                order[i, j] = 0;
            }
        }
        return order;
    }




    //턴시작전, 게임종료상황인가 체크.(양쪽다 할수있는게 없다면 종료상황이다 무승부패턴으로 기록.)
    //한쪽만 할수있는게 없다면 휴식후 다음플레이어턴 진행
    void startNextTurn()
    {
        turn++;
        if(check_CanDoAnyThingInThisTrun())
        {
            playTurn(turn);
        }
        else
        {
            turn++;
            {
                if (check_CanDoAnyThingInThisTrun())
                {
                    playTurn(turn);
                }
                else
                {
                    recordPattern(0);
                }
            }
        }
    }



    //한곳이라도 둘수있는곳이 있는지 체크.
    bool check_CanDoAnyThingInThisTrun()
    {
        for(int r=0; r<8; r++){for(int l=0;l<8;l++){
            if(othelloBoard[r,l]==0) {
                if(checkCanPutHere(r, l,othelloBoard)){
                    return true;
                }
             }
        }}
        return false;
    }



    //해당 자리에 둘수있는지를 체크
    bool checkCanPutHere(int r, int l, int[,] board)
    {
        board = new int[MaxR, MaxL];
        int playingTeamNo = 2 - (turn % 2);
        //이웃한 돌이 반대팀이고, 해당방향 끝까지중에 같은편이 있으면 감쌀수있다. 감쌀수있으면, 획득가능.
        if (r >= 2)  { if (board[r - 1, l] == 2 - playingTeamNo){
                if (IsStoneWhichTeam_teamNo_Here(r-2, l, -1, 0, playingTeamNo, board)) { return true; } } }
        if(r<=5) {if (board[r + 1, l] == 2 - playingTeamNo){
                if (IsStoneWhichTeam_teamNo_Here(r+2, l, 1, 0, playingTeamNo, board)) { return true; } } }

        if (l >= 2)  { if (board[r, l - 1] == 2 - playingTeamNo) {
                if (IsStoneWhichTeam_teamNo_Here(r, l-2, 0, -1, playingTeamNo, board)) { return true; } } }
        if(l<=5) {if (board[r, l + 1] == 2 - playingTeamNo) {
                if (IsStoneWhichTeam_teamNo_Here(r, l+2, 0, 1, playingTeamNo, board)) { return true; } } }
        
        if (r >= 2&& l >= 2)  { if (board[r - 1, l-1] == 2 - playingTeamNo){
                if (IsStoneWhichTeam_teamNo_Here(r-2, l-2, -1, -1, playingTeamNo, board)) { return true; } } }
        
        if (r >= 2&& l <= 5)  { if (board[r - 1, l+1] == 2 - playingTeamNo){
                if (IsStoneWhichTeam_teamNo_Here(r-2, l+2, -1, +1, playingTeamNo, board)) { return true; } } }
        
        if (r <= 5 && l >= 2)  { if (board[r + 1, l-1] == 2 - playingTeamNo){
                if (IsStoneWhichTeam_teamNo_Here(r+2, l-2, +1, -1, playingTeamNo, board)) { return true; } } }
        
        if (r <= 5 && l <= 5)  { if (board[r + 1, l+1] == 2 - playingTeamNo){
                if (IsStoneWhichTeam_teamNo_Here(r+2, l+2, +1, +1, playingTeamNo, board)) { return true; } } }
        return false;
    }

    //r,l부터 rincreases,lincreases만큼 증가시키면서, teamNo의 돌이 있는지를 체크. 있으면 바로 true리턴 끝까지 탐색시 false;
    bool IsStoneWhichTeam_teamNo_Here(int r, int l, int rIncreases, int lIncreases, int teamNo ,int[,] board)
    {
        for(;r<MaxR&&r>=0&&l<MaxL&&l>=0;r+=rIncreases,l+=lIncreases)
        {
            if (board[r, l] == teamNo) { return true; }
        }
        return false;
    }


    //이번턴 할게 분명히 있다.
    void playTurn(int T)
    {
        //AI는 이번 턴에서 가장 이득을 보는 수를 탐색할 것이다.
        FindBestPlayInThisTurn();
    }


    void FindBestPlayInThisTurn()
    {
        //각 수를 두면 (해당플레이어 입장서) 몇점이되는지를 확인.
        //엔딩패턴인 경우,
        //-100으로 자기 승리, -101로 적승리 -102로 무승부.
        //이 둘 빼고 탐색해서 가장 높은 수를 선택.
        int bestScore = 0;int R = 0; int L = 0;
        for(int r=0; r < MaxR; r++) {for(int l=0;l<MaxL;l++){
                int s = GetScoreByPuttingNewStoneHere(r,l);
                if (s > bestScore) { R = r;L = l;bestScore = s; }

        }}
        //점수가 증가하지 않았다는것은, 지금부터 가능한 모든 패턴을 검색했다는것이다.
        //따라서 이번 턴의 플레이어가 승리패턴 진입할수 있다면, 여기는 승리패턴이다.
        //이번턴의 플레이어가 승리할수 없는데 무승부 가능하다면, 여기는 무승부 패턴이다.
        //무승부의 패턴조차 없다면, 이것은 패배패턴이다.
        if(bestScore==0)
        {
            int BS = 0;
            //다시 지금부터 가능한 수를 검색해서
             for(int r=0; r < MaxR; r++) {for(int l=0;l<MaxL;l++){
                int s = GetScoreByPuttingNewStoneHere(r,l);
                    //.승리패턴 있으면 무조건 승리패턴
                    if (s == -100) { BS = s; }
                    // 승리패턴 없었는데 무승부 패턴 있다면, 일단체크.
                    if( (BS!=-100) && (s==-102)) { BS = s; }
                    //승리패턴&무승부패턴 없었는데 패배패턴밖에 없다면, 기록.
                    if( (BS!=-100) && (BS!=-102) && (s==-101)) { BS = 2; } 
            }}
             //가능한수 없다면(가능한수 없음 패턴이면 처음에 체크되었을텐데?) (일단 오류일수있으니) 무승부로 체크.
            if (BS == 0) { BS = -102; }


            //승리체크되있다면 승리로 기록
            if (BS == -100) { recordPattern(1); }
            //패배체크 되있다면 패배로 기록.
            else if (BS == -101) { recordPattern(-1); }
            //무승부체크 되있다면 무승부로 기록.
            else if(BS == -102) { recordPattern(0); }
        }
    }





    int GetScoreByPuttingNewStoneHere(int r, int l)
    {




        int teamNo = 2 - (turn % 2);
        //이미 누가 이길지 아는 수라면.
        if (wincheck[
            puttiingOrder[0, 0], puttiingOrder[0, 1], puttiingOrder[0, 2], puttiingOrder[0, 3], puttiingOrder[0, 4], puttiingOrder[0, 5], puttiingOrder[0, 6], puttiingOrder[0, 7],
            puttiingOrder[1, 1], puttiingOrder[1, 1], puttiingOrder[1, 2], puttiingOrder[1, 3], puttiingOrder[1, 4], puttiingOrder[1, 5], puttiingOrder[1, 6], puttiingOrder[1, 7],
            puttiingOrder[2, 2], puttiingOrder[2, 1], puttiingOrder[2, 2], puttiingOrder[2, 3], puttiingOrder[2, 4], puttiingOrder[2, 5], puttiingOrder[2, 6], puttiingOrder[2, 7],
            puttiingOrder[3, 3], puttiingOrder[3, 1], puttiingOrder[3, 2], 0, 0, puttiingOrder[3, 5], puttiingOrder[3, 6], puttiingOrder[3, 7],
            puttiingOrder[4, 4], puttiingOrder[4, 1], puttiingOrder[4, 2], 0, 0, puttiingOrder[4, 5], puttiingOrder[4, 6], puttiingOrder[4, 7],
            puttiingOrder[5, 5], puttiingOrder[5, 1], puttiingOrder[5, 2], puttiingOrder[5, 3], puttiingOrder[5, 4], puttiingOrder[5, 5], puttiingOrder[5, 6], puttiingOrder[5, 7],
            puttiingOrder[6, 6], puttiingOrder[6, 1], puttiingOrder[6, 2], puttiingOrder[6, 3], puttiingOrder[6, 4], puttiingOrder[6, 5], puttiingOrder[6, 6], puttiingOrder[6, 7],
            puttiingOrder[7, 7], puttiingOrder[7, 1], puttiingOrder[7, 2], puttiingOrder[7, 3], puttiingOrder[7, 4], puttiingOrder[7, 5], puttiingOrder[7, 6], puttiingOrder[7, 7]] != null) {
            int win = int.Parse(wincheck[
            puttiingOrder[0, 0], puttiingOrder[0, 1], puttiingOrder[0, 2], puttiingOrder[0, 3], puttiingOrder[0, 4], puttiingOrder[0, 5], puttiingOrder[0, 6], puttiingOrder[0, 7],
            puttiingOrder[1, 1], puttiingOrder[1, 1], puttiingOrder[1, 2], puttiingOrder[1, 3], puttiingOrder[1, 4], puttiingOrder[1, 5], puttiingOrder[1, 6], puttiingOrder[1, 7],
            puttiingOrder[2, 2], puttiingOrder[2, 1], puttiingOrder[2, 2], puttiingOrder[2, 3], puttiingOrder[2, 4], puttiingOrder[2, 5], puttiingOrder[2, 6], puttiingOrder[2, 7],
            puttiingOrder[3, 3], puttiingOrder[3, 1], puttiingOrder[3, 2], 0, 0, puttiingOrder[3, 5], puttiingOrder[3, 6], puttiingOrder[3, 7],
            puttiingOrder[4, 4], puttiingOrder[4, 1], puttiingOrder[4, 2], 0, 0, puttiingOrder[4, 5], puttiingOrder[4, 6], puttiingOrder[4, 7],
            puttiingOrder[5, 5], puttiingOrder[5, 1], puttiingOrder[5, 2], puttiingOrder[5, 3], puttiingOrder[5, 4], puttiingOrder[5, 5], puttiingOrder[5, 6], puttiingOrder[5, 7],
            puttiingOrder[6, 6], puttiingOrder[6, 1], puttiingOrder[6, 2], puttiingOrder[6, 3], puttiingOrder[6, 4], puttiingOrder[6, 5], puttiingOrder[6, 6], puttiingOrder[6, 7],
            puttiingOrder[7, 7], puttiingOrder[7, 1], puttiingOrder[7, 2], puttiingOrder[7, 3], puttiingOrder[7, 4], puttiingOrder[7, 5], puttiingOrder[7, 6], puttiingOrder[7, 7]
            ]);
            {
                if (win == 1) { return -100; }
                else if (win == -1) { return -101; }
                else if (win == 0) { return -102; }
            }
        }//처음보는 구간이라면
        else
        {
            //실제로 구한다.
            return GetRealScoreByPuttingNewStoneHere(r,l);
        }
        //승리패턴 기록에서 구했던건데 오류난경우, 
        //다시체크해야지 뭘어쩜.
        return GetRealScoreByPuttingNewStoneHere(r, l);

    }


   
    void recordPattern(int winDrawLose)
    {
        wincheck[
            puttiingOrder[0, 0], puttiingOrder[0, 1], puttiingOrder[0, 2], puttiingOrder[0, 3], puttiingOrder[0, 4], puttiingOrder[0, 5], puttiingOrder[0, 6], puttiingOrder[0, 7],
            puttiingOrder[1, 1], puttiingOrder[1, 1], puttiingOrder[1, 2], puttiingOrder[1, 3], puttiingOrder[1, 4], puttiingOrder[1, 5], puttiingOrder[1, 6], puttiingOrder[1, 7],
            puttiingOrder[2, 2], puttiingOrder[2, 1], puttiingOrder[2, 2], puttiingOrder[2, 3], puttiingOrder[2, 4], puttiingOrder[2, 5], puttiingOrder[2, 6], puttiingOrder[2, 7],
            puttiingOrder[3, 3], puttiingOrder[3, 1], puttiingOrder[3, 2], 0, 0, puttiingOrder[3, 5], puttiingOrder[3, 6], puttiingOrder[3, 7],
            puttiingOrder[4, 4], puttiingOrder[4, 1], puttiingOrder[4, 2], 0, 0, puttiingOrder[4, 5], puttiingOrder[4, 6], puttiingOrder[4, 7],
            puttiingOrder[5, 5], puttiingOrder[5, 1], puttiingOrder[5, 2], puttiingOrder[5, 3], puttiingOrder[5, 4], puttiingOrder[5, 5], puttiingOrder[5, 6], puttiingOrder[5, 7],
            puttiingOrder[6, 6], puttiingOrder[6, 1], puttiingOrder[6, 2], puttiingOrder[6, 3], puttiingOrder[6, 4], puttiingOrder[6, 5], puttiingOrder[6, 6], puttiingOrder[6, 7],
            puttiingOrder[7, 7], puttiingOrder[7, 1], puttiingOrder[7, 2], puttiingOrder[7, 3], puttiingOrder[7, 4], puttiingOrder[7, 5], puttiingOrder[7, 6], puttiingOrder[7, 7]]
            = winDrawLose.ToString();
    }

    //이미 탐색한수는 검색할필요없다.

    int GetRealScoreByPuttingNewStoneHere(int r, int l)
    {



        return 0;
    }

}
