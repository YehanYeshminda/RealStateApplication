using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class Tblbankbranch
    {
        public int Id { get; set; }
        public string? Brid { get; set; }
        public string? Bankid { get; set; }
        public string? BranchName { get; set; }
        public string? Address { get; set; }
        public int Status { get; set; }
        public string? Tel { get; set; }
        public string? ContactPerson { get; set; }
    }
}
