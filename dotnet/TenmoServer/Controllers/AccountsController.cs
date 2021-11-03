﻿using Microsoft.AspNetCore.Http;
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

        [HttpGet("{id}")]
        public ActionResult<Account> GetAccountBalance(int id)
        {
            Account account = _accountDao.GetAccount(id);
            if(account != null)
            {
                return Ok(account);
            }
            else
            {
                return NotFound();
            }
        }



    }
}