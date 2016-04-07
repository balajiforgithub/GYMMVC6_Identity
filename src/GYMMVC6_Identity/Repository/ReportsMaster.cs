using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using GYMONE.Models;
using Microsoft.Extensions.Configuration;

namespace GYMONE.Repository
{
    public class ReportsMaster : IlReports
    {
        private IConfiguration _connection;
        private string _connectionString;
        public ReportsMaster(IConfiguration connection)
        {
            _connection = connection;
            _connectionString = _connection.GetSection("Data").GetSection("DefaultConnection").GetSection("ConnectionString").Value;
        }
        public DataSet Generate_AllMemberDetailsReport()
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                DataSet ds = new DataSet();

                try
                {
                    SqlCommand cmd = new SqlCommand("Usp_GetAllRenwalrecords", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(ds);

                    if (ds.Tables.Count > 0)
                    {
                        return ds;
                    }

                    else
                    {
                        return ds = null;
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    ds.Dispose();
                }

            }
        }

        public DataSet Get_MonthwisePayment_details(string MonthID)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                DataSet ds = new DataSet();

                try
                {
                    SqlCommand cmd = new SqlCommand("Usp_GetMonthwisepaymentdetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@month", MonthID);
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(ds);

                    if (ds.Tables.Count > 0)
                    {
                        return ds;
                    }

                    else
                    {
                        return ds = null;
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    ds.Dispose();
                }

            }
        }

        public DataSet Get_YearwisePayment_details(string YearID)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                DataSet ds = new DataSet();

                try
                {
                    SqlCommand cmd = new SqlCommand("Usp_GetYearwisepaymentdetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@year", YearID);
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(ds);

                    if (ds.Tables.Count > 0)
                    {
                        return ds;
                    }

                    else
                    {
                        return ds = null;
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    ds.Dispose();
                }

            }
        }

        public DataSet Get_RenewalReport(RenewalSearch objRS)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                DataSet ds = new DataSet();

                try
                {
                    SqlCommand cmd = new SqlCommand("Usp_GetAllRenwalrecordsFromBetweenDate", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@exactdate", objRS.Exactdate);
                    cmd.Parameters.AddWithValue("@Paymentfromdt", objRS.Fromdate);
                    cmd.Parameters.AddWithValue("@Paymenttodt", objRS.Todate);
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(ds);

                    if (ds.Tables.Count > 0)
                    {
                        return ds;
                    }

                    else
                    {
                        return ds = null;
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    ds.Dispose();
                }

            }
        }
    }
}