using System.Data;
using System.Dynamic;

namespace API.Helpers;

public class DataToList
{
    public DataToList()
    {
    }

    public static List<dynamic> DataTableToDynamicList(DataTable dataTable)
    {
        List<dynamic> dynamicList = new List<dynamic>();

        foreach (DataRow row in dataTable.AsEnumerable())
        {
            dynamic dynamicObject = new ExpandoObject();

            foreach (DataColumn column in dataTable.Columns)
            {
                ((IDictionary<string, object>)dynamicObject)[column.ColumnName] = row[column];
            }

            dynamicList.Add(dynamicObject);
        }

        return dynamicList;
    }
}