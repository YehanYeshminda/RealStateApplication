using API.Models;
using API.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repos.Services;

public class ArchviedLeadService : IArchviedLeadInterface
{
    private readonly CRMContext _context;

    public ArchviedLeadService(CRMContext context)
    {
        _context = context;
    }
    
    public async Task<List<Tbllead>> GetAllArchivedLeads()
    {
        return await _context.Tblleads.Where(x => x.Status == 1).ToListAsync();
    }

    public async Task<List<Tbllead>> GetAllArchivedLeadsDependingOnDate(DateTime startTime, DateTime endTime)
    {
        return await _context.Tblleads.Where(x => x.Status == 1 && x.AddedOn >= startTime && x.AddedOn <= endTime)
            .ToListAsync();
    }

    public async Task<Tbllead> GetAllArchivedLeadsById(string leadNo)
    {
        return await _context.Tblleads.FirstOrDefaultAsync(x => x.Leadno == leadNo);
    }
}