using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Net;
using APIAuthentication.Global;

namespace APIAuthentication
{
	public class UsersDA
	{
		#region  Constructor
		public UsersDA() { }
		#endregion

		#region  GetUserByID
		internal static IDataReader Get(TransactionContext tc, int userID)
		{
			SqlDataReader dr = null;
			SqlCommand getCommand = null;
			getCommand = new SqlCommand("Select * from Users where UserID = '" + userID + "'", tc.Connection, tc.Transaction);
			dr = getCommand.ExecuteReader();
			return dr;
		}
		#endregion

		#region  GetUserByUserNo
		internal static IDataReader Get(TransactionContext tc, string userNo)
		{
			SqlDataReader dr = null;
			SqlCommand getCommand = null;
			getCommand = new SqlCommand("Select * from Users where UserNo = '" + userNo + "'", tc.Connection, tc.Transaction);
			dr = getCommand.ExecuteReader();
			return dr;
		}
		#endregion

		#region  GetUserByUserName
		internal static IDataReader GetUserByUserName(TransactionContext tc, string userName)
		{
			SqlDataReader dr = null;
			SqlCommand getCommand = null;
			getCommand = new SqlCommand("Select * from Users where UserName = '" + userName + "'", tc.Connection, tc.Transaction);
			dr = getCommand.ExecuteReader();
			return dr;
		}
		#endregion

		#region  GetUserByStatus
		internal static IDataReader GetUserByStatus(TransactionContext tc, EnumStatus status)
		{
			SqlDataReader dr = null;
			SqlCommand getCommand = null;
			getCommand = new SqlCommand("Select * from Users where Status = '" + (int)status + "'", tc.Connection, tc.Transaction);
			dr = getCommand.ExecuteReader();
			return dr;
		}
		#endregion

		#region  GetApproverUser
		internal static IDataReader GetApproverUser(TransactionContext tc, bool isApprover)
		{
			SqlDataReader dr = null;
			SqlCommand getCommand = null;
			getCommand = new SqlCommand("Select * from Users where IsApprover = '" + isApprover + "'", tc.Connection, tc.Transaction);
			dr = getCommand.ExecuteReader();
			return dr;
		}
		#endregion

		#region  GetApproverUser
		internal static IDataReader GetUserIsPasswordResetByAdmin(TransactionContext tc, bool isPasswordResetByAdmin)
		{
			SqlDataReader dr = null;
			SqlCommand getCommand = null;
			getCommand = new SqlCommand("Select * from Users where PasswordResetByAdmin = '" + isPasswordResetByAdmin + "'", tc.Connection, tc.Transaction);
			dr = getCommand.ExecuteReader();
			return dr;
		}
		#endregion

		#region  GetNewUser
		internal static IDataReader GetNewUser(TransactionContext tc, DateTime fromDate, DateTime toDate)
		{
			SqlDataReader dr = null;
			SqlCommand getCommand = null;
			getCommand = new SqlCommand("Select * from Users where CreatedDate between '" + fromDate + "' and '" + toDate + "'", tc.Connection, tc.Transaction);
			dr = getCommand.ExecuteReader();
			return dr;
		}
		#endregion

		#region  GetUpdatedUser
		internal static IDataReader GetUpdatedUser(TransactionContext tc, DateTime fromDate, DateTime toDate)
		{
			SqlDataReader dr = null;
			SqlCommand getCommand = null;
			getCommand = new SqlCommand("Select * from Users where ModifiedDate between '" + fromDate + "' and '" + toDate + "'", tc.Connection, tc.Transaction);
			dr = getCommand.ExecuteReader();
			return dr;
		}
		#endregion

		#region  GetUserByRoleID
		internal static IDataReader GetUserByRoleID(TransactionContext tc, int roleID)
		{
			SqlDataReader dr = null;
			SqlCommand getCommand = null;
			getCommand = new SqlCommand("Select * from Users where RoleID = '" + roleID + "'", tc.Connection, tc.Transaction);
			dr = getCommand.ExecuteReader();
			return dr;
		}
		#endregion
	}
}
