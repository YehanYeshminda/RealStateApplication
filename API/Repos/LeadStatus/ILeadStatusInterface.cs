using API.Models;
using API.Repos.Interfaces;

namespace API.Repos.LeadStatus;

public interface ILeadStatusInterface
{
    List<TblLeadStatus> GetAllLeadStatus();
    TblLeadStatus GetLeadStatusById(int statusId);
}