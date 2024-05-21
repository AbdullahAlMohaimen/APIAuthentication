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
	public class RoleDA
	{
		#region  Constructor
		public RoleDA() { }
		#endregion

		#region  GetAllRole()
		internal static IDataReader GetAllRole(TransactionContext tc)
		{
			SqlDataReader dr = null;
			SqlCommand getCommand = null;
			getCommand = new SqlCommand("Select * from Role Order by RoleID", tc.Connection, tc.Transaction);
			dr = getCommand.ExecuteReader();
			return dr;
		}
		#endregion

		#region  GetRoleByStatus()
		internal static IDataReader GetRoleByStatus(TransactionContext tc, EnumStatus status)
		{
			SqlDataReader dr = null;
			SqlCommand getCommand = null;
			getCommand = new SqlCommand("Select * from Role where status = '"+(int)status+"' Order by RoleID", tc.Connection, tc.Transaction);
			dr = getCommand.ExecuteReader();
			return dr;
		}
		#endregion

		#region  GetRoleByID()
		internal static IDataReader GetRoleByID(TransactionContext tc, int ID)
		{
			SqlDataReader dr = null;
			SqlCommand getCommand = null;
			getCommand = new SqlCommand("Select * from Role where RoleID = '"+ID+"'", tc.Connection, tc.Transaction);
			dr = getCommand.ExecuteReader();
			return dr;
		}
		#endregion

		#region  GetByCode()
		internal static IDataReader GetRoleByCode(TransactionContext tc, string code)
		{
			SqlDataReader dr = null;
			SqlCommand getCommand = null;
			getCommand = new SqlCommand("Select * from Role where Code = '" + code + "'", tc.Connection, tc.Transaction);
			dr = getCommand.ExecuteReader();
			return dr;
		}
		#endregion
	}
}
