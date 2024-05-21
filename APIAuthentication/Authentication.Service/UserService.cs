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

		#region  GetUserbyID()
		public DataTable GetUser(int userID)
		{
			DataTable userDataTable = new DataTable();
			List<Users> users = new List<Users>();
			Users oUser = new Users();
			TransactionContext tc = new TransactionContext();
			try
			{
				tc.Begin();
				DataReader oreader = new DataReader(UsersDA.Get(tc, userID));
				if (oreader.Read())
				{
					oUser = this.CreateObject<Users>(oreader);
				}
				tc.End();

				if (oUser != null)
				{
					users.Add(oUser);
				}
				userDataTable = GetUserDataTable(users);
			}
			catch (Exception ex)
			{
				tc.Rollback();
				throw new Exception("An error occurred while getting User by ID: " + ex.Message);
			}
			return userDataTable;
		}
		#endregion

		#region User DataTable Generate
		public DataTable GetUserDataTable(List<Users> allUsers)
		{
			DataRow oDR = null;
			DataTable roleDataTable = new DataTable();
			roleDataTable.Columns.Add("User Name", typeof(string));
			roleDataTable.Columns.Add("User No", typeof(string));
			roleDataTable.Columns.Add("Status", typeof(string));
			roleDataTable.Columns.Add("Role", typeof(string));
			roleDataTable.Columns.Add("Is Approver", typeof(string));

			foreach (Users oUser in allUsers)
			{
				oDR = roleDataTable.NewRow();
				oDR["User Name"] = oUser.UserName;
				oDR["User No"] = oUser.UserNo;
				oDR["Status"] = GlobalFunction.GetEnumName(oUser.Status);
				oDR["Role"] = "";
				oDR["Is Approver"] = GlobalFunction.ReturnYesOrNo(oUser.IsApprover); ;

				roleDataTable.Rows.Add(oDR);
			}
			roleDataTable.TableName = "Role";
			return roleDataTable;
		}
		#endregion
	}
}
