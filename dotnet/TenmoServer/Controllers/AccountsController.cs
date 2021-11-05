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
    public class AccountsController : ControllerBase
    {
        private static IAccountSqlDao _accountDao;
        private static ITransferSqlDao _transferDao;
        private static IUserDao _userDao;

        public AccountsController(IAccountSqlDao accountSqlDao, ITransferSqlDao transferSqlDao, IUserDao userDao)
        {
            _accountDao = accountSqlDao;
            _transferDao = transferSqlDao;
            _userDao = userDao;
        }

        [HttpGet("{userId}")]
        public ActionResult<Account> GetAccount(int userId)
        {
            Account account = _accountDao.GetAccount(userId, 0);
            if(account != null)
            {
                return Ok(account);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("account/{accountId}")]
        public ActionResult<User> GetUserByAccount(int accountId)
        {
            User user = _userDao.GetUser(accountId);
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return NotFound();
            }
        }

    }
}
