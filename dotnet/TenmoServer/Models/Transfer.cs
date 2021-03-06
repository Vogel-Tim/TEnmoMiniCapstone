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

        [Required(ErrorMessage = "Status ID is a required field for a Transfer.")]
        public int StatusId { get; set; }

        [Required(ErrorMessage = "The account transferred to is a required field for a Transfer.")]
        public int AccountFrom { get; set; }

        [Required(ErrorMessage = "The account transferred from is a required field for a Transfer.")]
        public int AccountTo { get; set; }

        [Required(ErrorMessage = "The amount to transfer is a required field for a Transfer.")]
        public decimal Amount { get; set; }
    }
}
