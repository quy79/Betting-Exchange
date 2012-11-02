using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using BetEx247.Core.Common.Utils;

namespace BetEx247.Core.Infrastructure
{
    public class DataFactory
    {
        Database database;

        public class Parameter        
        {
            public string Name { get; set; }
            public DbType Type { get; set; }
            public object Value { get; set; }
            public int Size { get; set; }
            public Parameter() { }
            public Parameter(string name, DbType type, object value)
            {
                this.Name = name;
                this.Type = type;
                this.Value = value;
            }
            public Parameter(string name, DbType type, object value, int size)
            {
                this.Name = name;
                this.Type = type;
                this.Value = value;
                this.Size = size;
            }
        }

        public DataFactory()
        {
            database = DatabaseFactory.CreateDatabase("BetEX_DataBase");
        }
        public DataFactory(string ConnectionStringName)
        {
            database = DatabaseFactory.CreateDatabase(ConnectionStringName);
        }

        public DataTable RetrieveData(string query)
        {
            DataTable dt = new DataTable();
            try
            {
                using (IDataReader dr = database.ExecuteReader(CommandType.Text, query))
                {
                    if (dr != null)
                    {
                        dt.Load(dr);
                    }
                }
            }
            catch (Exception ex)
            { throw new Exception("Access data layer -> RetrieveData", ex); }
            return dt;
        }

        public DataTable RetrieveData(string storeProcedureName, Parameter[] InParams)
        {
            DataTable dt = new DataTable();
            try
            {
                DbCommand cmd;
                cmd = database.GetStoredProcCommand(storeProcedureName);
                foreach (Parameter prm in InParams)
                {
                    database.AddInParameter(cmd, prm.Name, prm.Type, prm.Value);
                }
                using (IDataReader dr = database.ExecuteReader(cmd))
                {
                    if (dr != null)
                    {
                        dt.Load(dr);
                    }
                }
            }
            catch (Exception ex)
            { throw new Exception("Access data layer -> RetrieveData", ex); }
            return dt;
        }

        public DataTable RetrieveData(string storeProcedureName, Parameter[] InParams, ref Dictionary<string, Parameter> OutParams)
        {
            DataTable dt = new DataTable();
            try
            {
                DbCommand cmd;
                cmd = database.GetStoredProcCommand(storeProcedureName);
                foreach (Parameter prm in InParams)
                {
                    database.AddInParameter(cmd, prm.Name, prm.Type, prm.Value);
                }
                foreach (KeyValuePair<string, Parameter> prm in OutParams)
                {
                    database.AddOutParameter(cmd, prm.Value.Name, prm.Value.Type, prm.Value.Size);
                }
                using (IDataReader dr = database.ExecuteReader(cmd))
                {
                    if (dr != null)
                    {
                        dt.Load(dr);
                    }
                    foreach (KeyValuePair<string, Parameter> prm in OutParams)
                    {
                        prm.Value.Value = database.GetParameterValue(cmd, prm.Value.Name);
                    }
                }
            }
            catch (Exception ex)
            { throw new Exception("Access data layer -> RetrieveData", ex); }
            return dt;
        }

        public int Execute(string query)
        {
            return Execute(query, CommandType.Text);
        }

        public int Execute(string storeProcedureName, Parameter[] InParams)
        {
            int ret = 0;
            try
            {
                DbCommand cmd;
                cmd = database.GetStoredProcCommand(storeProcedureName);
                foreach (Parameter prm in InParams)
                {
                    database.AddInParameter(cmd, prm.Name, prm.Type, prm.Value);
                }
                ret = database.ExecuteNonQuery(cmd);
                
            }
            catch (Exception ex)
            { throw new Exception("Access data layer -> Execute", ex); }
            return ret;
        }

