using System.Data;
using API.Models;

namespace API.Repos.FileUpload;

public class FileUploadService : IFileUploadInterface
{
    private readonly IConfiguration _configuration;

    public FileUploadService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public int InsertExcelRecord(TblCallInsight tblCallInsight)
    {
        return 0;
    }

    public int RemoveDuplicateRecordsBasedOnNumber()
    {
        DAL dAL = new DAL(_configuration);
        int res = dAL.ExecuteNonQueryStoredProcedure("RemoveDuplicateCallInsights");
        return res;
    }

    public int InsertBulkContactData(DataTable table)
    {
        DAL dAL = new DAL(_configuration);
        int res = dAL.ExecuteBulkInsert(table);
        return res;
    }

    public int RemoveDuplicateRecordsBasedOnEmail()
    {
        DAL dAL = new DAL(_configuration);
        int res = dAL.ExecuteNonQueryStoredProcedure("RemoveDuplicateCallInsightsBasedOnEmail");
        return res;
    }
    
    
}