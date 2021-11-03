using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TenmoServer.Models
{
    public class Account
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public decimal Balance { get; set; }
    }
}
