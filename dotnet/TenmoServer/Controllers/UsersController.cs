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
    public class UsersController : ControllerBase
    {

        private readonly IUserDao userDao;

        public UsersController(IUserDao _userDao)
        {
            userDao = _userDao;
        }

        [HttpGet]
        public ActionResult<List<User>> GetUsers()
        {
            List<User> users = userDao.GetUsers();
            if (users.Count != 0)
            {
                return Ok(users);
            }
            else
            {
                return NotFound("No users in the system??");
            }
        }
    }
}
