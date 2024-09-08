using API.Models;
using API.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repos.Services
{
    public class ContactMethodService : IContactMethodInterface
    {
        private readonly CRMContext _db;

        public ContactMethodService(CRMContext context)
        {
            _db = context;
        }
        public async Task<TblPreferedContactMethod> GetContactMethod(int id)
        {
            return await _db.TblPreferedContactMethods.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
