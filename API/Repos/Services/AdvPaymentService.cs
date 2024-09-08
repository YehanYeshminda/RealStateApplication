using API.Models;
using API.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repos.Services
{
    public class AdvPaymentService : IAdvPaymentInterface
    {
        private readonly CRMContext _db;

        public AdvPaymentService(CRMContext context)
        {
            _db = context;
        }

        public async Task<IEnumerable<TblAdvPayment>> GetAllAdv()
        {
            return await _db.TblAdvPayments.ToListAsync();
        }

        public async Task<IEnumerable<dynamic>> GetViewAllAdv()
        {
            return await _db.TblvAdvPayments.ToListAsync();
        }
    }
}
