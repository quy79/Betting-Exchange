using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core;
using BetEx247.Data.Model;
using BetEx247.Data.DAL.Sports;
namespace BetEx247.Plugin.DataManager.Settle
{
    public class BetSettlement
    {
        private SoccerMatch SoccerMatchObj;
        private MyBet BackBetObj;
        private MyBet LayBetObj;
        private double PointsRefunded;
        private Member _memberBack, _memberLay;
        private DateTime SettledTime;

        public string[] MatchStatus = { "NotStarted", "InPlay-1stHalf", "InPlay-2ndHalf", "InPlay-ET1stHalf", "InPlay-ET2ndHalf", "InPlay-PK", "HT", "FT", "Postponed", "Cancelled" };
        //A List to store Bet Status
        public string[] BetStatus = { "UnMatched", "Matched", "UnSettled", "Settled", "Cancelled", "Lapsed", "Void" };

        // 2 Enums used to store Periods for Soccer and other sports
        public enum Soccer_Periods { FT, T1stHalf, T2ndHalf, ET1, ET2, PK };

        public enum Sport_Periods { Set1, Set2, Set3, Set4, Set5, Set6, Set7, Set8, Set9, ExSet, PK };

        public int[] DiscountPercent = {0,2,8,18,30,50,70,100,125,160,200,240,280,330,380,440,500,570,640,710,780,870,950,1030,1130,1230,1320,1430,1550,1650,1750,1900,2000,2150,
                                  2300,2400,2500,2650,2800,3000,3100,3300,3400,3600,3800,4000,4150,4300,4500,
                                  4700,4900,5100,5300,5500,5700,5900,6400,7000,7800,8500,9200};

        // Enum to store Odds Types
        public enum OddsTypes
        {
            SoccerMatchOdds, SoccerTotalGoalsOU, SoccerDrawNoBet,
            SoccerAsianHandicap, SoccerCorrectScores
        } ;

        MemberService _memberSvr = new MemberService();

        //constructor			
        public BetSettlement(SoccerMatch _SoccerMatchObj, MyBet _BackBetObj, MyBet _LayBetObj)
        {
            SoccerMatchObj = _SoccerMatchObj;
            BackBetObj = _BackBetObj;
            LayBetObj = _LayBetObj;
            _memberBack = new Member();
            _memberLay = new Member();
            _memberBack = _memberSvr.Member(_BackBetObj.MemberID);
            _memberLay = _memberSvr.Member(_LayBetObj.MemberID);
            SettledTime = DateTime.Now;
        }

