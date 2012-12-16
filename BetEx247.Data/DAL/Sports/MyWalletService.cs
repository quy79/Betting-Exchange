using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.DAL.Sports.Interfaces;
using BetEx247.Data.Model;
namespace BetEx247.Data.DAL.Sports
{
     public partial class MyWalletService 
    {
         public MyWalletService()
         {
         }
         public List<MyWallet> MyWallets()
         {
             using (var dba = new BetEXDataContainer())
             {
                 var list = dba.MyWallets.ToList();

                 return list;
             }
         }
         public MyWallet MyWallet(long ID)
         {
             using (var dba = new BetEXDataContainer())
             {
                 MyWallet _sport = dba.MyWallets.Where(w => w.ID == ID).SingleOrDefault();

                 return _sport;
             }
         }

         public MyWallet MyWalletbyMemberId(long MemberID)
         {
             using (var dba = new BetEXDataContainer())
             {
                 MyWallet _sport = dba.MyWallets.Where(w => w.MemberID == MemberID).SingleOrDefault();

                 return _sport;
             }
         }
         public Sport Sport(String name)
         {
             using (var dba = new BetEXDataContainer())
             {
                 Sport _sport = dba.Sports.Where(w => w.SportName == name).SingleOrDefault();

                 return _sport;
             }
         }
         public bool Delete(Sport sport)
         {
             using (var dba = new BetEXDataContainer())
             {

                 dba.DeleteObject(sport);
                 return true;
             }
         }
       
         public bool Update(MyWallet wallet)
         {
            using (var dba = new BetEXDataContainer())
             {
                 var _sport = dba.MyWallets.Where(w => w.ID == wallet.ID).SingleOrDefault();
                 if (_sport != null)
                 {

                     _sport.Available = wallet.Available;
                     _sport.Balance = wallet.Balance;
                     _sport.Exposure = wallet.Exposure;
                     _sport.UpdatedTime = DateTime.Now;
                     
                     dba.SaveChanges();
                     return true;
                 }
             }
             return false;
         }
        

         
    }
}
