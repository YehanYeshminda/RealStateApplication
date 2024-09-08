﻿using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class Tblpropertyregister
    {
        public string Id { get; set; } = null!;
        public string Propertname { get; set; } = null!;
        public int Type { get; set; }
        public int Category { get; set; }
        public int Subcategory { get; set; }
        public int City { get; set; }
        public string Nationality { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Geolocation { get; set; } = null!;
        public int Vender { get; set; }
        public decimal Costanually { get; set; }
        public decimal Othercost { get; set; }
        public string Rulesregulations { get; set; } = null!;
        public int Status { get; set; }
        public decimal Sellingprice { get; set; }
        public decimal Minsellingprice { get; set; }
        public decimal Anualcostforbuyer { get; set; }
        public decimal Deposit { get; set; }
        public int Contacttype { get; set; }
        public int Socialmedia { get; set; }
        public string Mainimg { get; set; } = null!;
        public string Otherimg { get; set; } = null!;
        public DateTime Dateofpurchorrent { get; set; }
        public DateTime Renewdate { get; set; }
        public DateTime Venderpaymentdate { get; set; }
        public string Paymentscheduleno { get; set; } = null!;
        public DateTime Addon { get; set; }
        public int Addby { get; set; }
    }
}