        public void DoSettle()
        {
            double MaximumMarketRate = GetMaxMarketRate(0) / 100;
            // xu ly cho truong hop tran dau bi Postponed hoac bi Cancelled		
            if (((SoccerMatchObj.MatchStatus.Equals("Postponed")) || ((SoccerMatchObj.MatchStatus.Equals("Cancelled")))))
            {
                //xu ly giong nhu DRAW 	
                string whichBetWin = "D"; bool Win100 = false; bool Win50 = false;
                ExecuteSettlement((int)OddsTypes.SoccerAsianHandicap, whichBetWin, Win100, Win50, MaximumMarketRate);
                return;
            }

            //Settle cho loai ca cuoc : Match Odds		
            if (BackBetObj.OddsTable.Equals("[dbo].[Soccer_MatchOdds]"))
            {
                // Doi voi loai ca cuoc nay, thuc ra ta cung khong can dung het data trong table [dbo].[Soccer_MatchOdds] !	
                // thuc ra ta chi su dung 2 fields : "Period", "Entrants" trong table [dbo].[Soccer_MatchOdds] de biet duoc la user bet hiep thi dau nao , so luong entrants cua loai odds nay	

                SoccerMatchOddsService _soccerMatchOddsSvr = new SoccerMatchOddsService();
                Soccer_MatchOdds _soccerMatchOdds = new Soccer_MatchOdds();
                List<Soccer_MatchOdds> _SoccerMatchOddsesList = _soccerMatchOddsSvr.SoccerMatchOddses((long)BackBetObj.SportID, (long)BackBetObj.CountryID, (long)BackBetObj.LeagueID, BackBetObj.MatchID);
                _soccerMatchOdds = _SoccerMatchOddsesList[0];




                //MaximumMarketRate tinh theo %	
                MaximumMarketRate = GetMaxMarketRate(_soccerMatchOdds.Entrants) / 100;

                if ((int)_soccerMatchOdds.Period == (int)Soccer_Periods.FT)
                {							//tran dau ket thuc FT						
                    if (SoccerMatchObj.HomeTeam_FTGoals - SoccerMatchObj.AwayTeam_FTGoals > 0)
                    {
                        //Result : Home Team win !!!!!!											
                        //Home Team BACK BET : WIN |   Away Team BACK BET : LOSE											
                        //Home Team LAY BET : LOSE |   Away Team LAY BET : WIN											
                        //DRAW BACK BET : LOSE |   DRAW LAY BET : WIN											

                        //Xet MyEntrant de biet la dat cua nao de ma xu ly tuong ung											
                        //Tham khao class MyBet o ben duoi											

                        string whichBetWin = ""; bool Win100 = true; bool Win50 = false;
                        if ((BackBetObj.MyEntrant == 1) && (LayBetObj.MyEntrant == 1))
                        {
                            whichBetWin = "B";
                        }
                        else if ((BackBetObj.MyEntrant == 3) && (LayBetObj.MyEntrant == 3))
                        {
                            whichBetWin = "L";
                        }
                        else if ((BackBetObj.MyEntrant == 2) && (LayBetObj.MyEntrant == 2))
                        {
                            whichBetWin = "L";
                        }
                        ExecuteSettlement((int)OddsTypes.SoccerMatchOdds, whichBetWin, Win100, Win50, MaximumMarketRate);



                    }
                    else if (SoccerMatchObj.HomeTeam_FTGoals - SoccerMatchObj.AwayTeam_FTGoals == 0)
                    {
                        //Result : Draw !!!!!											
                        //Home Team BACK BET : LOSE |   Away Team BACK BET : LOSE											
                        //Home Team LAY BET : WIN |   Away Team LAY BET : WIN											
                        //DRAW BACK BET : WIN |   DRAW LAY BET : LOSE											

                        //Xet MyEntrant de biet la dat cua nao de ma xu ly tuong ung											
                        //Tham khao class MyBet o ben duoi											
                        string whichBetWin = ""; bool Win100 = true; bool Win50 = false;
                        if ((BackBetObj.MyEntrant == 1) && (LayBetObj.MyEntrant == 1))
                        {
                            whichBetWin = "L";
                        }
                        else if ((BackBetObj.MyEntrant == 3) && (LayBetObj.MyEntrant == 3))
                        {
                            whichBetWin = "L";
                        }
                        else if ((BackBetObj.MyEntrant == 2) && (LayBetObj.MyEntrant == 2))
                        {
                            whichBetWin = "B";
                        }
                        ExecuteSettlement((int)OddsTypes.SoccerMatchOdds, whichBetWin, Win100, Win50, MaximumMarketRate);

                    }
                    else if (SoccerMatchObj.HomeTeam_FTGoals - SoccerMatchObj.AwayTeam_FTGoals < 0)
                    {
                        //Result : Away Team Win !!!!											
                        //Home Team BACK BET : LOSE |   Away Team BACK BET : WIN											
                        //Home Team LAY BET : WIN |   Away Team LAY BET : LOSE											
                        //DRAW BACK BET : LOSE |   DRAW LAY BET : WIN											

                        //Xet MyEntrant de biet la dat cua nao de ma xu ly tuong ung											
                        //Tham khao class MyBet o ben duoi											

                        string whichBetWin = ""; bool Win100 = true; bool Win50 = false;
                        if ((BackBetObj.MyEntrant == 1) && (LayBetObj.MyEntrant == 1))
                        {
                            whichBetWin = "L";
                        }
                        else if ((BackBetObj.MyEntrant == 3) && (LayBetObj.MyEntrant == 3))
                        {
                            whichBetWin = "B";
                        }
                        else if ((BackBetObj.MyEntrant == 2) && (LayBetObj.MyEntrant == 2))
                        {
                            whichBetWin = "L";
                        }
                        ExecuteSettlement((int)OddsTypes.SoccerMatchOdds, whichBetWin, Win100, Win50, MaximumMarketRate);
                    }
                }


            }


            //Settle cho loai ca cuoc : TotalGoalsOU		


            else if (BackBetObj.OddsTable.Equals("[dbo].[Soccer_TotalGoalsOU]"))
            {
                SoccerTotalGoalsOUService _soccerTotalGoalOUSvr = new SoccerTotalGoalsOUService();
                List<Soccer_TotalGoalsOU> _soccerTotalGoalsOUs = _soccerTotalGoalOUSvr.SoccerTotalGoalsOUs((long)BackBetObj.SportID, (long)BackBetObj.CountryID, (long)BackBetObj.LeagueID, BackBetObj.MatchID);

                Soccer_TotalGoalsOU _soccerTotalGoalsOU = _soccerTotalGoalsOUs[0];


                //MaximumMarketRate tinh theo %													
                MaximumMarketRate = GetMaxMarketRate(_soccerTotalGoalsOU.Entrants) / 100;

                int TotalGoals = 0;		//tong so ban thang 											
                if ((int)_soccerTotalGoalsOU.Period == (int)Soccer_Periods.FT)
                {								// Neu day la keo Over/Under 90 phut FT					
                    TotalGoals = (int)SoccerMatchObj.HomeTeam_FTGoals + (int)SoccerMatchObj.HomeTeam_FTGoals;
                }
                else if ((int)_soccerTotalGoalsOU.Period == (int)Soccer_Periods.T1stHalf)
                {								// Neu day la keo Over/Under 45 phut hiep mot					
                    TotalGoals = (int)SoccerMatchObj.HomeTeam_1stHalfGoals + (int)SoccerMatchObj.AwayTeam_1stHalfGoals;
                }


                if (TotalGoals - _soccerTotalGoalsOU.OU == 0)
                {
                    //ALL DRAW !!!!												
                    string whichBetWin = "D"; bool Win100 = false; bool Win50 = false;
                    ExecuteSettlement((int)OddsTypes.SoccerTotalGoalsOU, whichBetWin, Win100, Win50, MaximumMarketRate);

                }
                else if (TotalGoals - Convert.ToDouble(_soccerTotalGoalsOU.OU) == 0.25)
                {
                    //OVER BACK BET :  WIN 1/2 |   UNDER BACK BET : LOSE 1/2												
                    //OVER LAY BET : LOSE 1/2 |   UNDER LAY BET : WIN 1/2												
                    //Xet MyEntrant de biet la dat cua nao de ma xu ly tuong ung												
                    //Tham khao class MyBet o ben duoi												
                    string whichBetWin = ""; bool Win100 = false; bool Win50 = true;
                    if ((BackBetObj.MyEntrant == 1) && (LayBetObj.MyEntrant == 1))
                    {
                        whichBetWin = "B";
                    }
                    else if ((BackBetObj.MyEntrant == 2) && (LayBetObj.MyEntrant == 2))
                    {
                        whichBetWin = "L";
                    }
                    ExecuteSettlement((int)OddsTypes.SoccerTotalGoalsOU, whichBetWin, Win100, Win50, MaximumMarketRate);

                }
                else if (Convert.ToDouble(_soccerTotalGoalsOU.OU) - TotalGoals == 0.25)
                {
                    //OVER BACK BET :  LOSE 1/2 |   UNDER BACK BET : LOSE 1/2												
                    //OVER LAY BET : WIN 1/2 |   UNDER LAY BET : WIN 1/2												
                    //Xet MyEntrant de biet la dat cua nao de ma xu ly tuong ung												
                    //Tham khao class MyBet o ben duoi												
                    string whichBetWin = ""; bool Win100 = false; bool Win50 = true;
                    if ((BackBetObj.MyEntrant == 1) && (LayBetObj.MyEntrant == 1))
                    {
                        whichBetWin = "L";
                    }
                    else if ((BackBetObj.MyEntrant == 2) && (LayBetObj.MyEntrant == 2))
                    {
                        whichBetWin = "L";
                    }
                    ExecuteSettlement((int)OddsTypes.SoccerTotalGoalsOU, whichBetWin, Win100, Win50, MaximumMarketRate);

                }
                else if (TotalGoals - Convert.ToDouble(_soccerTotalGoalsOU.OU) >= 0.5)
                {
                    //OVER BACK BET :  WIN |   UNDER BACK BET : LOSE												
                    //OVER LAY BET : LOSE |   UNDER LAY BET : WIN												
                    //Xet MyEntrant de biet la dat cua nao de ma xu ly tuong ung												
                    //Tham khao class MyBet o ben duoi												
                    string whichBetWin = ""; bool Win100 = true; bool Win50 = false;
                    if ((BackBetObj.MyEntrant == 1) && (LayBetObj.MyEntrant == 1))
                    {
                        whichBetWin = "B";
                    }
                    else if ((BackBetObj.MyEntrant == 2) && (LayBetObj.MyEntrant == 2))
                    {
                        whichBetWin = "L";
                    }
                    ExecuteSettlement((int)OddsTypes.SoccerTotalGoalsOU, whichBetWin, Win100, Win50, MaximumMarketRate);
                }
                else if (Convert.ToDouble(_soccerTotalGoalsOU.OU) - TotalGoals <= 0.5)
                {
                    //OVER BACK BET :  LOSE |   UNDER BACK BET : WIN												
                    //OVER LAY BET : WIN |   UNDER LAY BET : LOSE												
                    //Xet MyEntrant de biet la dat cua nao de ma xu ly tuong ung												
                    //Tham khao class MyBet o ben duoi												
                    string whichBetWin = ""; bool Win100 = true; bool Win50 = false;
                    if ((BackBetObj.MyEntrant == 1) && (LayBetObj.MyEntrant == 1))
                    {
                        whichBetWin = "L";
                    }
                    else if ((BackBetObj.MyEntrant == 2) && (LayBetObj.MyEntrant == 2))
                    {
                        whichBetWin = "B";
                    }
                    ExecuteSettlement((int)OddsTypes.SoccerTotalGoalsOU, whichBetWin, Win100, Win50, MaximumMarketRate);
                }




                  //Settle cho loai ca cuoc : Soccer AsianHandicap  (GIAI THUAT GAN GIONG NHU OVER/UNDER )		
                else if (BackBetObj.OddsTable.Equals("[dbo].[Soccer_AsianHandicap]"))
                {
                    SoccerAsianHandicapService _soccerHandicapSvr = new SoccerAsianHandicapService();


                    List<Soccer_AsianHandicap> _soccerAsianHandicaps = _soccerHandicapSvr.SoccerAsianHandicaps((long)BackBetObj.SportID, (long)BackBetObj.CountryID, (long)BackBetObj.LeagueID, BackBetObj.MatchID);

                    Soccer_AsianHandicap _soccerAsianHandicap = _soccerAsianHandicaps[0];


                    //MaximumMarketRate tinh theo %																
                    MaximumMarketRate = GetMaxMarketRate((int)_soccerAsianHandicap.Entrants) / 100;

                    double GoalsDiff = 0;		//so ban thang chenh lech giua keo tren va keo duoi														
                    if ((int)_soccerAsianHandicap.Period == (int)Soccer_Periods.FT)
                    {								// Neu day la keo Over/Under 90 phut FT								
                        GoalsDiff = (int)SoccerMatchObj.HomeTeam_FTGoals - (int)SoccerMatchObj.AwayTeam_FTGoals;
                    }
                    else if (_soccerAsianHandicap.Period == (int)Soccer_Periods.T1stHalf)
                    {								// Neu day la keo Over/Under 45 phut hiep mot								
                        GoalsDiff = (int)SoccerMatchObj.HomeTeam_1stHalfGoals - (int)SoccerMatchObj.AwayTeam_1stHalfGoals;
                    }

                    if (float.Parse(_soccerAsianHandicap.HomeHandicap) <= 0)
                    {							//Home chap banh									
                        if (GoalsDiff >= 0)
                        {						//Home draw to win match !									
                            if (float.Parse(_soccerAsianHandicap.HomeHandicap) + GoalsDiff == 0)
                            {
                                //ALL DRAW !!!!													
                                string whichBetWin = "D"; bool Win100 = false; bool Win50 = false;
                                ExecuteSettlement((int)OddsTypes.SoccerAsianHandicap, whichBetWin, Win100, Win50, MaximumMarketRate);
                            }
                            else if (float.Parse(_soccerAsianHandicap.HomeHandicap) + GoalsDiff == 0.25)
                            {
                                //HOME-WIN-HANDICAP BACK BET :  WIN 1/2 |  HOME-WIN-HANDICAP LAY BET :  LOSE 1/2													
                                //AWAY-WIN-HANDICAP BACK BET :  LOSE 1/2 |  AWAY-WIN-HANDICAP LAY BET :  WIN 1/2													
                                //Xet MyEntrant de biet la dat cua nao de ma xu ly tuong ung													
                                //Tham khao class MyBet o ben duoi													
                                string whichBetWin = ""; bool Win100 = false; bool Win50 = true;
                                if ((BackBetObj.MyEntrant == 1) && (LayBetObj.MyEntrant == 1))
                                {
                                    whichBetWin = "B";
                                }
                                else if ((BackBetObj.MyEntrant == 2) && (LayBetObj.MyEntrant == 2))
                                {
                                    whichBetWin = "L";
                                }
                                ExecuteSettlement((int)OddsTypes.SoccerAsianHandicap, whichBetWin, Win100, Win50, MaximumMarketRate);
                            }
                            else if (float.Parse(_soccerAsianHandicap.HomeHandicap) + GoalsDiff == -0.25)
                            {
                                //HOME-WIN-HANDICAP BACK BET :  LOSE 1/2 |  HOME-WIN-HANDICAP LAY BET :  WIN 1/2													
                                //AWAY-WIN-HANDICAP BACK BET :  WIN 1/2 |  AWAY-WIN-HANDICAP LAY BET :  LOSE 1/2													
                                //Xet MyEntrant de biet la dat cua nao de ma xu ly tuong ung													
                                //Tham khao class MyBet o ben duoi													
                                string whichBetWin = ""; bool Win100 = false; bool Win50 = true;
                                if ((BackBetObj.MyEntrant == 1) && (LayBetObj.MyEntrant == 1))
                                {
                                    whichBetWin = "L";
                                }
                                else if ((BackBetObj.MyEntrant == 2) && (LayBetObj.MyEntrant == 2))
                                {
                                    whichBetWin = "B";
                                }
                                ExecuteSettlement((int)OddsTypes.SoccerAsianHandicap, whichBetWin, Win100, Win50, MaximumMarketRate);
                            }
                            else if (float.Parse(_soccerAsianHandicap.HomeHandicap) + GoalsDiff <= -0.5)
                            {
                                //HOME-WIN-HANDICAP BACK BET :  LOSE  |  HOME-WIN-HANDICAP LAY BET :  WIN													
                                //AWAY-WIN-HANDICAP BACK BET :  WIN  |  AWAY-WIN-HANDICAP LAY BET :  LOSE 													
                                //Xet MyEntrant de biet la dat cua nao de ma xu ly tuong ung													
                                //Tham khao class MyBet o ben duoi													
                                string whichBetWin = ""; bool Win100 = true; bool Win50 = false;
                                if ((BackBetObj.MyEntrant == 1) && (LayBetObj.MyEntrant == 1))
                                {
                                    whichBetWin = "L";
                                }
                                else if ((BackBetObj.MyEntrant == 2) && (LayBetObj.MyEntrant == 2))
                                {
                                    whichBetWin = "B";
                                }
                                ExecuteSettlement((int)OddsTypes.SoccerAsianHandicap, whichBetWin, Win100, Win50, MaximumMarketRate);
                            }
                            else if (float.Parse(_soccerAsianHandicap.HomeHandicap) + GoalsDiff >= 0.5)
                            {
                                //HOME-WIN-HANDICAP BACK BET :  WIN  |  HOME-WIN-HANDICAP LAY BET :  LOSE													
                                //AWAY-WIN-HANDICAP BACK BET :  LOSE  |  AWAY-WIN-HANDICAP LAY BET :  WIN													
                                //Xet MyEntrant de biet la dat cua nao de ma xu ly tuong ung													
                                //Tham khao class MyBet o ben duoi													
                                string whichBetWin = ""; bool Win100 = true; bool Win50 = false;
                                if ((BackBetObj.MyEntrant == 1) && (LayBetObj.MyEntrant == 1))
                                {
                                    whichBetWin = "B";
                                }
                                else if ((BackBetObj.MyEntrant == 2) && (LayBetObj.MyEntrant == 2))
                                {
                                    whichBetWin = "L";
                                }
                                ExecuteSettlement((int)OddsTypes.SoccerAsianHandicap, whichBetWin, Win100, Win50, MaximumMarketRate);
                            }
                        }
                        else
                        {	//Home lose match														
                            //HOME-WIN-HANDICAP BACK BET :  LOSE  |  HOME-WIN-HANDICAP LAY BET :  WIN														
                            //AWAY-WIN-HANDICAP BACK BET :  WIN  |  AWAY-WIN-HANDICAP LAY BET :  LOSE														
                            //Xet MyEntrant de biet la dat cua nao de ma xu ly tuong ung														
                            //Tham khao class MyBet o ben duoi														
                            string whichBetWin = ""; bool Win100 = true; bool Win50 = false;
                            if ((BackBetObj.MyEntrant == 1) && (LayBetObj.MyEntrant == 1))
                            {
                                whichBetWin = "L";
                            }
                            else if ((BackBetObj.MyEntrant == 2) && (LayBetObj.MyEntrant == 2))
                            {
                                whichBetWin = "B";
                            }
                            ExecuteSettlement((int)OddsTypes.SoccerAsianHandicap, whichBetWin, Win100, Win50, MaximumMarketRate);
                        }
                    }
                    else if (float.Parse(_soccerAsianHandicap.HomeHandicap) > 0)
                    {							//Away chap banh									
                        if (GoalsDiff <= 0)
                        {						//Away draw to win match !									
                            if (float.Parse(_soccerAsianHandicap.HomeHandicap) + GoalsDiff == -0.25)
                            {
                                //HOME-WIN-HANDICAP BACK BET :  WIN 1/2 |  HOME-WIN-HANDICAP LAY BET :  LOSE 1/2													
                                //AWAY-WIN-HANDICAP BACK BET :  LOSE 1/2 |  AWAY-WIN-HANDICAP LAY BET :  WIN 1/2													
                                //Xet MyEntrant de biet la dat cua nao de ma xu ly tuong ung													
                                //Tham khao class MyBet o ben duoi													
                                string whichBetWin = ""; bool Win100 = false; bool Win50 = true;
                                if ((BackBetObj.MyEntrant == 1) && (LayBetObj.MyEntrant == 1))
                                {
                                    whichBetWin = "B";
                                }
                                else if ((BackBetObj.MyEntrant == 2) && (LayBetObj.MyEntrant == 2))
                                {
                                    whichBetWin = "L";
                                }
                                ExecuteSettlement((int)OddsTypes.SoccerAsianHandicap, whichBetWin, Win100, Win50, MaximumMarketRate);
                            }
                            else if (float.Parse(_soccerAsianHandicap.HomeHandicap) + GoalsDiff == 0.25)
                            {
                                //HOME-WIN-HANDICAP BACK BET :  LOSE 1/2 |  HOME-WIN-HANDICAP LAY BET :  WIN 1/2													
                                //AWAY-WIN-HANDICAP BACK BET :  WIN 1/2 |  AWAY-WIN-HANDICAP LAY BET :  LOSE 1/2													
                                //Xet MyEntrant de biet la dat cua nao de ma xu ly tuong ung													
                                //Tham khao class MyBet o ben duoi													
                                string whichBetWin = ""; bool Win100 = false; bool Win50 = true;
                                if ((BackBetObj.MyEntrant == 1) && (LayBetObj.MyEntrant == 1))
                                {
                                    whichBetWin = "L";
                                }
                                else if ((BackBetObj.MyEntrant == 2) && (LayBetObj.MyEntrant == 2))
                                {
                                    whichBetWin = "B";
                                }
                                ExecuteSettlement((int)OddsTypes.SoccerAsianHandicap, whichBetWin, Win100, Win50, MaximumMarketRate);
                            }
                            else if (float.Parse(_soccerAsianHandicap.HomeHandicap) + GoalsDiff <= -0.5)
                            {
                                //HOME-WIN-HANDICAP BACK BET :  LOSE  |  HOME-WIN-HANDICAP LAY BET :  WIN													
                                //AWAY-WIN-HANDICAP BACK BET :  WIN  |  AWAY-WIN-HANDICAP LAY BET :  LOSE 													
                                //Xet MyEntrant de biet la dat cua nao de ma xu ly tuong ung													
                                //Tham khao class MyBet o ben duoi													
                                string whichBetWin = ""; bool Win100 = true; bool Win50 = false;
                                if ((BackBetObj.MyEntrant == 1) && (LayBetObj.MyEntrant == 1))
                                {
                                    whichBetWin = "L";
                                }
                                else if ((BackBetObj.MyEntrant == 2) && (LayBetObj.MyEntrant == 2))
                                {
                                    whichBetWin = "B";
                                }
                                ExecuteSettlement((int)OddsTypes.SoccerAsianHandicap, whichBetWin, Win100, Win50, MaximumMarketRate);
                            }
                            else if (float.Parse(_soccerAsianHandicap.HomeHandicap) + GoalsDiff >= 0.5)
                            {
                                //HOME-WIN-HANDICAP BACK BET :  WIN  |  HOME-WIN-HANDICAP LAY BET :  LOSE													
                                //AWAY-WIN-HANDICAP BACK BET :  LOSE  |  AWAY-WIN-HANDICAP LAY BET :  WIN													
                                //Xet MyEntrant de biet la dat cua nao de ma xu ly tuong ung													
                                //Tham khao class MyBet o ben duoi													
                                string whichBetWin = ""; bool Win100 = true; bool Win50 = false;
                                if ((BackBetObj.MyEntrant == 1) && (LayBetObj.MyEntrant == 1))
                                {
                                    whichBetWin = "B";
                                }
                                else if ((BackBetObj.MyEntrant == 2) && (LayBetObj.MyEntrant == 2))
                                {
                                    whichBetWin = "L";
                                }
                                ExecuteSettlement((int)OddsTypes.SoccerAsianHandicap, whichBetWin, Win100, Win50, MaximumMarketRate);
                            }
                        }
                        else
                        {	//Away lose match														
                            //HOME-WIN-HANDICAP BACK BET :  WIN  |  HOME-WIN-HANDICAP LAY BET :  LOSE														
                            //AWAY-WIN-HANDICAP BACK BET :  LOSE  |  AWAY-WIN-HANDICAP LAY BET :  WIN														
                            //Xet MyEntrant de biet la dat cua nao de ma xu ly tuong ung														
                            //Tham khao class MyBet o ben duoi														
                            string whichBetWin = ""; bool Win100 = true; bool Win50 = false;
                            if ((BackBetObj.MyEntrant == 1) && (LayBetObj.MyEntrant == 1))
                            {
                                whichBetWin = "B";
                            }
                            else if ((BackBetObj.MyEntrant == 2) && (LayBetObj.MyEntrant == 2))
                            {
                                whichBetWin = "L";
                            }
                            ExecuteSettlement((int)OddsTypes.SoccerAsianHandicap, whichBetWin, Win100, Win50, MaximumMarketRate);
                        }
                    }
                }
                //Settle cho loai ca cuoc : Soccer DrawNoBet 																	
                else if (BackBetObj.OddsTable.Equals("[dbo].[Soccer_DrawNoBet]"))
                {
                    SoccerDrawNoBetService _soccerDrawpSvr = new SoccerDrawNoBetService();


                    List<Soccer_DrawNoBet> _soccerAsianHandicaps = _soccerDrawpSvr.SoccerDrawNoBets((long)BackBetObj.SportID, (long)BackBetObj.CountryID, (long)BackBetObj.LeagueID, BackBetObj.MatchID);

                    Soccer_DrawNoBet _soccerDrawNoBet = _soccerAsianHandicaps[0];



                    //MaximumMarketRate tinh theo %																
                    MaximumMarketRate = GetMaxMarketRate(_soccerDrawNoBet.Entrants) / 100;
                    //….																
                    //To be updated later….!																
                }
                //Settle cho loai ca cuoc : Soccer CorrectScores																	
                else if (BackBetObj.OddsTable.Equals("[dbo].[Soccer_CorrectScores]"))
                {
                    //….																
                    //To be updated later….!																
                }




            }
        }


