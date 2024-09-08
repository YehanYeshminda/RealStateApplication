using System.Data;
using API.Models;

namespace API.Repos.FileUpload;

public interface IFileUploadInterface
{
    int InsertExcelRecord(TblCallInsight tblCallInsight);
    int RemoveDuplicateRecordsBasedOnNumber();
    int InsertBulkContactData(DataTable table);
}