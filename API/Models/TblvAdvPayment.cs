using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class TblvAdvPayment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? TypeName { get; set; }
        public string? CustName { get; set; }
        public string Address { get; set; } = null!;
        public decimal Chequepaid { get; set; }
        public string Chequeno { get; set; } = null!;
        public decimal Cashpaid { get; set; }
        public decimal Cardpaid { get; set; }
        public string? BankCode { get; set; }
        public string? Propertname { get; set; }
        public string Description { get; set; } = null!;
        public DateTime Addon { get; set; }
        public int Addby { get; set; }
    }
}
