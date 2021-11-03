using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class AccountSqlDao : IAccountSqlDao
    {

        private readonly string ConnectionString;

        public AccountSqlDao(string connString)
        {
            ConnectionString = connString;
        }

        public List<Account> List()
        {
            throw new NotImplementedException();
        }

        public Account GetAccount()
        {
            throw new NotImplementedException();
        }

        public Account UpdateAccount()
        {
            throw new NotImplementedException();
        }

        public Account Create()
        {
            throw new NotImplementedException();
        }

        public bool Delete()
        {
            throw new NotImplementedException();
        }

    }
}
