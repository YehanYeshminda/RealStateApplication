using API.Models;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace API.Repos.Jobs;

public class CallStatusResetJob : IJob
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ISchedulerFactory _schedulerFactory;

    public CallStatusResetJob(IServiceProvider serviceProvider, ISchedulerFactory schedulerFactory)
    {
        _serviceProvider = serviceProvider;
        _schedulerFactory = schedulerFactory;
    }
    
    public Task Execute(IJobExecutionContext context)
    {
        context.NextFireTimeUtc.ToString();

        try
        {
            CallStatusReset();
        }
        catch (Exception ex)
        {
            throw;
        }

        return Task.CompletedTask;
    }

    public async Task<string> CallStatusReset()
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

                var existingCalls = await crmContext.TblCallInsights
                    .Where(x => x.AssignedTo != "0" && x.CalledOn == null && x.CallEndedOn == null  && x.AssignedOn >= startDate && x.AssignedOn <= endDate).ToListAsync();
                
                foreach (var lead in existingCalls)
                {
                    lead.AssignedTo = "0";
                    lead.Status = 0;
                    lead.AssignedOn = null;
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