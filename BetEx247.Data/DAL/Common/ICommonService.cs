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
        List<Country> getAllCountry();

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
    }
}
