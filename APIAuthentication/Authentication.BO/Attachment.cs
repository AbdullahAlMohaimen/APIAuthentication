using APIAuthentication.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAuthentication.BO
{
	[Serializable]
	public class Attachment
	{
		#region Constructor
		public Attachment() { }
		#endregion

		public string FileName { get; set; }
		public string PrevFileName { get; set; }
		public string File { get; set; }
		public EnumAttachmentType FileType { get; set; }
		public byte[] FileAsByteArray { get; set; }
		public string FileTobase64 { get { return FileAsByteArray == null ? null : Convert.ToBase64String(FileAsByteArray);}}
		public string PreviousFileTobase64 { get; set; }
		public string Extension { get; set; }
		public string ConnectionString { get; set; }
	}
}
