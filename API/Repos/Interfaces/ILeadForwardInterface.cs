using API.Models;
using API.Repos.Dtos;
using API.Repos.Dtos.CommonDto;
using API.Repos.Dtos.LeadlogDtos;

namespace API.Repos.Interfaces
{
    public interface ILeadForwardInterface
    {
        Task<IEnumerable<TblLeadforward>> GetAllLeadForwards();
        Task<TblLeadforward> GetLeadForwardsById(int id);
        List<BankInfoDto> GetBankInfoByLeadId();
        Task<TblLeadforward> GetLeadForwardsByLeadId(string id);
        Task<GetLeadLogDto> GetLeadForwardWithLog(string id);
        Task<IEnumerable<dynamic>> GetLeadForwardListView();
    }
}
