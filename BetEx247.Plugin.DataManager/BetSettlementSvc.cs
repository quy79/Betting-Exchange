using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.DAL.Sports;
using BetEx247.Data.Model;
using BetEx247.Plugin.DataManager.Settle;
namespace BetEx247.Plugin.DataManager
{
    public class BetSettlementSvc
    {
        DateTime Local_ServiceDateTime;
        DateTime Goalserve_ServiceDateTimeGMT;

        public void doSetle4Type1()
        {
            SoccerMatchService _srv = new SoccerMatchService();
            List<SoccerMatch> _SoccerMatchesList = _srv.SoccerMatches4Settle(false);
            foreach (SoccerMatch _soccerMatch in _SoccerMatchesList)
            {
                DateTime localServerDateTime = DateTime.Now;
                DateTime goalserveDateTime = localServerDateTime.AddHours(-1);
                int dateCompare = DateTime.Compare(goalserveDateTime, (DateTime)_soccerMatch.MarketCloseTime);
                if (dateCompare > 0)
                {

                    // cap nhat BetStatus cua nhung UnMatched Bets →　"Lapsed"	

                    _soccerMatch.Settled = true;
                    _soccerMatch.MatchStatus = "Lapsed";
                    _srv.Update4Settle(_soccerMatch);

                }
            }


        }
        public void doSetle4Type2()
        {
            //	Ok, sau khi chuan bi xong cac data object can thiet, ta buoc vao phan giai thuat settle cho LOAI 2 nhu sau :	
            SoccerMatchService _srv = new SoccerMatchService();
            List<SoccerMatch> _SoccerMatchesList = _srv.SoccerMatches4Settle(false);

            foreach (SoccerMatch _soccermatch in _SoccerMatchesList)
            {
                DateTime localServerDateTime =DateTime.Now;									// lay gio hien tai local server (gio dia phuong of MALTA : GMT +1)							
                DateTime goalserveDateTime = localServerDateTime.AddHours(-1);
                // chuyen sang gio goalserve de so sanh (goalserve su dung GMT +0 cho data trong XML)							
                int dateCompare = DateTime.Compare(goalserveDateTime, (DateTime)_soccermatch.MarketCloseTime);														// so sanh MarketCloseTime cua tran dau voi gio hien tai)		

                if (_soccermatch.Settled == false)					// (mot so tran dau ta da settled = TRUE khi chay LOAI 1 roi, thi khong can xu ly)											
                {
                    // lay ra nhung record nao trong table Soccer_MatchedBets nao thoa nhung dieu kien sau:	
                    SoccerMatchedBetsService _SMSvr = new SoccerMatchedBetsService();
                    List<Soccer_MatchedBets> Soccer_MatchedBetsList = _SMSvr.SoccerMatchedBets(_soccermatch.ID.ToString(), _soccermatch.SportID, _soccermatch.CountryID, _soccermatch.LeagueID);
                    //SELECT * FROM [dbo].[Soccer_MatchedBets] WHERE  [dbo].[Soccer_MatchedBets] .[MatchID] = SoccerMatch.MatchID															
                    //                        AND [dbo].[Soccer_MatchedBets] .LeagueID = SoccerMatch.LeagueID									
                    //                        AND [dbo].[Soccer_MatchedBets].CountryID = SoccerMatch.CountryID									
                    //                        AND [dbo].[Soccer_MatchedBets] .SportID = SoccerMatch.SportID									
                    //                        ORDER BY  [dbo].[Soccer_MatchedBets].MatchedTime DESC									

                    //→ Luu vao variable : Soccer_MatchedBetsList															
                    //Tiep tuc, quet List nay de xet Settle cho tung element trong List.															
                    foreach (Soccer_MatchedBets _smb in Soccer_MatchedBetsList)
                    {
                        //Lay data trong MyBets ra, luu vao 2 bien : 														
                        MyBet BackBet, LayBet;
                        MyBetService _mbSvr = new MyBetService();

                        BackBet = _mbSvr.MyBet(Guid.Parse(_smb.BackBetID), "B", _soccermatch.LeagueID, _soccermatch.CountryID, _soccermatch.SportID);
                        LayBet = _mbSvr.MyBet(Guid.Parse(_smb.LayBetID), "L", _soccermatch.LeagueID, _soccermatch.CountryID, _soccermatch.SportID); ;

                        //BackBet = SELECT * FROM [dbo].[MyBets] WHERE  [dbo].[MyBets].[ID] = Soccer_MatchedBet.BackBetID														
                        //                        AND [dbo].[MyBets] .BL = "B"								
                        //                        AND [dbo].[MyBets] .LeagueID = Soccer_MatchedBet.LeagueID								
                        //                        AND [dbo].[MyBets] .CountryID = Soccer_MatchedBet.CountryID								
                        //                        AND [dbo].[MyBets] .SportID = Soccer_MatchedBet.SportID								

                        //LayBet = SELECT * FROM [dbo].[MyBets] WHERE  [dbo].[MyBets].[ID] = Soccer_MatchedBet.LayBetID														
                        //                        AND [dbo].[MyBets] .BL = "L"								
                        //                        AND [dbo].[MyBets] .LeagueID = Soccer_MatchedBet.LeagueID								
                        //                        AND [dbo].[MyBets] .CountryID = Soccer_MatchedBet.CountryID								
                        //                        AND [dbo].[MyBets] .SportID = Soccer_MatchedBet.SportID								

                        // Tao Settle Object de tien hanh settle														
                        BetSettlement BetSettle = new BetSettlement(_soccermatch, BackBet, LayBet);
                        //Chi can goi ham DoSettle() la ta da tien hanh Settle cho Bet duoc roi !														
                        BetSettle.DoSettle();

                    }  // end Soccer_MatchedBetsList  For loop															
                }
            }   // end SoccerMatchesList For loop !																	

        }

    }
}
