using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class TransfersController : ControllerBase
    {

        private static IAccountSqlDao _accountDao;
        private static ITransferSqlDao _transferDao;
        private static IUserDao _userDao;

        public TransfersController(IAccountSqlDao accountSqlDao, ITransferSqlDao transferSqlDao, IUserDao userDao)
        {
            _accountDao = accountSqlDao;
            _transferDao = transferSqlDao;
            _userDao = userDao;
        }

        [HttpPost] //localHost:44315/transfers
        public ActionResult<Transfer> CreateTransfer(Transfer transfer)
        {
            Transfer newTransfer = _transferDao.Create(transfer);
            return Created($"[controller]{newTransfer.Id}", newTransfer);
        }

        [HttpPut] //localHost:44315/transfers
        public ActionResult Transaction(Transfer transfer)
        {
            bool send = _transferDao.Send(transfer);
            bool receive = _transferDao.Receive(transfer);

            if (send && receive)
            {
                return Ok();
            }
            else
            {
                return NotFound("Transfer not found.");
            }
        }

    }
}
