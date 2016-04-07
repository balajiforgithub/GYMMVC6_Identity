using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Configuration;
using System.Data.SqlClient;
using GYMONE.Models;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace GYMONE.Repository
{
    public class Paymentlisting : IPaymentlisting
    {
        private IConfiguration _connection;
        private string _connectionString;

        public Paymentlisting(IConfiguration connection)
        {
            _connection = connection;
            _connectionString = _connection.GetSection("Data").GetSection("DefaultConnection").GetSection("ConnectionString").Value;
        }
        public IEnumerable<PaymentlistingDTO> AllPaymentDetails(string MemberID)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                var param = new  DynamicParameters();
                param.Add("@MemberID", MemberID);
                return con.Query<PaymentlistingDTO>("Usp_PaymentDetailinfo", param, null, true, 0, commandType: CommandType.StoredProcedure);
            }

        }


        public IEnumerable<PaymentlistingDTO> AllPaymentDetails()
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                return con.Query<PaymentlistingDTO>("Usp_ALLPaymentDetailinfo", null, null, true, 0, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<PaymentAutocompDTO> ListofMemberNo(string Memberno)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                var param = new DynamicParameters();
                param.Add("@MemberID", Memberno);
                return con.Query<PaymentAutocompDTO>("USP_listofMemberno", param, null, true, 0, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<PaymentAutocompDTO> ListofMemberName(string Membername)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                var param = new DynamicParameters();
                param.Add("@MemberFName", Membername);
                return con.Query<PaymentAutocompDTO>("USP_listofMemberName", param, null, true, 0, commandType: CommandType.StoredProcedure);
            }
        }


    }
}