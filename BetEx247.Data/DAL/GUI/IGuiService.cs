﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Core.XMLObjects.Sport.Interface;
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
        List<ISport> GetAllSport();

        /// <summary>
        /// Get Sport
        /// </summary>
        /// <param name="type">Sport type</param>
        /// <returns>Sport</returns>
        ISport GetSport(Constant.SportType type);
        #endregion

        #region database
        /// <summary>
        /// get all sport in sport table
        /// </summary>
        /// <returns>list sport</returns>
        IEnumerable<Sport> GetSportData();
        #endregion
    }
}
