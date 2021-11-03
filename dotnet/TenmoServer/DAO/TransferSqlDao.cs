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

        public Transfer Create()
        {
            throw new NotImplementedException();
        }

    }
}
