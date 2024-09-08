using API.Models;
using API.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repos.Services
{
    public class ExpenseService : IExpenseInterface
    {
        private readonly CRMContext _db;

        public ExpenseService(CRMContext context)
        {
            _db = context;
        }
        public async Task<IEnumerable<dynamic>> GetAllVExpense()
        {
            return await _db.TblvExpenses.ToListAsync();
        }
        public async Task<IEnumerable<Tblexpense>> GetAllExpense()
        {
            return await _db.Tblexpenses.ToListAsync();
        }

        public async Task<Tblexpense> GetExpenseByIdAsync(string id)
        {
            return await _db.Tblexpenses.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Tblexpensesaccount> GetExpenseaccountByIdAsync(int id)
        {
            return await _db.Tblexpensesaccounts.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Tblemaincat> GetEmaincatByIdAsync(int id)
        {
            return await _db.Tblemaincats.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Tblesubcat> GetEsubcatByIdAsync(int id)
        {
            return await _db.Tblesubcats.FirstOrDefaultAsync(x => x.Id == id);
        }

    }
}
