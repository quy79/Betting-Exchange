using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.DAL.Sports.Interfaces;
using BetEx247.Data.Model;
namespace BetEx247.Data.DAL.Sports
{
     public partial class DataControlStatusService : IDataControlStatusService
    {
         public DataControlStatusService()
         {
         }
        
         public DataControlStatu DataControlStatus(int ID)
         {
             using (var dba = new BetEXDataContainer())
             {
                 DataControlStatu _obj = dba.DataControlStatus.Where(w => w.ID == ID).SingleOrDefault();

                 return _obj;
             }
         }

         public bool Insert(DataControlStatu status)
         {
             using (var dba = new BetEXDataContainer())
             {
                 DataControlStatu _obj = dba.DataControlStatus.Where(w => w.ID == 1).SingleOrDefault();
                 if (_obj == null)
                 {
                     _obj.ID = 1;
                     dba.AddToDataControlStatus(_obj);
                     dba.SaveChanges();
                 }
                 else
                 {
                     Update(status);
                 }
                
                 
                 return true;
             }

             
         }
         public bool Update(DataControlStatu status)
         {
            using (var dba = new BetEXDataContainer())
             {
                 var _obj = dba.DataControlStatus.Where(w => w.ID == 1).SingleOrDefault();
                 if (_obj != null)
                 {

                     _obj.DataObject = _obj.DataObject;
                     
                     dba.SaveChanges();
                     return true;
                 }
             }
             return false;
         }
         public IQueryable<DataControlStatu> Table
         {
             get { throw new NotImplementedException(); }
         }

         public IList<DataControlStatu> GetAll()
         {
             using (var dba = new BetEXDataContainer())
             {
                 var list = dba.DataControlStatus.ToList();

                 return list;
             }
         }


         void IBase<DataControlStatu>.Insert(DataControlStatu entity)
         {
             throw new NotImplementedException();
         }



         public bool Delete(DataControlStatu entity)
         {
             throw new NotImplementedException();
         }
    }
}
