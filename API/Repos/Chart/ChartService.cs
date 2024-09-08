using API.Repos.Interfaces;
using Microsoft.Data.SqlClient;

namespace API.Repos.Chart
{
    public class ChartService : IChartInterface
    {
        private readonly IConfiguration _configuration;
        public ChartService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int GeControltMonthlyCallsTargetAccordingToStaff(int staffId)
        {
            DAL dAL = new DAL(_configuration);
            SqlParameter[] sQlParameters = new SqlParameter[]
            {
                new SqlParameter("@StaffID", staffId)
            };

            var result = dAL.ExecuteScalarStoredProcedure("spGetValMonthlyTarget", sQlParameters);

            if (result == null)
            {
                return 0;
            }

            return Convert.ToInt32(result);
        }

        public int GetCallsLeftUser(int assignedUser)
        {
            DAL dAL = new DAL(_configuration);
            SqlParameter[] sQlParameters = new SqlParameter[]
            {
                new SqlParameter("@USERID", assignedUser)
            };

            var result = dAL.ExecuteScalarStoredProcedure("GetCallsLeftUser", sQlParameters);

            if (result == null)
            {
                return 0;
            }

            return Convert.ToInt32(result);
        }

        public int GetCallsLeftUserAdmin()
        {
            DAL dAL = new DAL(_configuration);
            SqlParameter[] sQlParameters = new SqlParameter[]
            {
            };

            var result = dAL.ExecuteScalarStoredProcedure("GetCallsLeftUserAdmin", sQlParameters);

            if (result == null)
            {
                return 0;
            }

            return Convert.ToInt32(result);
        }

        public int GetControlCallsMonthlyTargetAccordingToStaff(int staffId)
        {
            DAL dAL = new DAL(_configuration);
            SqlParameter[] sQlParameters = new SqlParameter[]
            {
                new SqlParameter("@StaffID", staffId)
            };
            
            var result = dAL.ExecuteScalarStoredProcedure("spGetMonthlyTarget", sQlParameters);

            if (result == null)
            {
                return 0;
            }
            
            return Convert.ToInt32(result);
        }

        public int GetLeadsConversionsTodayAdmin()
        {
            DAL dAL = new DAL(_configuration);
            SqlParameter[] sQlParameters = new SqlParameter[]
            {
            };
            
            var result = dAL.ExecuteScalarStoredProcedure("spGetLeadsForTodayAdmin", sQlParameters);

            if (result == null)
            {
                return 0;
            }
            
            return Convert.ToInt32(result);
        }

        public int GetLeadsConversionsTodayUser(int assignedUser)
        {
            DAL dAL = new DAL(_configuration);
            SqlParameter[] sQlParameters = new SqlParameter[]
            {
                new SqlParameter("@assigned", assignedUser)
            };
            
            var result = dAL.ExecuteScalarStoredProcedure("spGetLeadsForTodayUser", sQlParameters);

            if (result == null)
            {
                return 0;
            }
            
            return Convert.ToInt32(result);
        }

        public int GetTotalCallsLeft(int assignedUser)
        {
            DAL dAL = new DAL(_configuration);
            SqlParameter[] sQlParameters = new SqlParameter[]
            {
                new SqlParameter("@assigned", assignedUser)
            };
            
            var result = dAL.ExecuteScalarStoredProcedure("GetAllChartCallsLeftData", sQlParameters);

            if (result == null)
            {
                return 0;
            }
            
            return Convert.ToInt32(result);
        }

        public int GetTotalCallsLeftAdmin()
        {
            DAL dAL = new DAL(_configuration);
            SqlParameter[] sQlParameters = new SqlParameter[]
            {
            };
            
            var result = dAL.ExecuteScalarStoredProcedure("GetAllChartCallsLeftDataAdmin", sQlParameters);

            if (result == null)
            {
                return 0;
            }
            
            return Convert.ToInt32(result);
        }
    }
}