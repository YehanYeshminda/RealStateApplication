using System.Data;
using API.Models;
using API.Repos.Dtos;
using API.Repos.LeadStatus;
using Microsoft.Data.SqlClient;

namespace API.Repos.Notification;

public class Notification : INotification
{
    private readonly IConfiguration _configuration;

    public Notification(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public void InsertNotification(int notify, DateTime date, string time, string message, int priorityid, int addby, DateTime addon, string forwardto, DateTime snoozeon, DateTime from)
    {
        using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("InsertNotification", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@notify", notify);
                command.Parameters.AddWithValue("@date", date);
                command.Parameters.AddWithValue("@time", time);
                command.Parameters.AddWithValue("@message", message);
                command.Parameters.AddWithValue("@priorityid", priorityid);
                command.Parameters.AddWithValue("@addby", addby);
                command.Parameters.AddWithValue("@addon", addon);
                command.Parameters.AddWithValue("@forwardto", forwardto);
                command.Parameters.AddWithValue("@snoozeon", snoozeon);
                command.Parameters.AddWithValue("@from", from);
                command.Parameters.AddWithValue("@status", 0);

                command.ExecuteNonQuery();
            }
        }
    }

    public void UpdateNotification(int id, int notify, DateTime date, string time, string message, int priorityid, string forwardto, DateTime snoozeon, DateTime from)
    {
        using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("UpdateNotification", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@id", id); 
                command.Parameters.AddWithValue("@notify", notify);
                command.Parameters.AddWithValue("@date", date);
                command.Parameters.AddWithValue("@time", time);
                command.Parameters.AddWithValue("@message", message);
                command.Parameters.AddWithValue("@priorityid", priorityid);
                command.Parameters.AddWithValue("@forwardto", forwardto);
                command.Parameters.AddWithValue("@snoozeon", snoozeon);
                command.Parameters.AddWithValue("@from", from);

                command.ExecuteNonQuery();
            }
        }
    }




    public List<VNotificationReturnCustom> GetAllNotificationsForUserForAll(bool isAdmin, int? id)
    {
        List<VNotificationReturnCustom> branches = new List<VNotificationReturnCustom>();

        DAL dAL = new DAL(_configuration);

        if (isAdmin)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
            };
        
            DataTable result = dAL.ExecuteStoredProcedure("GetNotificationsForAllUsers", parameters);

            foreach (DataRow item in result.Rows)
            {
                VNotificationReturnCustom branch = new VNotificationReturnCustom
                {
                    Id = item["Id"] != DBNull.Value ? Convert.ToInt32(item["Id"]) : 0,
                    FromFirstName = item["firstname"] != DBNull.Value ? Convert.ToString(item["firstname"]) : "",
                    FromLastName = item["lastname"] != DBNull.Value ? Convert.ToString(item["lastname"]) : "",
                    Time = item["time"] != DBNull.Value ? Convert.ToString(item["time"]) : "",
                    Message = item["message"] != DBNull.Value ? Convert.ToString(item["message"]) : "",
                    PriorityId = item["priorityid"] != DBNull.Value ? Convert.ToString(item["priorityid"]) : "",
                    AddBy = item["AddBy"] != DBNull.Value ? Convert.ToString(item["AddBy"]) : "",
                    AddonTime = item["addon"] != DBNull.Value ? Convert.ToDateTime(item["addon"]) : DateTime.MinValue,
                    DateAdded = item["date"] != DBNull.Value ? Convert.ToDateTime(item["date"]) : DateTime.MinValue,
                    ForwardTo = item["ForwardTo"] != DBNull.Value ? Convert.ToString(item["ForwardTo"]) : "",
                    SnoozeOn = item["snoozeon"] != DBNull.Value ? Convert.ToDateTime(item["snoozeon"]) : DateTime.MinValue,
                    FromTime = item["from"] != DBNull.Value ? Convert.ToDateTime(item["from"]) : DateTime.MinValue,

                };
            
                branches.Add(branch);
            }
        }
        else
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ForwardTo", id),
            };
        
            DataTable result = dAL.ExecuteStoredProcedure("GetNotificationsForAllForwardToUser", parameters);

            foreach (DataRow item in result.Rows)
            {
                VNotificationReturnCustom branch = new VNotificationReturnCustom
                {
                    Id = item["Id"] != DBNull.Value ? Convert.ToInt32(item["Id"]) : 0,
                    FromFirstName = item["firstname"] != DBNull.Value ? Convert.ToString(item["firstname"]) : "",
                    FromLastName = item["lastname"] != DBNull.Value ? Convert.ToString(item["lastname"]) : "",
                    Time = item["time"] != DBNull.Value ? Convert.ToString(item["time"]) : "",
                    Message = item["message"] != DBNull.Value ? Convert.ToString(item["message"]) : "",
                    PriorityId = item["priorityid"] != DBNull.Value ? Convert.ToString(item["priorityid"]) : "",
                    AddBy = item["AddBy"] != DBNull.Value ? Convert.ToString(item["AddBy"]) : "",
                    AddonTime = item["addon"] != DBNull.Value ? Convert.ToDateTime(item["addon"]) : DateTime.MinValue,
                    DateAdded = item["date"] != DBNull.Value ? Convert.ToDateTime(item["date"]) : DateTime.MinValue,
                    ForwardTo = item["ForwardTo"] != DBNull.Value ? Convert.ToString(item["ForwardTo"]) : "",
                    SnoozeOn = item["snoozeon"] != DBNull.Value ? Convert.ToDateTime(item["snoozeon"]) : DateTime.MinValue,
                    FromTime = item["from"] != DBNull.Value ? Convert.ToDateTime(item["from"]) : DateTime.MinValue,

                };
            
                branches.Add(branch);
            }
        }
            
        return branches;
    }
    
    public List<VNotificationForUser> GetAllNotificationsForUser(int id, DateTime startDate, DateTime endDate)
    {
        List<VNotificationForUser> branches = new List<VNotificationForUser>();

        DAL dAL = new DAL(_configuration);
        SqlParameter[] parameters = new SqlParameter[]
        {
            new SqlParameter("@StartDate", startDate),
            new SqlParameter("@EndDate", endDate),
            new SqlParameter("@ForwardTo", id),
        };
        
        DataTable result = dAL.ExecuteStoredProcedure("GetNotificationsForUser", parameters);

        foreach (DataRow item in result.Rows)
        {
            VNotificationForUser branch = new VNotificationForUser
            {
                Notify = item["notify"] != DBNull.Value ? Convert.ToString(item["notify"]) : "",
                Date = item["date"] != DBNull.Value ? Convert.ToDateTime(item["date"]) : DateTime.MinValue,
                Time = item["time"] != DBNull.Value ? Convert.ToString(item["time"]) : "",
                Message = item["message"] != DBNull.Value ? Convert.ToString(item["message"]) : "",
                FirstName = item["firstname"] != DBNull.Value ? Convert.ToString(item["firstname"]) : "",
                LastName = item["lastname"] != DBNull.Value ? Convert.ToString(item["lastname"]) : "",
            };
            
            branches.Add(branch);
        }
            
        return branches;
    }


}