        public int Execute(string storeProcedureName, Parameter[] InParams, ref Dictionary<string, Parameter> OutParams)
        {
            int ret = 0;
            try
            {
                DbCommand cmd;
                cmd = database.GetStoredProcCommand(storeProcedureName);
                foreach (Parameter prm in InParams)
                {
                    if (prm.Type == DbType.String && prm.Size != 0 && prm.Size < prm.Value.ToString().Length)
                    {
                        prm.Value = prm.Value.ToString().Substring(0, prm.Size);
                    }
                    database.AddInParameter(cmd, prm.Name, prm.Type, prm.Value);
                }
                foreach (KeyValuePair<string, Parameter> prm in OutParams)
                {
                    database.AddOutParameter(cmd, prm.Value.Name, prm.Value.Type, prm.Value.Size);
                }
                ret = database.ExecuteNonQuery(cmd);
                foreach (KeyValuePair<string, Parameter> prm in OutParams)
                {
                    prm.Value.Value = database.GetParameterValue(cmd, prm.Value.Name);
                }

            }
            catch (Exception ex)
            { throw new Exception("Access data layer -> Execute", ex); }
            return ret;
        }

        public IDataReader ExecuteIReader(string storeProcedureName, Parameter[] InParams)
        {
            IDataReader dr;
            try
            {
                DbCommand cmd;
                cmd = database.GetStoredProcCommand(storeProcedureName);
                foreach (Parameter prm in InParams)
                {
                    database.AddInParameter(cmd, prm.Name, prm.Type, prm.Value);
                }
                dr = database.ExecuteReader(cmd);

            }
            catch (Exception ex)
            {
                throw new Exception("Access data layer -> ExecuteIReader", ex);
            }
            return dr;
        }
        public IDataReader ExecuteIReader(string query)
        {
            IDataReader dr;
            try
            {
                dr = database.ExecuteReader(CommandType.Text, query);

            }
            catch (Exception ex)
            {
                throw new Exception("Access data layer -> ExecuteIReader", ex);
            }
            return dr;
        }

        public DataTable ExecuteReader(string query)
        {
            return ExecuteReader(query, CommandType.Text);
        }


