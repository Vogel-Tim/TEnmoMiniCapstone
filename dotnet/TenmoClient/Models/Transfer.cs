using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace TenmoClient.Models
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


        //public static Transfer CreateTransfer()
        //{
        //    Transfer transfer = new Transfer();
        //    transfer.TypeId = 2;
        //    transfer.StatusId = 2;
        //    transfer.AccountFrom = 2001;
        //    transfer.AccountTo = 2002;
        //    transfer.Amount = 110;

        //    return transfer;
        //}

    }
}