        //Function to calculate MaximumMarketRate			
        public int GetMaxMarketRate(int _entrants)
        {
            int maxMarketRate = 3;
            if (_entrants <= 3)
            {
                maxMarketRate = 3;
            }
            else if (_entrants == 4)
            {
                maxMarketRate = 4;
            }
            else if (_entrants >= 5)
            {
                maxMarketRate = 5;
            }
            return maxMarketRate;
        }

        //Function to update MyWallet			
        public void UpdateMyWalet(MyBet _myBet)
        {

            MyWallet _myWallet = new MyWallet();

            MyWalletService _myWalletSvr = new MyWalletService();
            MyBetService _myBerSvr = new MyBetService();

            _myWallet = _myWalletSvr.MyWalletbyMemberId(_myBet.MemberID);//SELECT * FROM [dbo].[MyWallet] WHERE [dbo].[MyWallet].[MemberID] = _myBet.MemberID;		

            //Calculate total of exposures for my bets which has not been settled yet !		
            double TotalExposures = 0;
            List<double> BetExposuresList = new List<double>();

            List<MyBet> _mybetList = _myBerSvr.MyBetExposuresList(_myBet.ID);

            foreach (MyBet _mb in _mybetList)
            {
                TotalExposures += Convert.ToDouble(_mb.Exposure);
            }
            //BetExposuresList = SELECT Exposure FROM [dbo].[MyBets] WHERE [dbo].[MyBets].[ID] <> _myBet.ID		


            //For Each Exposure in BetExposuresList {		
            //TotalExposures += Exposure;	
            //}		
            // Update Wallet Balance		
            _myWallet.Balance = _myWallet.Balance + _myBet.NetProfit - Convert.ToDecimal(TotalExposures);
            // Update Wallet Available		
            _myWallet.Available = _myWallet.Balance - Convert.ToDecimal(TotalExposures);

            //update lai table [dbo].[MyWallet]		
            _myWalletSvr.Update(_myWallet);
        }


