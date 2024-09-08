using System.Data;
using Microsoft.Data.SqlClient;

namespace API;

public class DAL
{
    private IConfiguration configuration;
    private readonly string connectionString;
    private SqlConnection connection;


    public DAL(IConfiguration configuration)
    {
        configuration = configuration;
        connectionString = configuration.GetConnectionString("DefaultConnection");
        connection = new SqlConnection(connectionString);
    }
    
    public void OpenConnection()
    {
        if (connection.State != ConnectionState.Open)
        {
            connection.Open(); 
        }
    }
    
    public void CloseConnection()
    {
        if (connection.State != ConnectionState.Closed)
        {
            connection.Close();
        }
    }
    
    public DataTable ExecuteQuery(string query)
    {
        OpenConnection();

        DataTable dataTable = new DataTable();

        using (SqlCommand cmd = new SqlCommand(query, connection))
        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
        {
            adapter.Fill(dataTable);
        }

        CloseConnection();

        return dataTable;
    }
    
    public int ExecuteNonQuery(string query)
    {
        OpenConnection();

        using (SqlCommand cmd = new SqlCommand(query, connection))
        {
            int rowsAffected = cmd.ExecuteNonQuery();
            CloseConnection();
            return rowsAffected;
        }
    }
    
    public object ExecuteScalar(string query)
    {
        OpenConnection();

        using (SqlCommand cmd = new SqlCommand(query, connection))
        {
            object result = cmd.ExecuteScalar();
            CloseConnection();
            return result;
        }
    }
    
    public DataTable ExecuteStoredProcedure(string procedureName, SqlParameter[] parameters = null)
    {
        OpenConnection();

        DataTable dataTable = new DataTable();

        using (SqlCommand cmd = new SqlCommand(procedureName, connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }

            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                adapter.Fill(dataTable);
            }
        }

        CloseConnection();

        return dataTable;
    }
    
    public int ExecuteNonQueryStoredProcedure(string procedureName, SqlParameter[] parameters = null)
    {
        OpenConnection();

        using (SqlCommand cmd = new SqlCommand(procedureName, connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }

            int rowsAffected = cmd.ExecuteNonQuery();
            CloseConnection();

            return rowsAffected;
        }
    }


    public int ExecuteBulkInsert(DataTable table)
    {
        OpenConnection();
        using (var bulkCopy = new SqlBulkCopy(connection))
        {
            bulkCopy.DestinationTableName = "tblCallInsights";
            bulkCopy.WriteToServer(table);
            
            CloseConnection();
            return -1;
        }
    }
    
    
    public object ExecuteScalarStoredProcedure(string procedureName, SqlParameter[] parameters = null)
    {
        OpenConnection();

        using (SqlCommand cmd = new SqlCommand(procedureName, connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }

            object result = cmd.ExecuteScalar();
            CloseConnection();
            return result;
        }
    }
}