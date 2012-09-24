using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetEx247.Data.Model;

namespace BetEx247.Data.DAL
{
    public interface IBase<T> where T : class,new()
    {            
        IList<T> GetAll();
        void Insert(T entity);
        bool Update(T entity);
        bool Delete(T entity);
        IQueryable<T> Table { get; }
    }
}
