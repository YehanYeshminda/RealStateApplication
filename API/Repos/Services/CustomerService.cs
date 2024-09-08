using API.Models;
using API.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repos.Services
{
    public class CustomerService : ICustomerInterface
    {
        private readonly CRMContext _db;

        public CustomerService(CRMContext context)
        {
            _db = context;
        }
        public async Task<IEnumerable<Tblcustomer>> GetAllCustomerAsync()
        {
            return await _db.Tblcustomers.ToListAsync();
        }

    }
}
