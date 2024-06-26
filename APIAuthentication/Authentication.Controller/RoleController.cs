﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using Microsoft.Extensions.Options;
using System.Reflection;
using APIAuthentication.BO;
using APIAuthentication.Service;
using APIAuthentication.Global;

namespace APIAuthentication.Controller
{
	[Route("api/Role")]
	[ApiController]
	public class RoleController : ControllerBase
	{
		#region Role Endpoint

		#region GetAllRole
		[HttpGet("GetAllRole")]
		public IActionResult GetAllRole()	
		{
			DataTable roleDataTable = new DataTable();
			string json;
			try
			{
				object[] parameters = null;
				MethodInvocation oMethodInvocation = new MethodInvocation("GetAllRole", EnumServiceInstance.RoleService, parameters);
				roleDataTable = (DataTable) new RoleService().DataWithReflection(oMethodInvocation);
				json = JsonConvert.SerializeObject(roleDataTable);
			}
			catch (Exception e)
			{
				return StatusCode(500, new { message = e.Message });
			}
			return Content(json, "application/json");
		}
		#endregion

		#region GetAllRoleByStatus
		[HttpGet("GetAllRoleByStatus/{status}")]
		public IActionResult GetAllRoleByStatus(EnumStatus status)
		{
			DataTable roleDataTable = new DataTable();
			string json;
			try
			{
				object[] parameters = new object[] { status };
				MethodInvocation oMethodInvocation = new MethodInvocation("GetAllRoleByStatus", EnumServiceInstance.RoleService, parameters);
				roleDataTable = (DataTable)new RoleService().DataWithReflection(oMethodInvocation);
				json = JsonConvert.SerializeObject(roleDataTable);
			}
			catch (Exception e)
			{
				return StatusCode(500, new { message = e.Message });
			}
			return Content(json, "application/json");
		}
		#endregion

		#region GetRoleByID
		[HttpGet("GetRoleByID/{ID}")]
		public IActionResult GetRoleByID(int ID)
		{
			DataTable roleDataTable = new DataTable();
			string json;
			try
			{
				object[] parameters = new object[] { ID };
				MethodInvocation oMethodInvocation = new MethodInvocation("Get", EnumServiceInstance.RoleService, parameters);
				roleDataTable = (DataTable)new RoleService().DataWithReflection(oMethodInvocation);
				json = JsonConvert.SerializeObject(roleDataTable);
			}
			catch (Exception e)
			{
				return StatusCode(500, new { message = e.Message });
			}
			return Content(json, "application/json");
		}
		#endregion

		#region GetRoleByCode
		[HttpGet("GetRoleByCode/{Code}")]
		public IActionResult GetRoleByCode(string Code)
		{
			DataTable roleDataTable = new DataTable();
			string json;
			try
			{
				object[] parameters = new object[] { Code };
				MethodInvocation oMethodInvocation = new MethodInvocation("Get", EnumServiceInstance.RoleService, parameters);
				roleDataTable = (DataTable)new RoleService().DataWithReflection(oMethodInvocation);
				json = JsonConvert.SerializeObject(roleDataTable);
			}
			catch (Exception e)
			{
				return StatusCode(500, new { message = e.Message });
			}
			return Content(json, "application/json");
		}
		#endregion

		#endregion
	}
}
