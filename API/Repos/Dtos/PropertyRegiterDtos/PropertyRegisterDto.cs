namespace API.Repos.Dtos.PropertyRegiterDtos
{
    public class PropertyRegisterDto
    {
        public string Hash { get; set; }
        public string Id { get; set; } = null!;
        public string Propertname { get; set; } = null!;
        public int Type { get; set; }
        public int Category { get; set; }
        public int Subcategory { get; set; }
        public int City { get; set; }
        public string Nationality { get; set; }
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
        public IFormFile MainImg { get; set; }
        public IFormFile? OtherImages0 { get; set; }
        public IFormFile? OtherImages1 { get; set; }
        public IFormFile? OtherImages2 { get; set; }
        public IFormFile? OtherImages3 { get; set; }
        public IFormFile? OtherImages4 { get; set; }
        public IFormFile? OtherImages5 { get; set; }
        public IFormFile? OtherImages6 { get; set; }
        public IFormFile? OtherImages7 { get; set; }
        public IFormFile? OtherImages8 { get; set; }
        public IFormFile? OtherImages9 { get; set; }
        public IFormFile? OtherImages10 { get; set; }
        public IFormFile? OtherImages11 { get; set; }
        public IFormFile? OtherImages12 { get; set; }
        public IFormFile? OtherImages13 { get; set; }
        public IFormFile? OtherImages14 { get; set; }
        public IFormFile? OtherImages15 { get; set; }
        public IFormFile? OtherImages16 { get; set; }
        public IFormFile? OtherImages17 { get; set; }
        public IFormFile? OtherImages18 { get; set; }
        public IFormFile? OtherImages19 { get; set; }
        public IFormFile? OtherImages20 { get; set; }
        public IFormFile? OtherImages21 { get; set; }
        public IFormFile? OtherImages22 { get; set; }
        public IFormFile? OtherImages23 { get; set; }
        public IFormFile? OtherImages24 { get; set; }
        public IFormFile? OtherImages25 { get; set; }
        public IFormFile? OtherImages26 { get; set; }
        public IFormFile? OtherImages27 { get; set; }
        public IFormFile? OtherImages28 { get; set; }
        public IFormFile? OtherImages29 { get; set; }
        public IFormFile? OtherImages30 { get; set; }
        public DateTime Dateofpurchorrent { get; set; }
        public DateTime Renewdate { get; set; }
        public DateTime Venderpaymentdate { get; set; }
        public string Paymentscheduleno { get; set; } = null!;
    }
}
