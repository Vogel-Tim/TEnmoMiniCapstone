using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface IAccountSqlDao
    {
        public List<Account> List();

        public Account GetAccount(int id);

        //public bool Delete(int id);
    }
}
