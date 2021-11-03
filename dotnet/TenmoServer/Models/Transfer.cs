using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TenmoServer.Models
{
    public class Transfer
    {
        public int Id { get; set; } 

        [Required(ErrorMessage = "Type ID is a required field for a Transfer.")]
        public int TypeId { get; set; }

        public int StatusId { get; set; }

        public int AccountFrom { get; set; }

        public int AccountTo { get; set; }

        public decimal Amount { get; set; }
    }
}
