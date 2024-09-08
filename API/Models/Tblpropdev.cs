using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class Tblpropdev
    {
        public string Id { get; set; } = null!;
        public DateTime Date { get; set; }
        public string Propname { get; set; } = null!;
        public int Vender { get; set; }
        public string Propertyno { get; set; } = null!;
        public int Expenseaccount { get; set; }
        public string Description { get; set; } = null!;
        public decimal Amount { get; set; }
        public decimal Cashpaid { get; set; }
        public decimal Banktransfer { get; set; }
        public int Bankid { get; set; }
        public decimal Chequepaid { get; set; }
        public int Chequeid { get; set; }
        public int Approvedby { get; set; }
        public DateTime Addon { get; set; }
        public int Addby { get; set; }
    }
}
