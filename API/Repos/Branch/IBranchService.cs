using API.Models;

namespace API.Repos.Branch;

public interface IBranchService
{
    Task<Tblbranch?> GetExistingBranchByBranchId(int brId);
}