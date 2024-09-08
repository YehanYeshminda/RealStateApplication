using System.Data;
using API.Models;
using Microsoft.Data.SqlClient;

namespace API.Repos.Control;

public class ControlService : IControlInterface
{
    private readonly IConfiguration _configuration;

    public ControlService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public List<Tblcontrol> GetAllControls()
    {
        List<Tblcontrol> branches = new List<Tblcontrol>();

        DAL dAL = new DAL(_configuration);
        SqlParameter[] parameters = new SqlParameter[]
        {

        };
        
        DataTable result = dAL.ExecuteStoredProcedure("GetAllTblControls", parameters);

        foreach (DataRow item in result.Rows)
        {
            Tblcontrol branch = new Tblcontrol
            {
                Cid = Convert.ToInt32(item["cid"]),
                Grnid = Convert.ToInt32(item["Grnid"]),
                PurchaseReturnId = Convert.ToInt32(item["PurchaseReturnID"]),
                InvoiceNo = Convert.ToInt32(item["InvoiceNo"]),
                PoNo = Convert.ToInt32(item["PoNo"]),
                AdvrptNo = Convert.ToInt32(item["ADVRptNo"]),
                CashMovement = Convert.ToInt32(item["CashMovement"]),
                IssueNoteNo = Convert.ToInt32(item["IssueNoteNo"]),
                BarCode = Convert.ToInt32(item["BarCode"]),
                LeadNo = Convert.ToInt32(item["LeadNo"]),
                PaymentScheduleNo = Convert.ToInt32(item["PaymentScheduleNo"]),
                Id = Convert.ToInt32(item["Id"]),
                LeadStats = Convert.ToInt32(item["lead_stats"]),
                CallStats = Convert.ToInt32(item["call_stats"]),
                CallsLeftStats = Convert.ToInt32(item["calls_left_stats"]),
            };
            
            branches.Add(branch);
        }
            
        return branches;
    }

    public Tblcontrol GetControlTopOne()
    {
        Tblcontrol user = new Tblcontrol();
        DAL dAL = new DAL(_configuration);
        SqlParameter[] sQlParameters = new SqlParameter[]
        {
        };
        
        DataTable result = dAL.ExecuteStoredProcedure("GetTopOneFromTblControl", sQlParameters);

        if (result.Rows.Count > 0)
        {
            DataRow item = result.Rows[0];
            user.Cid = Convert.ToInt32(item["cid"]);
            user.Grnid = Convert.ToInt32(item["Grnid"]);
            user.PurchaseReturnId = Convert.ToInt32(item["PurchaseReturnID"]);
            user.InvoiceNo = Convert.ToInt32(item["InvoiceNo"]);
            user.PoNo = Convert.ToInt32(item["PoNo"]);
            user.AdvrptNo = Convert.ToInt32(item["ADVRptNo"]);
            user.CashMovement = Convert.ToInt32(item["CashMovement"]);
            user.IssueNoteNo = Convert.ToInt32(item["IssueNoteNo"]);
            user.BarCode = Convert.ToInt32(item["BarCode"]);
            user.LeadNo = Convert.ToInt32(item["LeadNo"]);
            user.PaymentScheduleNo = Convert.ToInt32(item["PaymentScheduleNo"]);
            user.Id = Convert.ToInt32(item["Id"]);
            user.LeadStats = Convert.ToInt32(item["lead_stats"]);
            user.CallStats = Convert.ToInt32(item["call_stats"]);
            user.CallsLeftStats = Convert.ToInt32(item["calls_left_stats"]);
        }

        return user;
    }

    public int UpdateControl(Tblcontrol tblcontrol)
    {
        DAL dAL = new DAL(_configuration);

        SqlParameter[] sQlParameters = new SqlParameter[]
        {
            new SqlParameter("@Cid", tblcontrol.Cid),
            new SqlParameter("@GRNID", tblcontrol.Grnid),
            new SqlParameter("@PurchaseReturnID", tblcontrol.PurchaseReturnId),
            new SqlParameter("@InvoiceNo", tblcontrol.InvoiceNo),
            new SqlParameter("@PoNo", tblcontrol.PoNo),
            new SqlParameter("@ADVRptNo", tblcontrol.AdvrptNo),
            new SqlParameter("@CashMovement", tblcontrol.CashMovement),
            new SqlParameter("@IssueNoteNo", tblcontrol.IssueNoteNo),
            new SqlParameter("@BankTR", tblcontrol.BankTr),
            new SqlParameter("@BarCode", tblcontrol.BarCode),
            new SqlParameter("@LeadNo", tblcontrol.LeadNo),
            new SqlParameter("@PaymentScheduleNo", tblcontrol.PaymentScheduleNo),
            new SqlParameter("@Id", tblcontrol.Id),
            new SqlParameter("@LeadStatus", tblcontrol.LeadStats),
            new SqlParameter("@CallStats", tblcontrol.CallStats),
            new SqlParameter("@CallLeftStats", tblcontrol.CallsLeftStats),
            
        };

        int res = dAL.ExecuteNonQueryStoredProcedure("UpdateTblControl", sQlParameters);
        return res;
    }
}