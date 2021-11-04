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

        [HttpGet("{id}")]
        public ActionResult<Transfer> GetTransfer(int id)
        {
            Transfer transfer = _transferDao.GetTransfer(id);
            if (transfer.Id != 0)
            {
                return Ok(transfer);
            }
            else
            {
                return NotFound("No transfer exists with provided Id.");
            }
        }

        [HttpGet("account/{userId}")]  //Bring up in scrum meeting. Id param, but don't want in URL
        public ActionResult<List<Transfer>> ListTransfers(int userId)
        {
            string user = User.Identity.Name;
            List<Transfer> transfers = _transferDao.List(userId);
            if (transfers.Count != 0)
            {
                return Ok(transfers);
            }
            else
            {
                return NotFound("No transfers exist for provided account");
            }
        }

        [HttpPost("transfer")] //localHost:44315/transfers/transfer
        public ActionResult<Transfer> CreateTransfer(Transfer transfer)
        {
            Transfer newTransfer = _transferDao.Create(transfer);
            return Created($"[controller]{newTransfer.Id}", newTransfer);
        }

        [HttpPut("transfer")] //localHost:44315/transfers/transfer
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
