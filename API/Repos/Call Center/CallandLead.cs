using System.Data;
using API;
using API.Models;
using API.Repos.Call_Center;
using API.Repos.Dtos;
using API.Repos.LeadStatus;
using API.Repos.Notification;
using Microsoft.Data.SqlClient;

public class CallandLead : ICallandLeads
{
    private readonly IConfiguration _configuration;

    public CallandLead(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public List<CallandLeadcountsDto> GetCallsAndLeadsCounts(DateTime startDate, DateTime endDate)
    {
        List<CallandLeadcountsDto> callsAndLeads = new List<CallandLeadcountsDto>();
        DAL dAL = new DAL(_configuration);

        SqlParameter[] parameters = new SqlParameter[]
        {
        new SqlParameter("@StartDate", startDate),
        new SqlParameter("@EndDate", endDate),
        };

        DataTable result = dAL.ExecuteStoredProcedure("StaffPerformance", parameters);

        foreach (DataRow row in result.Rows)
        {
            var callAndLead = new CallandLeadcountsDto
            {
                StaffName = row["StaffName"].ToString(),
                LeadConvertedCount = (int)row["LeadConvertedCount"],
                CallMadeCount = (int)row["CallMadeCount"],
                MeetingsPlanned = (int)row["MeetingsPlanned"]
            };

            callsAndLeads.Add(callAndLead);
        }

        return callsAndLeads;
    }


    public List<CallandLeadcountsDto> GetSingleCallsAndLeadsCounts(DateTime startDate, DateTime endDate, int staffId)
    {
        List<CallandLeadcountsDto> callsAndLeads = new List<CallandLeadcountsDto>();
        DAL dAL = new DAL(_configuration);

        SqlParameter[] parameters = new SqlParameter[]
        {
        new SqlParameter("@StartDate", startDate),
        new SqlParameter("@EndDate", endDate),
        new SqlParameter("@StaffId", staffId),
        };

        DataTable result = dAL.ExecuteStoredProcedure("SingleStaffPerformance", parameters);

        foreach (DataRow row in result.Rows)
        {
            var callAndLead = new CallandLeadcountsDto
            {
                StaffName = row["StaffName"].ToString(),
                LeadConvertedCount = (int)row["LeadConvertedCount"],
                CallMadeCount = (int)row["CallMadeCount"],
                MeetingsPlanned = (int)row["MeetingsPlanned"]
            };

            callsAndLeads.Add(callAndLead);
        }

        return callsAndLeads;
    }

}