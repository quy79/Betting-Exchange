using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;
using System.Web.Mvc;

namespace BetEx247.Data.DAL
{
    /// <summary>
    /// Common data in database
    /// </summary>
    public partial interface ICommonService
    {
        //get all country in database
        SelectList getAllCountry();

        /// <summary>
        /// Get list gender
        /// </summary>
        /// <returns>Gender</returns>
        SelectList MakeSelectListGender();

        /// <summary>
        /// Get list Status
        /// </summary>
        /// <returns>Status</returns>
        SelectList MakeSelectListStatus();

        /// <summary>
        /// Get list currency
        /// </summary>
        /// <returns>Currency list</returns>
        SelectList MakeSelectListCurrency();

        /// <summary>
        /// Get list Month
        /// </summary>
        /// <returns>Currency list</returns>
        SelectList MakeSelectListMonth();

        /// <summary>
        /// Get list Year Birth Available
        /// </summary>
        /// <returns>Currency list</returns>
        SelectList MakeSelectListYearBirth();

        /// <summary>
        /// Get list Year Card Available
        /// </summary>
        /// <returns>Currency list</returns>
        SelectList MakeSelectListYearCard();

        /// <summary>
        /// Get list Date search type
        /// </summary>
        /// <returns>Search Date list</returns>
        SelectList MakeSelectListDateSearch();

        /// <summary>
        /// Get Bet Display search 
        /// </summary>
        /// <returns>Bet Display list</returns>
        SelectList MakeSelectListBetDisplay();
    }
}
