using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repos.Branch;

public class BranchService : IBranchService
{
    private readonly CRMContext _db;

    public BranchService(CRMContext db)
    {
        _db = db;
    }
    
    public async Task<Tblbranch?> GetExistingBranchByBranchId(int brId)
    {
        return await _db.Tblbranches.FirstOrDefaultAsync(x => x.BrId == brId);
    }
}