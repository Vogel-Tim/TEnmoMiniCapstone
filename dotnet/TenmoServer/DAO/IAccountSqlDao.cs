using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    interface IAccountSqlDao
    {
        public List<Account> List();

        public Account GetAccount();

        public Account UpdateAccount();

        public Account Create();

        public bool Delete();
    }
}
