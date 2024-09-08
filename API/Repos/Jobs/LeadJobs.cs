using API.Models;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace API.Repos.Jobs;

public class LeadJobs : IJob
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ISchedulerFactory _schedulerFactory;

    public LeadJobs(IServiceProvider serviceProvider, ISchedulerFactory schedulerFactory)
    {
        _serviceProvider = serviceProvider;
        _schedulerFactory = schedulerFactory;
    }
    
    public Task Execute(IJobExecutionContext context)
    {
        context.NextFireTimeUtc.ToString();

        try
        {
            RemoveLeadsCron();
        }
        catch (Exception ex)
        {
            throw;
        }

        return Task.CompletedTask;
    }

    public async Task<string> RemoveLeadsCron()
    {
        try
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo dubaiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time");

                DateTime dubaiTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, dubaiTimeZone);
        
                DateTime currentDate = dubaiTime;
                DateTime startDate = currentDate.Date;
                DateTime endDate = currentDate.Date.AddDays(1);
                
                var crmContext = scope.ServiceProvider.GetRequiredService<CRMContext>();

                var existingLeads = await crmContext.Tblleads
                    .Where(x => x.IsInterested == 0 && x.AddedOn >= startDate && x.AddedOn <= endDate).ToListAsync();
                
                foreach (var lead in existingLeads)
                {
                    lead.Status = 1;
                }
                
                await crmContext.SaveChangesAsync();

                return "";
            }
        }
        catch (Exception ex)
        {
            return "Error occurred while delete and leads! " + ex.Message;
        }
    }
}