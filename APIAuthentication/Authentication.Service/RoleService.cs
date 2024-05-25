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
	public class RoleService : ServiceTemplate
	{
		#region Role Data Mapping
		public RoleService() { }

		private void MapObject(Role oRole, DataReader oReader)
		{
			base.SetObjectID(oRole, oReader.GetInt32("RoleID").Value);
			oRole.Code = oReader.GetString("Code", string.Empty);
			oRole.Name = oReader.GetString("Name", string.Empty);
			oRole.Status = (EnumStatus)oReader.GetInt32("Status", 1);
			oRole.Description = oReader.GetString("Description", string.Empty);
			oRole.CreatedBy = oReader.GetInt32("CreatedBy", 0);
			oRole.CreatedDate = oReader.GetDateTime("CreatedDate");
			oRole.ModifiedBy = oReader.GetInt32("ModifiedBy", 0);
			oRole.ModifiedDate = oReader.GetDateTime("ModifiedDate");
			this.SetObjectState(oRole, APIAuthentication.Global.ObjectState.Saved);
		}
		protected override T CreateObject<T>(DataReader oReader)
		{
			Role oRole = new Role();
			MapObject(oRole, oReader);
			return oRole as T;
		}

		protected Role CreateObject(DataReader oReader)
		{
			Role oRole = new Role();
			MapObject(oRole, oReader);
			return oRole;
		}
		#endregion


		#region RoleWithReflection
		public object DataWithReflection(MethodInvocation oMethodInvocation)
		{
			List<Role> oRole = new List<Role>();
			DataTable roleDatatable = new DataTable();
			MethodInfo oMethodInfo = typeof(RoleService).GetMethod(oMethodInvocation.MethodName, oMethodInvocation.ParameterTypes);

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
			if (result is List<Role>)
			{
				oRole = (List<Role>)result;
			}
			else if (result is Role)
			{
				oRole = new List<Role> { (Role)result };
			}
			#endregion

			#region Convert into custom Datatable
			if (oRole.Count > 0)
				roleDatatable = GetRoleDataTable(oRole);
			#endregion

			return roleDatatable;
		}
		#endregion

		#region  GetAllRole()
		public List<Role> GetAllRole()
		{
			List<Role> roles = new List<Role>();
			TransactionContext tc = new TransactionContext();
			try
			{
				tc.Begin();
				DataReader dr = new DataReader(RoleDA.GetAllRole(tc));
				roles = this.CreateObjects<Role>(dr);
				dr.Close();
				tc.End();
			}
			catch (Exception ex)
			{
				tc.Rollback();
				throw new Exception("An error occurred while getting all roles: " + ex.Message);
			}
			return roles;
		}
		#endregion

		#region  GetAllRoleByStatus()
		public List<Role> GetAllRoleByStatus(EnumStatus status)
		{
			List<Role> roles = new List<Role>();
			TransactionContext tc = new TransactionContext();
			try
			{
				tc.Begin();
				DataReader dr = new DataReader(RoleDA.GetRoleByStatus(tc, status));
				roles = this.CreateObjects<Role>(dr);
				dr.Close();
				tc.End();
			}
			catch (Exception ex)
			{
				tc.Rollback();
				throw new Exception("An error occurred while getting roles by status : " + ex.Message);
			}
			return roles;
		}
		#endregion

		#region  GetRoleByID()
		public Role Get(int ID)
		{
			Role oRole = new Role();
			TransactionContext tc = new TransactionContext();
			try
			{
				tc.Begin();
				DataReader oreader = new DataReader(RoleDA.GetRoleByID(tc,ID));
				if (oreader.Read())
				{
					oRole = this.CreateObject<Role>(oreader);
				}
				oreader.Close();
				tc.End();
			}
			catch (Exception ex)
			{
				tc.Rollback();
				throw new Exception("An error occurred while getting Role by ID: " + ex.Message);
			}
			return oRole;
		}
		#endregion

		#region  GetRoleByCode()
		public Role Get(string code)
		{
			Role oRole = new Role();
			TransactionContext tc = new TransactionContext();
			try
			{
				tc.Begin();
				DataReader oreader = new DataReader(RoleDA.GetRoleByCode(tc, code));
				if (oreader.Read())
				{
					oRole = this.CreateObject<Role>(oreader);
				}
				oreader.Close();
				tc.End();
			}
			catch (Exception ex)
			{
				tc.Rollback();
				throw new Exception("An error occurred while getting Role by ID: " + ex.Message);
			}
			return oRole;
		}
		#endregion

		#region Role DataTable Generate
		public DataTable GetRoleDataTable(List<Role> allRoles)
		{
			DataRow oDR = null;
			DataTable roleDataTable = new DataTable();
			roleDataTable.Columns.Add("Code", typeof(string));
			roleDataTable.Columns.Add("Name", typeof(string));
			roleDataTable.Columns.Add("Status", typeof(string));
			roleDataTable.Columns.Add("Description", typeof(string));

			foreach (Role oRole in allRoles)
			{
				oDR = roleDataTable.NewRow();
				oDR["Code"] = oRole.Code;
				oDR["Name"] = oRole.Name;
				oDR["Status"] = GlobalFunction.GetEnumName(oRole.Status);
				oDR["Description"] = oRole.Description;

				roleDataTable.Rows.Add(oDR);
			}
			roleDataTable.TableName = "Role";
			return roleDataTable;
		}
		#endregion
	}
}
