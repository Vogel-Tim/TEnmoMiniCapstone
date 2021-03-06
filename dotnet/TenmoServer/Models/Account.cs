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

        [Required(ErrorMessage = "User ID is a required field for an account.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Balance is a required field for an account.")]
        public decimal Balance { get; set; }
    }
}
