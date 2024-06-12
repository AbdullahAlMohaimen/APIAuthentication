using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Data;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Data.SqlClient;
using APIAuthentication.BO;
using APIAuthentication.Global;
using APIAuthentication.Authentication.DA;
using System.Reflection;
using System.Collections;
using ClosedXML;

namespace APIAuthentication.Service
{
	[Serializable]
	public class UserService : ServiceTemplate
	{
		#region User Data Mapping
		public UserService() { }

		private void MapObject(Users oUser, DataReader oReader)
		{
			base.SetObjectID(oUser, oReader.GetInt32("UserID").Value);
			oUser.LoginID = oReader.GetString("LoginID", string.Empty);
			oUser.UserName = oReader.GetString("UserName", string.Empty);
			oUser.Status = (EnumStatus)oReader.GetInt32("status", 1);
			oUser.Email = oReader.GetString("Email", string.Empty);
			oUser.RoleID = oReader.GetInt32("RoleID", 0);
			oUser.MasterID = oReader.GetInt32("MasterID", 0);
			oUser.AuthorizedDate = oReader.GetDateTime("AuthorizedDate", DateTime.MinValue);
			oUser.Password = oReader.GetString("Password", string.Empty);
			oUser.PasswordHints = oReader.GetString("PasswordHints", string.Empty);
			oUser.Salt = oReader.GetString("Salt", string.Empty);
			oUser.ForgetPasswordDate = oReader.GetDateTime("ForgetPasswordDate", DateTime.MinValue);
			oUser.LastChangeDate = oReader.GetDateTime("LastChangedDate", DateTime.MinValue);
			oUser.TempStatus = (EnumStatus)oReader.GetInt32("TempStatus", 0);
			oUser.TempStatusTime = oReader.GetDateTime("TempStatusTime", DateTime.MinValue);
			oUser.ChangePasswordNextLogon = oReader.GetInt32("ChangePasswordAtNextLogon", 0);
			oUser.PasswordResetByAdmin = oReader.GetBoolean("PasswordResetByAdmin", false);

			oUser.CreatedBy = oReader.GetInt32("CreatedBy", 0);
			oUser.CreatedDate = oReader.GetDateTime("CreatedDate", DateTime.MinValue);
			oUser.ModifiedBy = oReader.GetInt32("ModifiedBy", 0);
			oUser.ModifiedDate = oReader.GetDateTime("ModifiedDate", DateTime.MinValue);
			oUser.PasswordResetBy = oReader.GetInt32("PasswordResetBy", 0);
			oUser.PasswordResetDate = oReader.GetDateTime("PasswordResetDate", DateTime.MinValue);
			oUser.StatusChangedDate = oReader.GetDateTime("StatusChangedDate", DateTime.MinValue);
			oUser.IsApprover = oReader.GetBoolean("IsApprover", false);
			oUser.UserNo = oReader.GetString("UserNo", null);
			this.SetObjectState(oUser, APIAuthentication.Global.ObjectState.Saved);
		}
		protected override T CreateObject<T>(DataReader oReader)
		{
			Users oUser = new Users();
			MapObject(oUser, oReader);
			return oUser as T;
		}
		protected Users CreateObject(DataReader oReader)
		{
			Users oUser = new Users();
			MapObject(oUser, oReader);
			return oUser;
		}
		#endregion

		#region RoleWithReflection
		public object DataWithReflection(MethodInvocation oMethodInvocation)
		{
			List<Users> oUsers = new List<Users>();
			DataTable userDatatable = new DataTable();
			MethodInfo oMethodInfo = typeof(UserService).GetMethod(oMethodInvocation.MethodName, oMethodInvocation.ParameterTypes);

			#region Check Method is available or not
			if (oMethodInfo == null)
			{
				throw new ArgumentException($"Method '{oMethodInvocation.MethodName}' not found");
			}
			#endregion

			#region Method Parameters Check
			if (oMethodInvocation.Parameters != null)
			{
				if (oMethodInfo.GetParameters().Length != oMethodInvocation.Parameters.Length)
				{
					throw new ArgumentException($"Number of parameters provided does not match the method signature of '{oMethodInvocation.MethodName}'");
				}
			}
			#endregion