        //Function to Execute Settle			
        public void ExecuteSettlement(int OddsType, string whichBetWin, bool Win100, bool Win50, double MaximumMarketRate)
        {
            if (whichBetWin.Equals("B"))
            {
                //settle win for BACK bet	
                BackBetObj.IsWon = true;
                BackBetObj.IsDraw = false;
                BackBetObj.Exposure = 0;
                BackBetObj.Liability = 0;
                BackBetObj.Payouts = 0;

                if (Win100)
                {	// win full 100%
                    BackBetObj.GrossProfit = BackBetObj.Stake * (BackBetObj.Price - 1);
                }
                else
                {	// win 1/2
                    BackBetObj.GrossProfit = Convert.ToDecimal(BackBetObj.Stake) * Convert.ToDecimal(BackBetObj.Price - 1) * Decimal.Parse("0.5");
                }
                // cong thuc tinh Commission = GrossProfit x MaximumMarketRate x (1 - (DiscountRate / 100) ) 	
                BackBetObj.Commission = Convert.ToDecimal((Decimal)BackBetObj.GrossProfit) * (1 - _memberBack.DiscountRate / 100) * (decimal)(MaximumMarketRate);
                BackBetObj.NetProfit = BackBetObj.GrossProfit - BackBetObj.Commission;

                //Update Points bang function : UpdatePoints() duoc xay dung san trong class Member, giai thuat tinh nhu sau :	
                //       BackBetObj.PointsRefunded  =  BackBetObj.GrossProfit x MaximumMarketRate x (1 - _memberBack.DiscountRate / 100) 	
                //      [dbo].[Members].[TotalPoints] = [dbo].[Members].[TotalPoints] + BackBetObj.PointsRefunded	


                //_memberBack.UpdatePoints();	
                BackBetObj.PointsRefunded = BackBetObj.Exposure * Convert.ToDecimal(MaximumMarketRate) * (1 - _memberBack.DiscountRate / 100);
                _memberBack.TotalPoints = (int)(_memberBack.TotalPoints + BackBetObj.PointsRefunded);

                //Update DiscountRate bang function : UpdateDiscountRate() duoc xay dung san trong class Member	
                //(can cu va bang tham chieu ben phai !)	


                _memberBack.DiscountRate = findDiscount((int)_memberBack.TotalPoints);

                //call function UpdateMyWallet	
                MyBetService _mbSvr = new MyBetService();

                UpdateMyWalet(BackBetObj);

                //cuoi cung, set cac field can thiet con lai de hoan tat Settle	
                BackBetObj.SettledTime = SettledTime;
                BackBetObj.BetStatus = "Settled";
                //update table [dbo].[MyBets]	


                //BackBetObj.Update();	
                _mbSvr.Update(BackBetObj);

                //settle lose for LAY bet	
                LayBetObj.IsWon = false;
                LayBetObj.IsDraw = false;
                LayBetObj.GrossProfit = 0 - LayBetObj.Exposure;
                LayBetObj.Commission = 0;
                LayBetObj.NetProfit = 0 - LayBetObj.Exposure;

                //Update Points bang function : UpdatePoints() duoc xay dung san trong class Member, giai thuat tinh nhu sau :	
                //       LayBetObj.PointsRefunded  =  LayBetObj.Exposure x MaximumMarketRate x (1 - _memberLay.DiscountRate / 100) 	
                //      [dbo].[Members].[TotalPoints] = [dbo].[Members].[TotalPoints] + LayBetObj.PointsRefunded	


                //_memberLay.UpdatePoints();	
                LayBetObj.PointsRefunded = BackBetObj.Exposure * Convert.ToDecimal(MaximumMarketRate) * (1 - _memberLay.DiscountRate / 100);
                _memberLay.TotalPoints = (long)(_memberLay.TotalPoints + LayBetObj.PointsRefunded);

                //Update DiscountRate bang function : UpdateDiscountRate() duoc xay dung san trong class Member	
                //(can cu va bang tham chieu ben phai !)	


                //
                // _memberLay.UpdateDiscountRate();	

                _memberLay.DiscountRate = findDiscount((int)_memberLay.TotalPoints);

                //call function UpdateMyWallet	



                UpdateMyWalet(LayBetObj);

                //cuoi cung, set cac field can thiet con lai de hoan tat Settle	
                LayBetObj.SettledTime = SettledTime;
                LayBetObj.BetStatus = "Settled";
                //update table [dbo].[MyBets]	


                // LayBetObj.Update();
                _mbSvr.Update(LayBetObj);

            }
            else if (whichBetWin.Equals("L"))
            {
                //settle win for LAY bet	
                LayBetObj.IsWon = true;
                LayBetObj.IsDraw = false;
                LayBetObj.Exposure = 0;
                LayBetObj.Liability = 0;
                LayBetObj.Payouts = 0;

                if (Win100)
                {	// win full 100%
                    LayBetObj.GrossProfit = LayBetObj.Stake;
                }
                else
                {	// win 1/2
                    LayBetObj.GrossProfit = decimal.Parse("0.5") * LayBetObj.Stake;
                }
                // cong thuc tinh Commission = GrossProfit x MaximumMarketRate x (1 - (DiscountRate / 100) ) 	
                LayBetObj.Commission = LayBetObj.GrossProfit * (decimal)MaximumMarketRate * (1 - _memberLay.DiscountRate / 100);
                LayBetObj.NetProfit = LayBetObj.GrossProfit - LayBetObj.Commission;

                //Update Points bang function : UpdatePoints() duoc xay dung san trong class Member, giai thuat tinh nhu sau :	
                //       LayBetObj.PointsRefunded  =  LayBetObj.GrossProfit x MaximumMarketRate x (1 - _memberLay.DiscountRate / 100) 	
                //      [dbo].[Members].[TotalPoints] = [dbo].[Members].[TotalPoints] + LayBetObj.PointsRefunded	


                // _memberLay.UpdatePoints();
                BackBetObj.PointsRefunded = BackBetObj.Exposure * Convert.ToDecimal(MaximumMarketRate) * (1 - _memberBack.DiscountRate / 100);
                _memberLay.TotalPoints = (long)(_memberLay.TotalPoints + LayBetObj.PointsRefunded);

                //Update DiscountRate bang function : UpdateDiscountRate() duoc xay dung san trong class Member	
                //(can cu va bang tham chieu ben phai !)	


                // _memberLay.UpdateDiscountRate();	
                _memberBack.DiscountRate = findDiscount((int)_memberLay.TotalPoints);

                //call function UpdateMyWallet	


                UpdateMyWalet(LayBetObj);

                //cuoi cung, set cac field can thiet con lai de hoan tat Settle	
                LayBetObj.SettledTime = SettledTime;
                LayBetObj.BetStatus = "Settled";
                //update table [dbo].[MyBets]	

                MyBetService _mbSvr = new MyBetService();
                //LayBetObj.Update();	
                _mbSvr.Update(LayBetObj);

                //settle lose for BACK bet	
                BackBetObj.IsWon = false;
                BackBetObj.IsDraw = false;
                BackBetObj.GrossProfit = 0 - BackBetObj.Stake;
                LayBetObj.Commission = 0;
                LayBetObj.NetProfit = 0 - BackBetObj.Stake;

                //Update Points bang function : UpdatePoints() duoc xay dung san trong class Member, giai thuat tinh nhu sau :	
                //       BackBetObj.PointsRefunded  =  BackBetObj.Exposure x MaximumMarketRate x (1 - _memberBack.DiscountRate / 100) 	
                //      [dbo].[Members].[TotalPoints] = [dbo].[Members].[TotalPoints] + BackBetObj.PointsRefunded	


                // _memberBack.UpdatePoints();

                BackBetObj.PointsRefunded = BackBetObj.Exposure * Convert.ToDecimal(MaximumMarketRate) * (1 - _memberBack.DiscountRate / 100);
                _memberBack.TotalPoints = (long)(_memberBack.TotalPoints + BackBetObj.PointsRefunded);
                //      [dbo].[Members].[TotalPoints] = [dbo].[Members].[TotalPoints] + BackBetObj.PointsRefunded;

                //Update DiscountRate bang function : UpdateDiscountRate() duoc xay dung san trong class Member	
                //(can cu va bang tham chieu ben phai !)	


                // _memberBack.UpdateDiscountRate();	
                _memberBack.DiscountRate = findDiscount((int)_memberBack.TotalPoints);
                //call function UpdateMyWallet	


                UpdateMyWalet(BackBetObj);

                //cuoi cung, set cac field can thiet con lai de hoan tat Settle	
                BackBetObj.SettledTime = SettledTime;
                BackBetObj.BetStatus = "Settled";
                //update table [dbo].[MyBets]	


                // BackBetObj.Update();
                _mbSvr.Update(BackBetObj);

            }
            else if (whichBetWin.Equals("D"))
            {
                //settle draw for BACK bet + LAY bet	
                BackBetObj.IsWon = false;
                BackBetObj.IsDraw = true;
                BackBetObj.GrossProfit = 0;
                BackBetObj.Commission = 0;
                BackBetObj.NetProfit = 0;
                BackBetObj.Exposure = 0;
                BackBetObj.Liability = 0;
                BackBetObj.Payouts = 0;
                LayBetObj.IsWon = false;
                LayBetObj.IsDraw = true;
                LayBetObj.GrossProfit = 0;
                LayBetObj.Commission = 0;
                LayBetObj.NetProfit = 0;
                LayBetObj.Exposure = 0;
                LayBetObj.Liability = 0;
                LayBetObj.Payouts = 0;
                //cuoi cung, set cac field can thiet con lai de hoan tat Settle	
                BackBetObj.SettledTime = SettledTime;
                LayBetObj.SettledTime = SettledTime;
                BackBetObj.BetStatus = "Settled";
                LayBetObj.BetStatus = "Settled";

                //update table [dbo].[MyBets]	

                // BackBetObj.Update();	
                // LayBetObj.Update();
                MyBetService _mbSvr = new MyBetService();
                //LayBetObj.Update();	
                _mbSvr.Update(BackBetObj);
                _mbSvr.Update(LayBetObj);

            }

        }
        public int findDiscount(int point)
        {
            for (int i = 0; i < DiscountPercent.Length; i++)
            {
                if (DiscountPercent[i] <= point)
                {
                    return i;
                }
            }
            if (point > DiscountPercent[DiscountPercent.Length - 1])
            {
                return DiscountPercent.Length - 1;
            }
            return 0;
        }

        //Function to update MyWallet									
      
    }	//end class BetSettlement			

}
