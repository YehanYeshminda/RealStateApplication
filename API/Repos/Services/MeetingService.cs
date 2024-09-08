using API.Models;
using API.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repos.Services
{
    public class MeetingService : IMeetingInterface
    {
        private readonly CRMContext _db;

        public MeetingService(CRMContext context)
        {
            _db = context;
        }


        public async Task<TblMeeting> GetMeetingByIdAsync(int id)
        {
            return await _db.TblMeetings.FirstOrDefaultAsync(x => x.Id == id);
        }


        public async Task<IEnumerable<TblMeeting>> GetAllMeeting()
        {
            return await _db.TblMeetings.ToListAsync();
        }

        public async Task<IEnumerable<TblMeeting>> GetAllMeetingByUserId(int staffId)
        {
            return await _db.TblMeetings.Where(x => x.Staffid == staffId).ToListAsync();
        }

        public async Task<IEnumerable<dynamic>> GetAllVMeeting()
        {
            return await _db.TblVmeetings.ToListAsync();
        }
    }
}
