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

        public Transfer Create(Transfer transfer);

        public bool Send(Transfer transfer);

        public bool Receive(Transfer transfer);
    }
}
