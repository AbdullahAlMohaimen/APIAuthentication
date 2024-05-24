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
		internal static IDataReader GetUserByUserNo(TransactionContext tc, string userNo)
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
			getCommand = new SqlCommand("Select * from Users where UserNo = '" + userName + "'", tc.Connection, tc.Transaction);
			dr = getCommand.ExecuteReader();
			return dr;
		}
		#endregion
	}
}
