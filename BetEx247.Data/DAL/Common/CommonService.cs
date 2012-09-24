using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;
using System.Web.Mvc;
using BetEx247.Core;
using System.Configuration;

namespace BetEx247.Data.DAL
{
    public partial class CommonService : ICommonService
    {
        /// <summary>
        /// get all country in database
        /// </summary>
        /// <returns></returns>
        public List<Country> getAllCountry()
        {
            using (var dba = new BetEXDataContainer())
            {
                var listCountry = dba.Countries.ToList();

                return listCountry;
            }
        }

        /// <summary>
        /// Get list gender
        /// </summary>
        /// <returns>Gender</returns>
        public SelectList MakeSelectListGender()
        {
            List<SelectListItem> Items = new List<SelectListItem>();

            SelectListItem AddMale = new SelectListItem();
            AddMale.Text = "Male";
            AddMale.Value = "M";
            Items.Add(AddMale);

            SelectListItem AddFemale = new SelectListItem();
            AddFemale.Text = "Female";
            AddFemale.Value = "F";
            Items.Add(AddFemale);

            SelectList Res = new SelectList(Items, "Value", "Text");
            return Res;
        }

        /// <summary>
        /// Get list Status
        /// </summary>
        /// <returns>Status</returns>
        public SelectList MakeSelectListStatus()
        {
            List<SelectListItem> Items = new List<SelectListItem>();

            SelectListItem AddActive = new SelectListItem();
            AddActive.Text =Constant.Status.ACTIVE;
            AddActive.Value = Constant.Status.ACTIVE;
            Items.Add(AddActive);

            SelectListItem AddDelete = new SelectListItem();
            AddDelete.Text = Constant.Status.DELETED;
            AddDelete.Value = Constant.Status.DELETED;
            Items.Add(AddDelete);

            SelectList Res = new SelectList(Items, "Value", "Text");
            return Res;
        }

        /// <summary>
        /// Get list currency
        /// </summary>
        /// <returns>Currency list</returns>
        public SelectList MakeSelectListCurrency()
        {
            List<SelectListItem> Items = new List<SelectListItem>();
            string currencyList = ConfigurationManager.AppSettings["CurrencyList"];
            string[] lstCurrency = currencyList.Split(',');
            int i = 0;
            foreach (string item in lstCurrency)
            {
                i++;
                SelectListItem AddItem = new SelectListItem();
                AddItem.Text = item;
                AddItem.Value = i.ToString(); 
                Items.Add(AddItem);
            }                   

            SelectList Res = new SelectList(Items, "Value", "Text");
            return Res;
        }
    }
}
