using API.Models;
using API.Repos.Dtos;
using API.Repos.Dtos.CampainHDtos;
using API.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repos.Services
{
    public class CompaignService : ICampaignInterface
    {
        private readonly CRMContext _db;

        public CompaignService(CRMContext context)
        {
            _db = context;
        }

        public void AddNewCampaignH(TblCampainH CampainHDto)
        {
            _db.TblCampainHs.AddAsync(CampainHDto);
        }

        public async Task<IEnumerable<TblCampainH>> GetAllCompaignsAsync()
        {
            return await _db.TblCampainHs.ToListAsync();
        }

        public async Task<TblCampainH> GetCompaignByIdAsync(string no)
        {
            return await _db.TblCampainHs.FirstOrDefaultAsync(x => x.No == no);
        }

        public async Task<TblCampainMedium> GetMediaWithId(int id)
        {
            return await _db.TblCampainMedia.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
