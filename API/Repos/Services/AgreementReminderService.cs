using API.Models;
using API.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repos.Services
{
    public class AgreementReminderService : IAgreementReminderInterface
    {
        private readonly CRMContext _db;

        public AgreementReminderService(CRMContext context)
        {
            _db = context;
        }

        public async Task<IEnumerable<TblAgreementReminder>> GetAllAgreementReminder()
        {
            return await _db.TblAgreementReminders.ToListAsync();
        }

        public async Task<IEnumerable<dynamic>> GetAllAgreementReminderView()
        {
            return await _db.TblvAgreementReminders.ToListAsync();
        }
    }
}
