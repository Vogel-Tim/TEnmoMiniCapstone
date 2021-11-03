using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    interface ITransferSqlDao
    {

        public List<Transfer> List();

        public Transfer GetTransfer();

        public Transfer Create();
    }
}
