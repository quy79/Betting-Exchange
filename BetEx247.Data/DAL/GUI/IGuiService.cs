﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core;
using BetEx247.Data.Model;

namespace BetEx247.Data.DAL
{
    public partial interface IGuiService
    {
        #region xml 
        /// <summary>
        /// get all sport in system
        /// </summary>
        /// <returns>list sport</returns>
        List<Sport> GetAllSport();

        /// <summary>
        /// Get Sport
        /// </summary>
        /// <param name="type">Sport type</param>
        /// <returns>Sport</returns>
        Sport GetSport(Constant.SportType type);
        #endregion

        #region database
        /// <summary>
        /// get all sport in sport table
        /// </summary>
        /// <returns>list sport</returns>
        IEnumerable<Sport> GetSportData();

        List<SoccerMatch> LiveInMatches(bool isSoccer);

        List<PSV_MATCHES> UpCommingMatches(bool isSoccer, long? leagueId, int? countryId, int? sportId, int day);

        SoccerCountry GetCountryByLeage(long leagueId,int countryId,int sportId);

        SoccerCountry GetCountryByCountry(int countryId);

        List<PSV_ALLTOURNAMENT> GetTournamentByCountry(int countryId);

        /// <summary>
        /// Get top league set by admin
        /// </summary>
        /// <returns>list psv_top_event</returns>
        List<PSV_TOP_EVENT> GetTopEvent();

        SoccerLeague GetSoccerLeague(long leagueId, int countryId, int sportId);
        #endregion
    }
}