        public DataTable ExecuteReader(string storeProcedureName, Parameter[] InParams)
        {
            DataTable dt = new DataTable();
            try
            {
                DbCommand cmd;
                cmd = database.GetStoredProcCommand(storeProcedureName);
                foreach (Parameter prm in InParams)
                {
                    database.AddInParameter(cmd, prm.Name, prm.Type, prm.Value);
                }
                using (IDataReader dr = database.ExecuteReader(cmd))
                {
                    if (dr != null)
                    {
                        dt.Load(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Access data layer -> ExecuteReader", ex);
            }
            return dt;
        }

        public DataTable ExecuteReader(string storeProcedureName, Parameter[] InParams, ref Dictionary<string, Parameter> OutParams)
        {
            DataTable dt = new DataTable();
            try
            {
                DbCommand cmd;
                cmd = database.GetStoredProcCommand(storeProcedureName);
                foreach (Parameter prm in InParams)
                {
                    database.AddInParameter(cmd, prm.Name, prm.Type, prm.Value);
                }
                foreach (KeyValuePair<string,Parameter> prm in OutParams)
                {
                    database.AddOutParameter(cmd, prm.Value.Name, prm.Value.Type, prm.Value.Size);
                }
                using (IDataReader dr = database.ExecuteReader(cmd))
                {
                    if (dr != null)
                    {
                        dt.Load(dr);
                        foreach (KeyValuePair<string, Parameter> prm in OutParams)
                        {
                            prm.Value.Value = database.GetParameterValue(cmd, prm.Value.Name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Access data layer -> ExecuteReader", ex);
            }
            return dt;
        }

        public DataSet ExecuteDataset(string storeProcedureName, Parameter[] InParams)
        {
            DataSet ds;
            try
            {
                DbCommand cmd;
                cmd = database.GetStoredProcCommand(storeProcedureName);
                foreach (Parameter prm in InParams)
                {
                    database.AddInParameter(cmd, prm.Name, prm.Type, prm.Value);
                }
                ds = database.ExecuteDataSet(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Access data layer -> ExecuteReader", ex);
            }
            return ds;
        }

        public object ExecuteScalar(string storeProcedureName, Parameter[] InParams)
        {
            object ret = 0;
            try
            {
                DbCommand cmd;
                cmd = database.GetStoredProcCommand(storeProcedureName);
                foreach (Parameter prm in InParams)
                {
                    if (prm.Type == DbType.String && prm.Size != 0 && prm.Size < prm.Value.ToString().Length)
                    {
                        prm.Value = prm.Value.ToString().Substring(0, prm.Size);
                    }
                    database.AddInParameter(cmd, prm.Name, prm.Type, prm.Value);
                }
                ret = database.ExecuteScalar(cmd);
            }
            catch (Exception ex)
            { throw new Exception("Access data layer -> Execute", ex); }
            return ret;
        }

        public object ExecuteScalar(string storeProcedureName, Parameter[] InParams, ref Dictionary<string, Parameter> OutParams)
        {
            object ret = 0;
            try
            {
                DbCommand cmd;
                cmd = database.GetStoredProcCommand(storeProcedureName);
                foreach (Parameter prm in InParams)
                {
                    if (prm.Type == DbType.String && prm.Size != 0 && prm.Size < prm.Value.ToString().Length)
                    {
                        prm.Value = prm.Value.ToString().Substring(0, prm.Size);
                    }
                    database.AddInParameter(cmd, prm.Name, prm.Type, prm.Value);
                }
                foreach (KeyValuePair<string, Parameter> prm in OutParams)
                {
                    database.AddOutParameter(cmd, prm.Value.Name, prm.Value.Type, prm.Value.Size);
                }
                ret = database.ExecuteScalar(cmd);
                foreach (KeyValuePair<string, Parameter> prm in OutParams)
                {
                    prm.Value.Value = database.GetParameterValue(cmd, prm.Value.Name);
                }

            }
            catch (Exception ex)
            { throw new Exception("Access data layer -> Execute", ex); }
            return ret;
        }

        #region Extended
        public int Execute(string queryName, CommandType cType)
        {
            int ret = 0;
            try
            {
                ret = database.ExecuteNonQuery(cType, queryName);
            }
            catch (Exception ex)
            { throw new Exception("Access data layer -> Execute", ex); }
            return ret;
        }

        public int Execute(string storeProcedureName, params object[] pars)
        {
            int ret = 0;
            try
            {
                DbCommand cmd = database.GetStoredProcCommand(storeProcedureName);
                for (int i = 0; i < pars.Length; i += 2)
                {
                    DbType t = pars[i + 1] != null ? TypeConvertor.ToDbType(pars[i + 1].GetType()) : DbType.Object;
                    database.AddInParameter(cmd, pars[i].ToString(), t, pars[i + 1]);
                }
                ret = database.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            { throw new Exception("Access data layer -> Execute", ex); }
            return ret;
        }

        public int Execute(string storeProcedureName, ref Dictionary<string, object> OutParams, params object[] pars)
        {
            int ret = 0;
            try
            {
                DbCommand cmd = database.GetStoredProcCommand(storeProcedureName);
                for (int i = 0; i < pars.Length; i += 2)
                {
                    DbType t = pars[i + 1] != null ? TypeConvertor.ToDbType(pars[i + 1].GetType()) : DbType.Object;
                    database.AddInParameter(cmd, pars[i].ToString(), t, pars[i + 1]);
                }
                foreach (KeyValuePair<string, object> prm in OutParams)
                {
                    DbType t = prm.Value != null ? TypeConvertor.ToDbType(prm.Value.GetType()) : DbType.Object;
                    database.AddOutParameter(cmd, prm.Key, t, int.Parse(prm.Value.ToString()));
                }
                ret = database.ExecuteNonQuery(cmd);
                for (int i = 0; i < OutParams.Count; i++)
                {
                    OutParams[OutParams.ElementAt(i).Key] = database.GetParameterValue(cmd, OutParams.ElementAt(i).Key);
                }
            }
            catch (Exception ex)
            { throw new Exception("Access data layer -> Execute", ex); }
            return ret;
        }

        public DataTable ExecuteReader(string query, CommandType ctype)
        {
            DataTable dt = new DataTable();
            try
            {
                using (IDataReader dr = database.ExecuteReader(ctype, query))
                {
                    if (dr != null)
                    {
                        dt.Load(dr);
                    }
                }
            }
            catch (Exception ex)
            { throw new Exception("Access data layer -> ExecuteReader", ex); }
            return dt;
        }

        public IDataReader ExecuteIReader(string storeProcedureName, params object[] pars)
        {
            IDataReader dr;
            try
            {
                DbCommand cmd = database.GetStoredProcCommand(storeProcedureName);
                for (int i = 0; i < pars.Length; i += 2)
                {
                    DbType t = pars[i + 1] != null ? TypeConvertor.ToDbType(pars[i + 1].GetType()) : DbType.Object;
                    database.AddInParameter(cmd, pars[i].ToString(), t, pars[i + 1]);
                }

                dr = database.ExecuteReader(cmd);
            }
            catch (Exception ex)
            { throw new Exception("Access data layer -> ExecuteReader", ex); }
            return dr;
        }

        public DataTable ExecuteReader(string storeProcedureName, params object[] pars)
        {
            DataTable dt = new DataTable();
            try
            {
                DbCommand cmd = database.GetStoredProcCommand(storeProcedureName);
                for (int i = 0; i < pars.Length; i += 2)
                {
                    DbType t = pars[i + 1] != null ? TypeConvertor.ToDbType(pars[i + 1].GetType()) : DbType.Object;
                    database.AddInParameter(cmd, pars[i].ToString(), t, pars[i + 1]);
                }
                using (IDataReader dr = database.ExecuteReader(cmd))
                {
                    if (dr != null)
                    {
                        dt.Load(dr);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            { throw new Exception("Access data layer -> ExecuteReader", ex); }
            return dt;
        }

        public DataTable ExecuteReader(string storeProcedureName, ref Dictionary<string, object> OutParams, params object[] pars)
        {
            DataTable dt = new DataTable();
            try
            {
                DbCommand cmd = database.GetStoredProcCommand(storeProcedureName);
                for (int i = 0; i < pars.Length; i += 2)
                {
                    DbType t = pars[i + 1] != null ? TypeConvertor.ToDbType(pars[i + 1].GetType()) : DbType.Object;
                    database.AddInParameter(cmd, pars[i].ToString(), t, pars[i + 1]);
                }
                foreach (KeyValuePair<string, object> prm in OutParams)
                {
                    DbType t = prm.Value != null ? TypeConvertor.ToDbType(prm.Value.GetType()) : DbType.Object;
                    database.AddOutParameter(cmd, prm.Key, t, int.Parse(prm.Value.ToString()));
                }
                using (IDataReader dr = database.ExecuteReader(cmd))
                {
                    if (dr != null)
                    {
                        dt.Load(dr);
                    }
                    dr.Close();
                }
                for (int i = 0; i < OutParams.Count; i++)
                {
                    OutParams[OutParams.ElementAt(i).Key] = database.GetParameterValue(cmd, OutParams.ElementAt(i).Key);
                }
            }
            catch (Exception ex)
            { throw new Exception("Access data layer -> ExecuteReader", ex); }
            return dt;
        }

        public DataSet ExecuteDataset(string storeProcedureName, params object[] pars)
        {
            DataSet ds;
            try
            {
                DbCommand cmd = database.GetStoredProcCommand(storeProcedureName);
                for (int i = 0; i < pars.Length; i += 2)
                {
                    DbType t = pars[i + 1] != null ? TypeConvertor.ToDbType(pars[i + 1].GetType()) : DbType.Object;
                    database.AddInParameter(cmd, pars[i].ToString(), t, pars[i + 1]);
                }
                ds = database.ExecuteDataSet(cmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Access data layer -> ExecuteReader", ex);
            }
            return ds;
        }

        public IDataReader RetrieveReader(string query)
        {
            try
            {
                return database.ExecuteReader(CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw new Exception("Access data layer -> ExecuteReader", ex);
            }
        }

        public IDataReader RetrieveReader(string storeProcedureName, params object[] pars)
        {
            try
            {
                DbCommand cmd = database.GetStoredProcCommand(storeProcedureName);
                for (int i = 0; i < pars.Length; i += 2)
                {
                    DbType t = pars[i + 1] != null ? TypeConvertor.ToDbType(pars[i + 1].GetType()) : DbType.Object;
                    database.AddInParameter(cmd, pars[i].ToString(), t, pars[i + 1]);
                }
                return database.ExecuteReader(cmd);
            }
            catch (Exception ex)
            { throw new Exception("Access data layer -> ExecuteReader", ex); }
        }
        #endregion
    }
}