			#region Method Call
			var result = oMethodInfo.Invoke(oMethodInvocation.ServiceInstance, oMethodInvocation.Parameters);
			if (result is List<Users>)
			{
				oUsers = (List<Users>)result;
			}
			else if (result is Users)
			{
				oUsers = new List<Users> { (Users)result };
			}
			#endregion

			#region Convert into custom Datatable
			if (oUsers.Count > 0)
				userDatatable = GetUserDataTable(oUsers);
			#endregion

			return userDatatable;
		}
		#endregion

		#region  GetUserbyID()
		public Users Get(int userID)
		{
			Users oUser = null;
			TransactionContext tc = new TransactionContext();
			try
			{
				tc.Begin();
				DataReader oreader = new DataReader(UsersDA.Get(tc, userID));
				if (oreader.Read())
				{
					oUser = this.CreateObject<Users>(oreader);
				}
				oreader.Close();

				if (oUser != null)
				{
					oUser.Role = new RoleService().Get(tc, oUser.RoleID);
				}
				tc.End();
			}
			catch (Exception ex)
			{
				tc.Rollback();
				throw new Exception("An error occurred while getting User by ID: " + ex.Message);
			}
			return oUser;
		}
		#endregion

		#region  GetUserbyUserNo()
		public Users Get(string userNo)
		{
			Users oUser = null;
			TransactionContext tc = new TransactionContext();
			try
			{
				tc.Begin();
				DataReader oreader = new DataReader(UsersDA.Get(tc, userNo));
				if (oreader.Read())
				{
					oUser = this.CreateObject<Users>(oreader);
				}
				oreader.Close();

				if (oUser != null)
				{
					oUser.Role = new RoleService().Get(tc, oUser.RoleID);
				}
				tc.End();
			}
			catch (Exception ex)
			{
				tc.Rollback();
				throw new Exception("An error occurred while getting User by ID: " + ex.Message);
			}
			return oUser;
		}
		#endregion

		#region  GetUserbyUserName()
		public Users GetUserByUserName(string userNo)
		{
			Users oUser = null;
			TransactionContext tc = new TransactionContext();
			try
			{
				tc.Begin();
				DataReader oreader = new DataReader(UsersDA.Get(tc, userNo));
				if (oreader.Read())
				{
					oUser = this.CreateObject<Users>(oreader);
				}
				oreader.Close();

				if (oUser != null)
				{
					oUser.Role = new RoleService().Get(tc, oUser.RoleID);
				}
				tc.End();
			}
			catch (Exception ex)
			{
				tc.Rollback();
				throw new Exception("An error occurred while getting User by ID: " + ex.Message);
			}
			return oUser;
		}
		#endregion
		#region User DataTable Generate
		public DataTable GetUserDataTable(List<Users> allUsers)
		{
			CallingMethodInformation oMethodInformation = new CallingMethodInformation(Environment.StackTrace);

			DataRow oDR = null;
			DataTable roleDataTable = new DataTable();
			roleDataTable.Columns.Add("User Name", typeof(string));
			roleDataTable.Columns.Add("User No", typeof(string));
			roleDataTable.Columns.Add("Status", typeof(string));
			roleDataTable.Columns.Add("Email", typeof(string));
			roleDataTable.Columns.Add("Role", typeof(string));
			roleDataTable.Columns.Add("Last Password Changed Date", typeof(string));
			roleDataTable.Columns.Add("Is Approver", typeof(string));

			foreach (Users oUser in allUsers)
			{
				oDR = roleDataTable.NewRow();
				oDR["User Name"] = oUser.UserName;
				oDR["User No"] = oUser.UserNo;
				oDR["Status"] = GlobalFunction.GetEnumName(oUser.Status);
				oDR["Email"] = oUser.Email;
				oDR["Role"] = oUser.Role == null ? "" : oUser.Role.Name;
				oDR["Last Password Changed Date"] = oUser.LastChangeDate != DateTime.MinValue ? oUser.LastChangeDate.ToString("yyyy-MM-dd") : "";
				oDR["Is Approver"] = GlobalFunction.ReturnYesOrNo(oUser.IsApprover);

				roleDataTable.Rows.Add(oDR);
			}
			roleDataTable.TableName = "Users";
			return roleDataTable;
		}
		#endregion
	}
}
