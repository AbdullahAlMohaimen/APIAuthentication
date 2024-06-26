﻿using APIAuthentication.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAuthentication.BO
{
	#region LoginInfo
	[Serializable]
	public class LoginInfo : BasicBaseObject
	{
		#region Constructor
		public LoginInfo()
		{
			_loginTime = DateTime.Now;
			_logoutTime = null;
			_isLogout = false;
		}
		#endregion

		#region Property

		#region UserID : INT 
		private int _userID;
		public int UserID
		{
			get { return _userID; }
			set { _userID = value; }
		}
		#endregion

		#region LoginID : String 
		private string _loginID;
		public string LoginID
		{
			get { return _loginID; }
			set { _loginID = value; }
		}
		#endregion

		#region UserName : String 
		private string _userName;
		public string UserName
		{
			get { return _userName; }
			set { _userName = value; }
		}
		#endregion

		#region Type : String 
		private string _type;
		public string Type
		{
			get { return _type; }
			set { _type = value; }
		}
		#endregion

		#region PCNumber : String 
		private string _pcNumber;
		public string PCNumber
		{
			get { return _pcNumber; }
			set { _pcNumber = value; }
		}
		#endregion

		#region LoginTime : DateTime 
		private DateTime _loginTime;
		public DateTime LoginTime
		{
			get { return _loginTime; }
			set { _loginTime = value; }
		}
		#endregion

		#region LogoutTime : DateTime 
		private DateTime? _logoutTime;
		public DateTime? LogoutTime
		{
			get { return _logoutTime; }
			set { _logoutTime = value; }
		}
		#endregion

		#region IsLogout : Bool
		public bool _isLogout;
		public bool IsLogout
		{
			get { return _isLogout; }
			set { _isLogout = value; }
		}
		#endregion

		#endregion

	}
	#endregion

	#region ILoginInfo Service
	public interface ILoginInfo
	{
		LoginInfo GetLoginInfo(int ID);
		List<LoginInfo> GetLoginInfos();
		string Save(LoginInfo loginInfo);
		void Delete(int ID);
	}
	#endregion
}
