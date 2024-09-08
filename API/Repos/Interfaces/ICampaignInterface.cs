using API.Models;
using API.Repos.Dtos;
using API.Repos.Dtos.CampainHDtos;

namespace API.Repos.Interfaces
{
    public interface ICampaignInterface
    {
        Task<IEnumerable<TblCampainH>> GetAllCompaignsAsync();
        Task<TblCampainH> GetCompaignByIdAsync(string no);
        void AddNewCampaignH(TblCampainH CampainHDto);

        Task<TblCampainMedium> GetMediaWithId(int id);

    }
}
