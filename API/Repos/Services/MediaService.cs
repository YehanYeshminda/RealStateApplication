using API.Models;
using API.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repos.Services
{
    public class MediaService : IMediaInterface
    {
        private readonly CRMContext _db;

        public MediaService(CRMContext context)
        {
            _db = context;
        }
        public async Task<IEnumerable<TblMedium>> GetAllMedia()
        {
            return await _db.TblMedia.ToListAsync();
        }

        public async Task<TblMediaLink> GetMedialinkWithId(int id)
        {
            return await _db.TblMediaLinks.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<TblMedium> GetMediaWithId(int id)
        {
            return await _db.TblMedia.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
