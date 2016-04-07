using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Dapper;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace GYMONE.Repository
{
    public class PaymentDetails : IPaymentDetails
    {
        private IConfiguration _connection;
        private string _connectionString;
        public PaymentDetails(IConfiguration connection)
        {
            _connection = connection;
            _connectionString = _connection.GetSection("Data").GetSection("DefaultConnection").GetSection("ConnectionString").Value;
        }
        public int InsertPaymentDetails(Models.PaymentDetailsDTO objPD)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                var paramater = new DynamicParameters();
                paramater.Add("@PaymentID", objPD.PaymentID);
                paramater.Add("@PlanID", objPD.PlanID);
                paramater.Add("@WorkouttypeID", objPD.WorkouttypeID);
                paramater.Add("@Paymenttype", "Cash");
                paramater.Add("@PaymentFromdt", objPD.PaymentFromdt);
                paramater.Add("@PaymentAmount", objPD.PaymentAmount);
                paramater.Add("@CreateUserID", objPD.CreateUserID);
                paramater.Add("@ModifyUserID", objPD.ModifyUserID);
                paramater.Add("@RecStatus", "A");
                paramater.Add("@MemberID", objPD.MemberID);
                paramater.Add("@PaymentIDOUT", dbType: DbType.Int32, direction: ParameterDirection.Output);
                con.Execute("sprocPaymentDetailsInsertUpdateSingleItem", paramater, null, 0, CommandType.StoredProcedure);
                int PaymentID = paramater.Get<int>("PaymentIDOUT");
                return PaymentID;
            }
        }

        public int UpdatePaymentDetails(Models.PaymentDetailsDTO objPD)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                var paramater = new DynamicParameters();
                paramater.Add("@PaymentID", objPD.PaymentID);
                paramater.Add("@PlanID", objPD.PlanID);
                paramater.Add("@WorkouttypeID", objPD.WorkouttypeID);
                paramater.Add("@Paymenttype", "Cash");
                paramater.Add("@PaymentFromdt", objPD.PaymentFromdt);
                paramater.Add("@PaymentAmount", objPD.PaymentAmount);
                paramater.Add("@ModifyUserID", objPD.ModifyUserID);
                paramater.Add("@RecStatus", "A");
                paramater.Add("@MemberID", objPD.MemberID);
                paramater.Add("@PaymentIDOUT", dbType: DbType.Int32, direction: ParameterDirection.Output);
                con.Execute("sprocMemberRegistrationUpdateSingleItem", paramater, null, 0, CommandType.StoredProcedure);
                int PaymentID = paramater.Get<int>("PaymentIDOUT");
                return PaymentID;
            }

        }
    }
}