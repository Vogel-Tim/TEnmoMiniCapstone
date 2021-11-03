using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class TransferSqlDao : ITransferSqlDao
    {

        private readonly string ConnectionString;

        public TransferSqlDao(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public List<Transfer> List()
        {
            throw new NotImplementedException();
        }

        public Transfer GetTransfer()
        {
            throw new NotImplementedException();
        }

        //public Account UpdateAccountBalance(int id)
        //{
        //    try
        //    {
        //        using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
        //        {

        //            sqlConn.Open();
        //            string updateStatement = "UPDATE accounts SET balance = "
        //        }
        //    }
        //    catch (SqlException)
        //    {

        //        throw new NotImplementedException();
        //    }
        //}

        public Transfer Create()
        {
            throw new NotImplementedException();
        }

    }
}
