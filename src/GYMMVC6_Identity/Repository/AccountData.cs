using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using GYMONE.Models;
using Dapper;
using GYMMVC6_Identity.Models;
using Microsoft.Extensions.Configuration;

namespace GYMONE.Repository
{
    public class AccountData : IAccountData
    {
        private IConfiguration _connection;
        private string _connectionString;
        public AccountData(IConfiguration connection)
        {
            _connection = connection;
            _connectionString = _connection.GetSection("Data").GetSection("DefaultConnection").GetSection("ConnectionString").Value;
        }
        public IEnumerable<Role> GetRoles()
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                return con.Query<Role>("Usp_GetRoles", null, null, true, 0, CommandType.StoredProcedure).ToList();
            }
        }

        public IEnumerable<Users> GetAllUsers()
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                return con.Query<Users>("Usp_GetAllUsers", null, null, true, 0, CommandType.StoredProcedure).ToList();
            }
        }

        public string GetRoleByUserID(string UserId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                var para = new DynamicParameters();
                para.Add("@UserId", UserId);
                return con.Query<string>("Usp_getRoleByUserID", para, null, true, 0, CommandType.StoredProcedure).SingleOrDefault();
            }
        }

        /// <summary>
        /// Get User
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public ApplicationUser GetUserByUserId(string UserId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                var para = new DynamicParameters();
                para.Add("@UserId", UserId);
                return con.Query<ApplicationUser>("Usp_GetUserInfoById", para, null, true, 0, CommandType.StoredProcedure).SingleOrDefault();
            }
        }
        public ApplicationUser GetUserByUserName(string userName)
        {
            
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                var para = new DynamicParameters();
                para.Add("@UserName", userName);
                return con.Query<ApplicationUser>("Usp_GetUserInfoByName", para, null, true, 0, CommandType.StoredProcedure).SingleOrDefault();
            }
        }

        public string GetUserID_By_UserName(string UserName)
        {
           
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                var para = new DynamicParameters();
                para.Add("@UserName", UserName);
                return con.Query<string>("Usp_UserIDbyUserName", para, null, true, 0, CommandType.StoredProcedure).SingleOrDefault();
            }
        }

        public string Get_checkUsernameExits(string useremail)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                var para = new DynamicParameters();
                para.Add("@UserEmail", useremail);
                return con.Query<string>("Usp_checkUsernameExits", para, null, true, 0, CommandType.StoredProcedure).SingleOrDefault();
            }
        }

        public bool Get_CheckUserRoles(string UserId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                var para = new DynamicParameters();
                para.Add("@UserId", UserId);
                return con.Query<bool>("Usp_CheckUserRoles", para, null, true, 0, CommandType.StoredProcedure).SingleOrDefault();
            }
        }

        public string GetUserName_BY_UserID(string UserId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                var para = new DynamicParameters();
                para.Add("@UserId", UserId);
                return con.Query<string>("Usp_UserNamebyUserID", para, null, true, 0, CommandType.StoredProcedure).SingleOrDefault();
            }
        }

        public IEnumerable<AllroleandUser> DisplayAllUser_And_Roles()
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                return con.Query<AllroleandUser>("Usp_DisplayAllUser_And_Roles", null, null, true, 0, CommandType.StoredProcedure).ToList();
            }
        }
    }
}