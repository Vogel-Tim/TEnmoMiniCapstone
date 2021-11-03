using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferSqlDao
    {

        public List<Transfer> List(int id);

        public Transfer GetTransfer();

        public bool Create(int typeId, int statusId, int accountFrom, int accountTo, decimal amount);

        //public Transfer Send();

        //public Transfer Recieve();
    }
}
