using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;
using BetEx247.Data.DAL.Sports.Interfaces;
namespace BetEx247.Data.DAL.Sports
{
    public partial class MemberService 
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly BetEXDataContainer _context = new BetEXDataContainer();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
       public List<Member> Members()
        {
            using (var dba = new BetEXDataContainer())
            {
                var list = dba.Members.ToList();

                return list;
            }
        }
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
       public Member Member(long ID)
        {
            using (var dba = new BetEXDataContainer())
            {
                var obj = dba.Members.Where(w => w.MemberID == ID).SingleOrDefault();

                return obj;
            }
        }
       
    }
}
