using System.Data;
using API.Models;
using Microsoft.Data.SqlClient;

namespace API.Repos.LeadStatus;

public class LeadStatusNewService : ILeadStatusInterface
{
    private readonly IConfiguration _configuration;

    public LeadStatusNewService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public List<TblLeadStatus> GetAllLeadStatus()
    {
        List<TblLeadStatus> branches = new List<TblLeadStatus>();

        DAL dAL = new DAL(_configuration);
        SqlParameter[] parameters = new SqlParameter[]
        {
        };
        
        DataTable result = dAL.ExecuteStoredProcedure("GetAllLeadStatus", parameters);

        foreach (DataRow item in result.Rows)
        {
            TblLeadStatus branch = new TblLeadStatus
            {
                Id = Convert.ToInt32(item["Id"]),
                Leadstatus = item["Leadstatus"].ToString(),
                Status = Convert.ToInt32(item["Status"]),
                Remark = item["remark"].ToString(),
            };
            
            branches.Add(branch);
        }
            
        return branches;
    }

    public TblLeadStatus GetLeadStatusById(int statusId)
    {
        TblLeadStatus user = new TblLeadStatus();
        DAL dAL = new DAL(_configuration);
        SqlParameter[] sQlParameters = new SqlParameter[]
        {
            new SqlParameter("@StatusId", statusId)
        };
        
        DataTable result = dAL.ExecuteStoredProcedure("GetLeadStatusById", sQlParameters);

        if (result.Rows.Count > 0)
        {
            DataRow item = result.Rows[0];
            user.Id = item["Id"] != DBNull.Value ? Convert.ToInt32(item["Id"]) : 0;
            user.Leadstatus = item["Leadstatus"] != DBNull.Value ? Convert.ToString(item["Leadstatus"]) : "";
            user.Status = item["Status"] != DBNull.Value ? Convert.ToInt32(item["Status"]) : 0;
            user.Remark = item["Remark"] != DBNull.Value ? Convert.ToString(item["Remark"]) : "";
        }

        return user;
    }
}