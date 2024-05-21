using System.Data;
using System.Text.RegularExpressions;
using ClosedXML.Excel;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.Globalization;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Aspose.Cells;
using Aspose.Pdf;
using System.Drawing.Imaging;
using APIAuthentication.BO;
using Aspose.CAD.ImageOptions;
using APIAuthentication.Global;
using OfficeOpenXml;

namespace APIAuthentication
{
	public class GlobalFunction
	{
		#region Encrypt/Decrypt
		public static string Encrypt(string plainText, string key)
		{
			if (string.IsNullOrEmpty(plainText))
				throw new ArgumentNullException(nameof(plainText));
			if (string.IsNullOrEmpty(key))
				throw new ArgumentNullException(nameof(key));

			using (Aes aesAlg = Aes.Create())
			{
				aesAlg.Key = Encoding.UTF8.GetBytes(key);
				aesAlg.IV = new byte[aesAlg.BlockSize / 8];

				ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
				using (MemoryStream msEncrypt = new MemoryStream())
				{
					using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
					{
						using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
						{
							swEncrypt.Write(plainText);
						}
					}
					return Convert.ToBase64String(msEncrypt.ToArray());
				}
			}
		}
		public static string Decrypt(string cipherText, string key)
		{
			if (string.IsNullOrEmpty(cipherText))
				throw new ArgumentNullException(nameof(cipherText));
			if (string.IsNullOrEmpty(key))
				throw new ArgumentNullException(nameof(key));

			byte[] cipherBytes = Convert.FromBase64String(cipherText);
			using (Aes aesAlg = Aes.Create())
			{
				aesAlg.Key = Encoding.UTF8.GetBytes(key);
				aesAlg.IV = new byte[aesAlg.BlockSize / 8];

				ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
				using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
				{
					using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
					{
						using (StreamReader srDecrypt = new StreamReader(csDecrypt))
						{
							return srDecrypt.ReadToEnd();
						}
					}
				}
			}
		}
		#endregion

		#region String Manipulation
		public static string ToCamelCase(string input)
		{
			if (string.IsNullOrEmpty(input))
				return input;

			return char.ToLower(input[0]) + input.Substring(1);
		}
		public static string ToSnakeCase(string input)
		{
			if (string.IsNullOrEmpty(input))
				return input;

			return string.Concat(input.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
		}
		public static string Base64Encode(string plainText)
		{
			if (string.IsNullOrEmpty(plainText))
				return plainText;

			var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
			return Convert.ToBase64String(plainTextBytes);
		}
		public static string Base64Decode(string base64EncodedData)
		{
			if (string.IsNullOrEmpty(base64EncodedData))
				return base64EncodedData;

			var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
			return Encoding.UTF8.GetString(base64EncodedBytes);
		}
		public static string RemoveSpecialCharacters(string input)
		{
			return Regex.Replace(input, @"[^0-9a-zA-Z]+", "");
		}
		public static string RemoveWhitespace(string input)
		{
			return Regex.Replace(input, @"\s+", "");
		}
		public static bool ValidateAlphanumeric(string input)
		{
			return Regex.IsMatch(input, "^[a-zA-Z0-9]*$");
		}
		public static double GenerateRandomDouble()
		{
			Random random = new Random();
			return random.NextDouble();
		}
		public static string ReverseString(string input)
		{
			if (string.IsNullOrEmpty(input))
				return input;

			char[] charArray = input.ToCharArray();
			Array.Reverse(charArray);
			return new string(charArray);
		}
		public static string TruncateString(string input, int maxLength)
		{
			if (input.Length > maxLength)
			{
				return input.Substring(0, maxLength);
			}
			return input;
		}
		public static string[] SplitString(string input, char separator)
		{
			return input.Split(separator);
		}
		public static string JoinStrings(IEnumerable<string> strings, string separator)
		{
			return string.Join(separator, strings);
		}
		public static string CapitalizeFirstLetter(string input)
		{
			if (string.IsNullOrEmpty(input))
				return input;
			return char.ToUpper(input[0]) + input.Substring(1);
		}
		public static string RemoveDuplicates(string input)
		{
			if (string.IsNullOrEmpty(input))
				return input;

			return new string(input.Distinct().ToArray());
		}
		public static string ReplaceStringByRegex(string input, string pattern, string replacement)
		{
			if (string.IsNullOrEmpty(input))
				return input;

			return Regex.Replace(input, pattern, replacement);
		}
		public static string MillionToInWords(int no)
		{
			if (no == 0)
				return "zero";

			if (no < 0)
				return "minus " + MillionToInWords(Math.Abs(no));

			string stringValue = "";

			if ((no / 1000000) > 0)
			{
				stringValue += MillionToInWords(no / 1000000) + " million ";
				no %= 1000000;
			}
			if ((no / 1000) > 0)
			{
				stringValue += MillionToInWords(no / 1000) + " thousand ";
				no %= 1000;
			}
			if ((no / 100) > 0)
			{
				stringValue += MillionToInWords(no / 100) + " hundred ";
				no %= 100;
			}
			if (no > 0)
			{
				if (stringValue != "")
					stringValue += "and ";

				var units = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
				var tens = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

				if (no < 20)
					stringValue += units[no];
				else
				{
					stringValue += tens[no / 10];
					if ((no % 10) > 0)
						stringValue += "-" + units[no % 10];
				}
			}
			return stringValue;
		}
		#endregion

		#region Date and Time
		public static string AgeCalculate(DateTime birthDate, DateTime currentDate)
		{
			int Years = 0;
			int Months = 0;
			int Days = 0;

			if ((currentDate.Year - birthDate.Year) > 0 ||
				(((currentDate.Year - birthDate.Year) == 0) && ((birthDate.Month < currentDate.Month) ||
				((birthDate.Month == currentDate.Month) && (birthDate.Day <= currentDate.Day)))))
			{
				int DaysInBdayMonth = DateTime.DaysInMonth(birthDate.Year, birthDate.Month);
				int DaysRemain = currentDate.Day + (DaysInBdayMonth - birthDate.Day);

				if (currentDate.Month > birthDate.Month)
				{
					Years = currentDate.Year - birthDate.Year;
					Months = currentDate.Month - (birthDate.Month + 1) + Math.Abs(DaysRemain / DaysInBdayMonth);
					Days = (DaysRemain % DaysInBdayMonth + DaysInBdayMonth) % DaysInBdayMonth;
				}
				else if (currentDate.Month == birthDate.Month)
				{
					if (currentDate.Day >= birthDate.Day)
					{
						Years = currentDate.Year - birthDate.Year;
						Months = 0;
						Days = currentDate.Day - birthDate.Day;
					}
					else
					{
						Years = (currentDate.Year - 1) - birthDate.Year;
						Months = 11;
						Days = DateTime.DaysInMonth(birthDate.Year, birthDate.Month) - (birthDate.Day - currentDate.Day);
					}
				}
				else
				{
					Years = (currentDate.Year - 1) - birthDate.Year;
					Months = currentDate.Month + (11 - birthDate.Month) + Math.Abs(DaysRemain / DaysInBdayMonth);
					Days = (DaysRemain % DaysInBdayMonth + DaysInBdayMonth) % DaysInBdayMonth;
				}
			}
			else
			{
				throw new ArgumentException("Birthday date must be earlier than current date");
			}

			return Years + " Years, " + Months + " Months, " + Days + " Days";
		}
		public static int CalculateDaysDifference(DateTime date1, DateTime date2)
		{
			return (date2 - date1).Days;
		}
		public static string GetDayOfWeek(DateTime date)
		{
			return date.DayOfWeek.ToString();
		}
		public static DateTime AddBusinessDays(DateTime date, int days)
		{
			if (days < 0) throw new ArgumentException("days cannot be negative", nameof(days));

			while (days > 0)
			{
				date = date.AddDays(1);
				if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
					days--;
			}
			return date;
		}
		public static string ConvertToLongDateString(DateTime date)
		{
			return date.ToLongDateString();
		}
		public static string ConvertToShortDateString(DateTime date)
		{
			return date.ToShortDateString();
		}
		public static string ConvertToLongTimeString(DateTime date)
		{
			return date.ToLongTimeString();
		}
		public static string ConvertToShortTimeString(DateTime date)
		{
			return date.ToShortTimeString();
		}

		public static DateTime ParseDateTime(string dateTimeString)
		{
			return DateTime.Parse(dateTimeString, CultureInfo.InvariantCulture);
		}
		public static int DayofWeek(DateTime fromDate, DateTime toDate, int day)
		{
			int i;
			DateTime dateTemp;
			i = 0;
			dateTemp = fromDate;
			while (dateTemp <= toDate)
			{
				if (((int)dateTemp.DayOfWeek == day)) i = i + 1;
				dateTemp = dateTemp.AddDays(1);
			}
			return i;
		}
		public static int DaysInMonth(int year, int month)
		{
			return DateTime.DaysInMonth(year, month);
		}
		public static int DaysBetweenDates(DateTime startDate, DateTime endDate)
		{
			return (endDate - startDate).Days;
		}
		public static DateTime GetStartOfDay(DateTime date)
		{
			return date.Date;
		}
		public static DateTime GetEndOfDay(DateTime date)
		{
			return date.Date.AddDays(1).AddTicks(-1);
		}
		public static DateTime StartOfQuarter(DateTime date)
		{
			int currentQuarter = (date.Month - 1) / 3 + 1;
			return new DateTime(date.Year, (currentQuarter - 1) * 3 + 1, 1);
		}
		public static DateTime EndOfQuarter(DateTime date)
		{
			int currentQuarter = (date.Month - 1) / 3 + 1;
			DateTime firstDayOfNextQuarter = new DateTime(date.Year, (currentQuarter - 1) * 3 + 4, 1);
			return firstDayOfNextQuarter.AddDays(-1);
		}
		public static DateTime ParseDate(string dateString)
		{
			return DateTime.Parse(dateString);
		}
		public static DateTime FirstDateOfYear(DateTime date)
		{
			return new DateTime(date.Year, 1, 1);
		}
		public static DateTime FirstDateOfYear(int year)
		{
			return new DateTime(year, 1, 1);
		}
		public static DateTime LastDateOfYear(DateTime date)
		{
			return new DateTime(date.Year, 12, 31);
		}
		public static DateTime LastDateOfYear(int year)
		{
			return new DateTime(year, 12, 31);
		}
		public static DateTime FirstDateOfMonth(DateTime date)
		{
			return new DateTime(date.Year, date.Month, 1);
		}
		public static DateTime FirstDateOfMonth(int year, int month)
		{
			return new DateTime(year, month, 1);
		}
		public static DateTime LastDateOfMonth(DateTime date)
		{
			DateTime lastDate = new DateTime(date.Year, date.Month, 1);
			lastDate = lastDate.AddMonths(1);
			lastDate = lastDate.AddDays(-1);
			return lastDate;
		}
		public static DateTime LastDateOfMonth(int year, int month)
		{
			DateTime lastDate = new DateTime(year, month, 1);
			lastDate = lastDate.AddMonths(1);
			lastDate = lastDate.AddDays(-1);
			return lastDate;
		}
		public static DateTime FirstDayOfWeek(DateTime date)
		{
			int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
			return date.AddDays(-1 * diff).Date;
		}
		public static DateTime LastDayOfWeek(DateTime date)
		{
			return LastDayOfWeek(date).AddDays(6);
		}
		public static bool IsMidnight(DateTime date)
		{
			return date.TimeOfDay == TimeSpan.Zero;
		}
		public static bool IsFutureDate(DateTime date)
		{
			return date > DateTime.Now;
		}
		public static bool IsPastDate(DateTime date)
		{
			return date < DateTime.Now;
		}
		public static DateTime EndOfDay(DateTime date)
		{
			return date.Date.AddDays(1).AddTicks(-1);
		}
		public static bool IsFirstDayOfMonth(DateTime date)
		{
			return (date.Day == 1);
		}
		public static bool IsFirstDayOfYear(DateTime date)
		{
			return (date.Month == 1 && date.Day == 1);
		}
		public static bool IsLastDayOfMonth(DateTime date)
		{
			int days = DateTime.DaysInMonth(date.Year, date.Month);

			return (days <= date.Day);
		}
		public static bool IsLastDayOfYear(DateTime date)
		{
			return (date.Month == 12 && date.Day == 31);
		}
		public static bool IsLeapYear(int year)
		{
			return (year % 4 == 0 && (year % 100 != 0 || year % 400 == 0));
		}
		public static bool IsSameDay(DateTime date1, DateTime date2)
		{
			return date1.Date == date2.Date;
		}
		public static DateTime AddWeeks(DateTime date, int weeks)
		{
			return date.AddDays(weeks * 7);
		}
		public static string ConvertToISO8601DateTime(DateTime date)
		{
			return date.ToString("o");
		}

		#endregion

		#region File Operations
		public static void WriteTextToFile(string filePath, string content)
		{
			File.WriteAllText(filePath, content);
		}
		public static string ReadTextFromFile(string filePath)
		{
			return File.ReadAllText(filePath);
		}
		public static void AppendTextToFile(string filePath, string content)
		{
			File.AppendAllText(filePath, content);
		}
		public static void DeleteFile(string filePath)
		{
			if (File.Exists(filePath))
			{
				File.Delete(filePath);
			}
		}
		public static void CopyFile(string sourceFilePath, string destinationFilePath)
		{
			File.Copy(sourceFilePath, destinationFilePath, true);
		}
		public static void MoveFile(string sourceFilePath, string destinationFilePath)
		{
			File.Move(sourceFilePath, destinationFilePath);
		}
		public static long GetFileSize(string filePath)
		{
			FileInfo fileInfo = new FileInfo(filePath);
			return fileInfo.Length;
		}
		public static string GetFileExtension(string fileName)
		{
			return Path.GetExtension(fileName);
		}
		public static string ReadFileToString(string filePath)
		{
			return File.ReadAllText(filePath);
		}
		public static void WriteStringToFile(string filePath, string content)
		{
			File.WriteAllText(filePath, content);
		}
		public static string[] GetAllFiles(string directoryPath)
		{
			return Directory.GetFiles(directoryPath);
		}
		#endregion

		#region Excel Operations
		public static DataTable ReadExcelFile(string filePath)
		{
			DataTable dt = new DataTable();
			using (var workbook = new XLWorkbook(filePath))
			{
				var worksheet = workbook.Worksheets.First();
				bool firstRow = true;
				foreach (var row in worksheet.Rows())
				{
					if (firstRow)
					{
						foreach (var cell in row.Cells())
						{
							dt.Columns.Add(cell.Value.ToString());
						}
						firstRow = false;
					}
					else
					{
						dt.Rows.Add();
						for (int i = 0; i < row.Cells().Count(); i++)
						{
							dt.Rows[dt.Rows.Count - 1][i] = row.Cell(i + 1).Value.ToString();
						}
					}
				}
			}
			return dt;
		}
		public static void WriteExcelFile(string filePath, DataTable dataTable)
		{
			using (var workbook = new XLWorkbook())
			{
				var worksheet = workbook.Worksheets.Add("Sheet1");

				for (int i = 0; i < dataTable.Columns.Count; i++)
				{
					worksheet.Cell(1, i + 1).Value = dataTable.Columns[i].ColumnName;
				}

				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					for (int j = 0; j < dataTable.Columns.Count; j++)
					{
						worksheet.Cell(i + 2, j + 1).Value = dataTable.Rows[i][j].ToString();
					}
				}
				workbook.SaveAs(filePath);
			}
		}
		#endregion

		public static int ConvertToInt(string value)
		{
			return int.Parse(value);
		}
		public static double ConvertToDouble(string value)
		{
			return double.Parse(value);
		}
		public static bool ConvertToBoolean(string value)
		{
			return bool.Parse(value);
		}
		public static string GetEnvironmentVariable(string variable)
		{
			return Environment.GetEnvironmentVariable(variable);
		}
		public static void SetEnvironmentVariable(string variable, string value)
		{
			Environment.SetEnvironmentVariable(variable, value);
		}
		public static string GetApplicationDirectory()
		{
			return AppDomain.CurrentDomain.BaseDirectory;
		}
		public static string GetEnumName<T>(T enumValue) where T : Enum
		{
			return Enum.GetName(typeof(T), enumValue);
		}
		public static string ReturnYesOrNo(bool flag)
		{
			return flag ? "yes" : "no";
		}
		public static int? GetApiDefaultIntData(string str)
		{
			int? value = null; ;
			if (str != null && str != "" && str.ToLower() != "null" && str.ToLower() != "undefined")
			{
				value = Convert.ToInt32(str);
			}
			return value;
		}
		public static DateTime GetApiDefaultDateData(string str)
		{
			DateTime dateValue = DateTime.MinValue;
			if (str != null && str != "" && str.ToLower() != "null" && str.ToLower() != "undefined")
			{
				dateValue = Convert.ToDateTime(str);
			}
			return dateValue;
		}
		public static void ExportToExcel(DataTable oDTable, string fileName, string sheetName)
		{

			string folderPath = fileName;
			using (XLWorkbook wb = new ClosedXML.Excel.XLWorkbook())
			{
				var ws = wb.Worksheets.Add(oDTable, sheetName);
				ws.Tables.FirstOrDefault().ShowAutoFilter = false;
				wb.SaveAs(folderPath);
			}
		}
		public static void ExportToExcel(StreamWriter wr, DataTable dt)
		{
			try
			{
				for (int i = 0; i < dt.Columns.Count; i++)
				{
					wr.Write(dt.Columns[i].ToString().ToUpper() + "\t");
				}
				wr.WriteLine();

				for (int i = 0; i < (dt.Rows.Count); i++)
				{
					for (int j = 0; j < dt.Columns.Count; j++)
					{
						if (dt.Rows[i][j] != null)
						{
							wr.Write(Convert.ToString(dt.Rows[i][j]) + "\t");
						}
						else
						{
							wr.Write("\t");
						}
					}
					wr.WriteLine();
				}
				wr.Close();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public static string GenerateGUID()
		{
			return Guid.NewGuid().ToString();
		}
		public static bool IsValidEmail(string email)
		{
			return Regex.IsMatch(email, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
		}
		public static bool IsValidPhoneNumber(string phoneNumber)
		{
			return Regex.IsMatch(phoneNumber, @"^[0-9]{10}$");
		}
		public static bool IsValidDate(string date, string format)
		{
			DateTime tempDate;
			return DateTime.TryParseExact(date, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out tempDate);
		}
		public static bool IsValidTime(string time, string format)
		{
			DateTime tempTime;
			return DateTime.TryParseExact(time, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out tempTime);
		}
		public static bool IsNumeric(string input)
		{
			return double.TryParse(input, out _);
		}
		public static bool FileExists(string path)
		{
			return File.Exists(path);
		}
		public static bool DirectoryExists(string path)
		{
			return Directory.Exists(path);
		}
		public static int? TryParseInt(string input)
		{
			int result;
			if (int.TryParse(input, out result))
			{
				return result;
			}
			return null;
		}
		public static double? TryParseDouble(string input)
		{
			double result;
			if (double.TryParse(input, out result))
			{
				return result;
			}
			return null;
		}
		public static bool? TryParseBool(string input)
		{
			bool result;
			if (bool.TryParse(input, out result))
			{
				return result;
			}
			return null;
		}
		public static int GenerateRandomInt(int minValue, int maxValue)
		{
			Random random = new Random();
			return random.Next(minValue, maxValue);
		}
		public static byte[] ReadFileBytes(string filePath)
		{
			return File.ReadAllBytes(filePath);
		}
		public static void WriteBytesToFile(byte[] bytes, string filePath)
		{
			File.WriteAllBytes(filePath, bytes);
		}
		public static int GetMax(params int[] numbers)
		{
			if (numbers == null || numbers.Length == 0)
				throw new ArgumentException("The collection of numbers is empty.");
			return numbers.Max();
		}
		public static int GetMin(params int[] numbers)
		{
			if (numbers == null || numbers.Length == 0)
				throw new ArgumentException("The collection of numbers is empty.");
			return numbers.Min();
		}
		public static T GetRandomItem<T>(IEnumerable<T> collection)
		{
			if (collection == null || !collection.Any())
				throw new ArgumentException("The collection is empty.");

			Random rand = new Random();
			return collection.ElementAt(rand.Next(collection.Count()));
		}
		public static IEnumerable<T> Shuffle<T>(IEnumerable<T> collection)
		{
			if (collection == null)
				throw new ArgumentException("Input collection cannot be null.");

			List<T> list = collection.ToList();
			Random rand = new Random();
			int n = list.Count;
			while (n > 1)
			{
				n--;
				int k = rand.Next(n + 1);
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
			return list;
		}
		public static string[] SplitStringByLength(string input, int maxLength)
		{
			if (string.IsNullOrEmpty(input))
				return new string[] { input };

			return Enumerable.Range(0, input.Length / maxLength)
				.Select(i => input.Substring(i * maxLength, maxLength))
				.ToArray();
		}
		public static TimeSpan GetTimeDifference(DateTime start, DateTime end)
		{
			return end - start;
		}
		public static DateTime RoundDateTime(DateTime dateTime, TimeSpan interval)
		{
			long ticks = (dateTime.Ticks + (interval.Ticks / 2) + 1) / interval.Ticks;
			return new DateTime(ticks * interval.Ticks, dateTime.Kind);
		}
		public static string GenerateRandomString(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			return new string(Enumerable.Repeat(chars, length)
			  .Select(s => s[new Random().Next(s.Length)]).ToArray());
		}
		public static string HashString(string input)
		{
			using (SHA256 sha256Hash = SHA256.Create())
			{
				byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

				StringBuilder builder = new StringBuilder();
				for (int i = 0; i < bytes.Length; i++)
				{
					builder.Append(bytes[i].ToString("x2"));
				}
				return builder.ToString();
			}
		}
		public static string NumberWithCommas(int number)
		{
			return number.ToString("N0");
		}
		public static string CommaSeperateToNumber(string numberString)
		{
			string[] parts = numberString.Split(',');
			StringBuilder reversed = new StringBuilder();
			for (int i = parts.Length - 1; i >= 0; i--)
			{
				reversed.Append(parts[i]);
			}
			return reversed.ToString();
		}
		public static bool IsDateInRange(DateTime date, DateTime startDate, DateTime endDate)
		{
			return date >= startDate && date <= endDate;
		}
		public static double HourDifference(DateTime start, DateTime end)
		{
			TimeSpan timeDifference = end - start;
			return timeDifference.TotalHours;
		}
		public static bool IsDateInCurrentMonth(DateTime date)
		{
			return date.Month == DateTime.Today.Month && date.Year == DateTime.Today.Year;
		}
		public static bool IsValidURL(string url)
		{
			Uri uriResult;
			return Uri.TryCreate(url, UriKind.Absolute, out uriResult) && uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps;
		}
		public static bool IsValidIPAddress(string ip)
		{
			IPAddress ipAddress;
			return IPAddress.TryParse(ip, out ipAddress);
		}
		public static DateTime ConvertTimeZone(DateTime dateTime, string sourceTimeZone, string targetTimeZone)
		{
			if (string.IsNullOrEmpty(sourceTimeZone) || string.IsNullOrEmpty(targetTimeZone))
				throw new ArgumentException("Source and target time zones cannot be null or empty.");

			TimeZoneInfo sourceZone = TimeZoneInfo.FindSystemTimeZoneById(sourceTimeZone);
			TimeZoneInfo targetZone = TimeZoneInfo.FindSystemTimeZoneById(targetTimeZone);

			return TimeZoneInfo.ConvertTime(dateTime, sourceZone, targetZone);
		}
		public static string FormatString(string input, params object[] args)
		{
			return string.Format(input, args);
		}
		public static string RemoveHtmlTags(string input)
		{
			return Regex.Replace(input, "<.*?>", string.Empty);
		}
		public static bool IsCollectionNullOrEmpty<T>(IEnumerable<T> collection)
		{
			return collection == null || !collection.Any();
		}
		public static bool AreCollectionsEqual<T>(IEnumerable<T> first, IEnumerable<T> second)
		{
			return first.SequenceEqual(second);
		}
		public static IEnumerable<T> RemoveDuplicates<T>(IEnumerable<T> collection)
		{
			return collection.Distinct();
		}
		public static bool IsAlphaNumeric(string input)
		{
			return Regex.IsMatch(input, "^[a-zA-Z0-9]*$");
		}
		public static string SanitizeHtml(string html)
		{
			return Regex.Replace(html, "<.*?>", string.Empty);
		}
		public static byte[] ConvertStringToBytes(string input)
		{
			return Encoding.UTF8.GetBytes(input);
		}
		public static string ConvertBytesToString(byte[] bytes)
		{
			return Encoding.UTF8.GetString(bytes);
		}
		public static DateTime GenerateRandomDateTime(DateTime startDate, DateTime endDate)
		{
			int range = (endDate - startDate).Days;
			return startDate.AddDays(new Random().Next(range));
		}
		public static string CapitalizeWords(string input)
		{
			return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower());
		}
		public static double RoundToDecimals(double value, int decimalPlaces)
		{
			return Math.Round(value, decimalPlaces);
		}
		public static double MinuteToSecond(double minute)
		{
			return minute * 60.0;
		}
		public static double SecondToMinute(double second)
		{
			return second / 60.0;
		}
		public static double HourToSecond(double hour)
		{
			return hour * 3600.0;
		}
		public static double SecondToHour(double second)
		{
			return second / 3600.0;
		}
		public static double MeterToKilometer(double meter)
		{
			return meter / 1000.0;
		}
		public static double KilometerToMeter(double kilometer)
		{
			return kilometer * 1000.0;
		}
		public static double InchToMeter(double inch)
		{
			return inch * 0.0254;
		}
		public static double MeterToInch(double meter)
		{
			return meter / 0.0254;
		}
		public static double CelsiusToFahrenheit(double celsius)
		{
			return celsius * 9 / 5 + 32;
		}
		public static double FahrenheitToCelsius(double fahrenheit)
		{
			return (fahrenheit - 32) * 5 / 9;
		}
		public static double KilogramToGram(double kilogram)
		{
			return kilogram * 1000.0;
		}
		public static double GramToKilogram(double gram)
		{
			return gram / 1000.0;
		}
		public static double PoundToKilogram(double pound)
		{
			return pound * 0.453592;
		}
		public static double KilogramToPound(double kilogram)
		{
			return kilogram / 0.453592;
		}
		public static string GenerateRandomPassword(int length, bool includeSpecialChars = true, bool includeNumbers = true, bool includeUppercase = true)
		{
			string chars = "abcdefghijklmnopqrstuvwxyz";
			string specialChars = "!@#$%^&*()-_=+";
			string numbers = "0123456789";
			string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

			string validChars = chars;
			if (includeSpecialChars)
				validChars += specialChars;
			if (includeNumbers)
				validChars += numbers;
			if (includeUppercase)
				validChars += uppercase;

			Random random = new Random();
			return new string(Enumerable.Repeat(validChars, length)
			  .Select(s => s[random.Next(s.Length)]).ToArray());
		}
		public static string DecimalToBinary(int decimalNumber)
		{
			return Convert.ToString(decimalNumber, 2);
		}
		public static int BinaryToDecimal(string binaryNumber)
		{
			return Convert.ToInt32(binaryNumber, 2);
		}
		public static bool IsStrongPassword(string password)
		{
			return Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$");
		}
		public static string GenerateRandomGuid()
		{
			return Guid.NewGuid().ToString();
		}
		public static bool IsValidIPv4(string ipAddress)
		{
			return Regex.IsMatch(ipAddress, @"^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$");
		}
		public static bool IsValidIPv6(string ipAddress)
		{
			return Regex.IsMatch(ipAddress, @"^(([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,})$");
		}
		public static string ReadFileAsString(string filePath)
		{
			if (!File.Exists(filePath))
				throw new FileNotFoundException("File not found.", filePath);

			return File.ReadAllText(filePath);
		}
		public static IEnumerable<string> ReadFileLines(string filePath)
		{
			if (!File.Exists(filePath))
				throw new FileNotFoundException("File not found.", filePath);

			return File.ReadLines(filePath);
		}
		public static void AppendLineToFile(string content, string filePath)
		{
			File.AppendAllText(filePath, content + Environment.NewLine);
		}
		public static void DeleteFileX(string filePath)
		{
			if (File.Exists(filePath))
				File.Delete(filePath);
		}
		public static void CreateDirectory(string directoryPath)
		{
			if (!Directory.Exists(directoryPath))
				Directory.CreateDirectory(directoryPath);
		}
		public static void DeleteDirectory(string directoryPath, bool recursive = false)
		{
			if (Directory.Exists(directoryPath))
				Directory.Delete(directoryPath, recursive);
		}
		public static string GetBase64String(byte[] bytes)
		{
			return Convert.ToBase64String(bytes);
		}
		public static byte[] GetBytesFromBase64String(string base64String)
		{
			return Convert.FromBase64String(base64String);
		}
		public static string FormatCurrency(decimal amount, string cultureInfo = "en-US")
		{
			return string.Format(new CultureInfo(cultureInfo), "{0:C}", amount);
		}
		public static string SerializeToJson<T>(T obj)
		{
			return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
		}
		public static T DeserializeFromJson<T>(string json)
		{
			return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
		}
		public static string SerializeToXml<T>(T obj)
		{
			using (StringWriter stringWriter = new StringWriter())
			{
				XmlSerializer serializer = new XmlSerializer(typeof(T));
				serializer.Serialize(stringWriter, obj);
				return stringWriter.ToString();
			}
		}
		public static T DeserializeFromXml<T>(string xml)
		{
			using (StringReader stringReader = new StringReader(xml))
			{
				XmlSerializer serializer = new XmlSerializer(typeof(T));
				return (T)serializer.Deserialize(stringReader);
			}
		}
		public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
		{
			double R = 6371; // Radius of the Earth in km
			double dLat = (lat2 - lat1) * (Math.PI / 180);
			double dLon = (lon2 - lon1) * (Math.PI / 180);
			double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
					   Math.Cos(lat1 * (Math.PI / 180)) * Math.Cos(lat2 * (Math.PI / 180)) *
					   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
			double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
			double distance = R * c; // Distance in km
			return distance;
		}
		public static void DownloadFile(string url, string outputPath)
		{
			using (var client = new WebClient())
			{
				client.DownloadFile(url, outputPath);
			}
		}
		public static string ObjectToJson(object value)
		{
			return JsonConvert.SerializeObject(value);
		}
		public static T JsonToObject<T>(string jsonString)
		{
			return JsonConvert.DeserializeObject<T>(jsonString);
		}
		public static string BytesToString(byte[] bytes)
		{
			return Convert.ToBase64String(bytes);
		}

		#region file converter
		public void ConvertWordToPdf(string inputPath, string outputPath)
		{
			Aspose.Words.Document doc = new Aspose.Words.Document(inputPath);
			doc.Save(outputPath, Aspose.Words.SaveFormat.Pdf);
		}
		public void ConvertDocToPdf(string inputPath, string outputPath)
		{
			Aspose.Words.Document doc = new Aspose.Words.Document(inputPath);
			doc.Save(outputPath, Aspose.Words.SaveFormat.Pdf);
		}
		public void ConvertPdfToExcel(string inputPath, string outputPath)
		{
			Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document(inputPath);
			pdfDocument.Save(outputPath, Aspose.Pdf.SaveFormat.Excel);
		}
		public void ConvertExcelToPdf(string inputPath, string outputPath)
		{
			Workbook workbook = new Workbook(inputPath);
			workbook.Save(outputPath, Aspose.Cells.SaveFormat.Pdf);
		}
		public void ConvertDwgToDocx(string inputPath, string outputPath)
		{
			using (Aspose.CAD.Image cadImage = Aspose.CAD.Image.Load(inputPath))
			{
				string pdfOutput = Path.ChangeExtension(outputPath, ".pdf");

				PdfOptions pdfOptions = new PdfOptions();
				CadRasterizationOptions rasterizationOptions = new CadRasterizationOptions
				{
					PageWidth = 1600,
					PageHeight = 1600
				};
				pdfOptions.VectorRasterizationOptions = rasterizationOptions;
				cadImage.Save(pdfOutput, pdfOptions);
				Aspose.Words.Document pdfDocument = new Aspose.Words.Document(pdfOutput);
				pdfDocument.Save(outputPath, Aspose.Words.SaveFormat.Docx);
			}
		}
		public void ConvertDocToExcel(string inputPath, string outputPath)
		{
			Aspose.Words.Document doc = new Aspose.Words.Document(inputPath);
			MemoryStream stream = new MemoryStream();
			doc.Save(stream, Aspose.Words.SaveFormat.Html);

			Workbook workbook = new Workbook(stream);
			workbook.Save(outputPath, Aspose.Cells.SaveFormat.Xlsx);
		}
		public void ConvertPngToJpg(string inputPath, string outputPath)
		{
			using (System.Drawing.Image image = System.Drawing.Image.FromFile(inputPath))
			{
				image.Save(outputPath, System.Drawing.Imaging.ImageFormat.Jpeg);
			}
		}
		public void ConvertPngToJpeg(string inputPath, string outputPath)
		{
			using (System.Drawing.Image image = System.Drawing.Image.FromFile(inputPath))
			{
				image.Save(outputPath, System.Drawing.Imaging.ImageFormat.Jpeg);
			}
		}
		public void ConvertJpgToPng(string inputPath, string outputPath)
		{
			using (System.Drawing.Image image = System.Drawing.Image.FromFile(inputPath))
			{
				image.Save(outputPath, System.Drawing.Imaging.ImageFormat.Png);
			}
		}
		public void ConvertJpegToPng(string inputPath, string outputPath)
		{
			using (System.Drawing.Image image = System.Drawing.Image.FromFile(inputPath))
			{
				image.Save(outputPath, System.Drawing.Imaging.ImageFormat.Png);
			}
		}

		public static byte[] ConvertDocToPdf(byte[] docData)
		{
			using (MemoryStream docStream = new MemoryStream(docData))
			{
				Aspose.Words.Document doc = new Aspose.Words.Document(docStream);
				using (MemoryStream pdfStream = new MemoryStream())
				{
					doc.Save(pdfStream, Aspose.Words.SaveFormat.Pdf);
					return pdfStream.ToArray();
				}
			}
		}
		public static byte[] ConvertDocxToPdf(byte[] docxData)
		{
			using (MemoryStream docxStream = new MemoryStream(docxData))
			{
				Aspose.Words.Document docX = new Aspose.Words.Document(docxStream);
				using (MemoryStream pdfStream = new MemoryStream())
				{
					docX.Save(pdfStream, Aspose.Words.SaveFormat.Pdf);
					return pdfStream.ToArray();
				}
			}
		}
		public static byte[] ConvertWordToPdf(byte[] wordData)
		{
			using (MemoryStream worddStream = new MemoryStream(wordData))
			{
				Aspose.Words.Document word = new Aspose.Words.Document(worddStream);
				using (MemoryStream pdfStream = new MemoryStream())
				{
					word.Save(pdfStream, Aspose.Words.SaveFormat.Pdf);
					return pdfStream.ToArray();
				}
			}
		}
		public static byte[] ConvertExcelToPdf(byte[] excelData)
		{
			using (MemoryStream excelStream = new MemoryStream(excelData))
			{
				Workbook workbook = new Workbook(excelStream);
				using (MemoryStream pdfStream = new MemoryStream())
				{
					workbook.Save(pdfStream, Aspose.Cells.SaveFormat.Pdf);
					return pdfStream.ToArray();
				}
			}
		}
		public static byte[] ConvertPdfToDoc(byte[] pdfData)
		{
			using (MemoryStream pdfStream = new MemoryStream(pdfData))
			{
				Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document(pdfStream);
				using (MemoryStream docStream = new MemoryStream())
				{
					pdfDocument.Save(docStream, Aspose.Pdf.SaveFormat.Doc);
					return docStream.ToArray();
				}
			}
		}
		public static byte[] ConvertPdfToDocx(byte[] pdfData)
		{
			using (MemoryStream pdfStream = new MemoryStream(pdfData))
			{
				Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document(pdfStream);
				using (MemoryStream docxStream = new MemoryStream())
				{
					pdfDocument.Save(docxStream, Aspose.Pdf.SaveFormat.DocX);
					return docxStream.ToArray();
				}
			}
		}
		public static byte[] ConvertDocToDocx(byte[] docData)
		{
			using (MemoryStream docStream = new MemoryStream(docData))
			{
				Aspose.Words.Document doc = new Aspose.Words.Document(docStream);
				using (MemoryStream docxStream = new MemoryStream())
				{
					doc.Save(docxStream, Aspose.Words.SaveFormat.Docx);
					return docxStream.ToArray();
				}
			}
		}
		public static byte[] ConvertPdfToExcel(byte[] pdfData)
		{
			using (MemoryStream pdfStream = new MemoryStream(pdfData))
			{
				Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document(pdfStream);
				using (MemoryStream excelStream = new MemoryStream())
				{
					pdfDocument.Save(excelStream, Aspose.Pdf.SaveFormat.Excel);
					return excelStream.ToArray();
				}
			}
		}
		public static byte[] ConvertPdfToWord(byte[] pdfData)
		{
			using (MemoryStream pdfStream = new MemoryStream(pdfData))
			{
				Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document(pdfStream);
				using (MemoryStream wordStream = new MemoryStream())
				{
					pdfDocument.Save(wordStream, Aspose.Pdf.SaveFormat.Doc);
					return wordStream.ToArray();
				}
			}
		}
		public static byte[] ConvertPngToJpg(byte[] pngData)
		{
			using (MemoryStream pngStream = new MemoryStream(pngData))
			{
				System.Drawing.Image image = System.Drawing.Image.FromStream(pngStream);
				using (MemoryStream jpgStream = new MemoryStream())
				{
					image.Save(jpgStream, ImageFormat.Jpeg);
					return jpgStream.ToArray();
				}
			}
		}
		public static byte[] ConvertPngToJpeg(byte[] pngData)
		{
			using (MemoryStream pngStream = new MemoryStream(pngData))
			{
				System.Drawing.Image image = System.Drawing.Image.FromStream(pngStream);
				using (MemoryStream jpgStream = new MemoryStream())
				{
					image.Save(jpgStream, ImageFormat.Jpeg);
					return jpgStream.ToArray();
				}
			}
		}
		public static byte[] ConvertJpgToPng(byte[] jpgData)
		{
			using (MemoryStream jpgStream = new MemoryStream(jpgData))
			{
				System.Drawing.Image image = System.Drawing.Image.FromStream(jpgStream);
				using (MemoryStream pngStream = new MemoryStream())
				{
					image.Save(pngStream, ImageFormat.Png);
					return pngStream.ToArray();
				}
			}
		}
		public static byte[] ConvertJpegToPng(byte[] jpegData)
		{
			using (MemoryStream jpgeStream = new MemoryStream(jpegData))
			{
				System.Drawing.Image image = System.Drawing.Image.FromStream(jpgeStream);
				using (MemoryStream pngStream = new MemoryStream())
				{
					image.Save(pngStream, ImageFormat.Png);
					return pngStream.ToArray();
				}
			}
		}

		public Attachment ConvertDocToDocX(Attachment externalItem)
		{
			string[] items = externalItem.File.Split(new char[] { ',', ' ' }, StringSplitOptions.None);
			byte[] newBytes = Convert.FromBase64String(items[1]);

			externalItem.FileAsByteArray = newBytes;

			if (externalItem.FileAsByteArray is byte[] docData)
			{
				using (MemoryStream docStream = new MemoryStream(docData))
				{
					Aspose.Words.Document doc = new Aspose.Words.Document(docStream);
					using (MemoryStream docxStream = new MemoryStream())
					{
						doc.Save(docxStream, Aspose.Words.SaveFormat.Docx);
						externalItem.FileName = Path.ChangeExtension(externalItem.FileName, "docx");
						externalItem.FileAsByteArray = docxStream.ToArray();
						externalItem.PrevFileName = externalItem.FileName;
					}
				}
			}
			return externalItem;
		}
		public Attachment ConvertAttachment(Attachment externalItem, EnumAttachmentType targetType)
		{
			string[] items = externalItem.File.Split(new char[] { ',', ' ' }, StringSplitOptions.None);
			byte[] fileBytes = Convert.FromBase64String(items[1]);
			externalItem.FileAsByteArray = fileBytes;

			if (fileBytes != null)
			{
				using (MemoryStream inputStream = new MemoryStream(fileBytes))
				{
					switch (targetType)
					{
						case EnumAttachmentType.DOCX:
							if (externalItem.FileType == EnumAttachmentType.DOC)
							{
								ConvertDocToDocx(inputStream, externalItem);
							}
							break;
						case EnumAttachmentType.PDF:
							if (externalItem.FileType == EnumAttachmentType.DOC || externalItem.FileType == EnumAttachmentType.DOCX)
							{
								ConvertWordToPdf(inputStream, externalItem);
							}
							else if (externalItem.FileType == EnumAttachmentType.EXCEL)
							{
								ConvertExcelToPdf(inputStream, externalItem);
							}
							break;
						case EnumAttachmentType.EXCEL:
							if (externalItem.FileType == EnumAttachmentType.DOC || externalItem.FileType == EnumAttachmentType.DOCX)
							{
								ConvertDocToExcel(inputStream, externalItem);
							}
							else if (externalItem.FileType == EnumAttachmentType.PDF)
							{
								ConvertPdfToExcel(inputStream, externalItem);
							}
							break;
						case EnumAttachmentType.JPG:
						case EnumAttachmentType.JPEG:
							if (externalItem.FileType == EnumAttachmentType.PNG)
							{
								ConvertPngToJpeg(inputStream, externalItem);
							}
							break;
						case EnumAttachmentType.PNG:
							if (externalItem.FileType == EnumAttachmentType.JPG || externalItem.FileType == EnumAttachmentType.JPEG)
							{
								ConvertJpegToPng(inputStream, externalItem);
							}
							break;
						default:
							throw new NotSupportedException("Conversion type not supported.");
					}
				}
			}

			return externalItem;
		}
		private void ConvertDocToDocx(Stream inputStream, Attachment externalItem)
		{
			Aspose.Words.Document doc = new Aspose.Words.Document(inputStream);
			using (MemoryStream docxStream = new MemoryStream())
			{
				doc.Save(docxStream, Aspose.Words.SaveFormat.Docx);
				externalItem.FileName = Path.ChangeExtension(externalItem.FileName, "docx");
				externalItem.FileAsByteArray = docxStream.ToArray();
				externalItem.PrevFileName = externalItem.FileName;
			}
		}
		private void ConvertWordToPdf(Stream inputStream, Attachment externalItem)
		{
			Aspose.Words.Document doc = new Aspose.Words.Document(inputStream);
			using (MemoryStream pdfStream = new MemoryStream())
			{
				doc.Save(pdfStream, Aspose.Words.SaveFormat.Pdf);
				externalItem.FileName = Path.ChangeExtension(externalItem.FileName, "pdf");
				externalItem.FileAsByteArray = pdfStream.ToArray();
				externalItem.PrevFileName = externalItem.FileName;
			}
		}
		private void ConvertExcelToPdf(Stream inputStream, Attachment externalItem)
		{
			Workbook workbook = new Workbook(inputStream);
			using (MemoryStream pdfStream = new MemoryStream())
			{
				workbook.Save(pdfStream, Aspose.Cells.SaveFormat.Pdf);
				externalItem.FileName = Path.ChangeExtension(externalItem.FileName, "pdf");
				externalItem.FileAsByteArray = pdfStream.ToArray();
				externalItem.PrevFileName = externalItem.FileName;
			}
		}
		private void ConvertDocToExcel(Stream inputStream, Attachment externalItem)
		{
			Aspose.Words.Document doc = new Aspose.Words.Document(inputStream);
			using (MemoryStream htmlStream = new MemoryStream())
			{
				doc.Save(htmlStream, Aspose.Words.SaveFormat.Html);
				htmlStream.Position = 0;

				Workbook workbook = new Workbook(htmlStream);
				using (MemoryStream excelStream = new MemoryStream())
				{
					workbook.Save(excelStream, Aspose.Cells.SaveFormat.Xlsx);
					externalItem.FileName = Path.ChangeExtension(externalItem.FileName, "xlsx");
					externalItem.FileAsByteArray = excelStream.ToArray();
					externalItem.PrevFileName = externalItem.FileName;
				}
			}
		}
		private void ConvertPdfToExcel(Stream inputStream, Attachment externalItem)
		{
			string[] items = externalItem.File.Split(new char[] { ',', ' ' }, StringSplitOptions.None);
			byte[] fileBytes = Convert.FromBase64String(items[1]);
			externalItem.FileAsByteArray = fileBytes;

			if (fileBytes != null)
			{
				using (MemoryStream excelStream = new MemoryStream())
				{
					ConvertPdfToExcel(inputStream, excelStream);
					externalItem.FileName = Path.ChangeExtension(externalItem.FileName, "xlsx");
					externalItem.FileAsByteArray = excelStream.ToArray();
					externalItem.PrevFileName = externalItem.FileName;
				}
			}
		}
		private void ConvertPdfToExcel(Stream inputStream, Stream outputStream)
		{
			Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document(inputStream);
			ExcelSaveOptions saveOptions = new ExcelSaveOptions
			{
				Format = ExcelSaveOptions.ExcelFormat.XLSX
			};
			pdfDocument.Save(outputStream, saveOptions);
		}
		private void ConvertPngToJpeg(Stream inputStream, Attachment externalItem)
		{
			using (System.Drawing.Image image = System.Drawing.Image.FromStream(inputStream))
			{
				using (MemoryStream jpgStream = new MemoryStream())
				{
					image.Save(jpgStream, ImageFormat.Jpeg);
					externalItem.FileName = Path.ChangeExtension(externalItem.FileName, "jpg");
					externalItem.FileAsByteArray = jpgStream.ToArray();
					externalItem.PrevFileName = externalItem.FileName;
				}
			}
		}
		private void ConvertJpegToPng(Stream inputStream, Attachment externalItem)
		{
			using (System.Drawing.Image image = System.Drawing.Image.FromStream(inputStream))
			{
				using (MemoryStream pngStream = new MemoryStream())
				{
					image.Save(pngStream, ImageFormat.Png);
					externalItem.FileName = Path.ChangeExtension(externalItem.FileName, "png");
					externalItem.FileAsByteArray = pngStream.ToArray();
					externalItem.PrevFileName = externalItem.FileName;
				}
			}
		}
		public static void DataTableToExcel(DataTable dataTable, string filePath)
		{
			var file = new FileInfo(filePath);
			if (file.Directory != null && !file.Directory.Exists)
			{
				file.Directory.Create();
			}
			using (var package = new ExcelPackage())
			{
				var worksheet = package.Workbook.Worksheets.Add("Sheet1");
				worksheet.Cells["A1"].LoadFromDataTable(dataTable, true);
				using (var range = worksheet.Cells[1, 1, 1, dataTable.Columns.Count])
				{
					range.Style.Font.Bold = true;
					range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
					range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
				}
				worksheet.Cells.AutoFitColumns(0);
				package.SaveAs(file);
			}
		}
		public static byte[] DataTableToExcelBytes(DataTable dataTable)
		{
			using (var package = new ExcelPackage())
			{
				var worksheet = package.Workbook.Worksheets.Add("Sheet1");
				worksheet.Cells["A1"].LoadFromDataTable(dataTable, true);
				using (var range = worksheet.Cells[1, 1, 1, dataTable.Columns.Count])
				{
					range.Style.Font.Bold = true;
					range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
					range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
				}
				worksheet.Cells.AutoFitColumns(0);
				using (var stream = new MemoryStream())
				{
					package.SaveAs(stream);
					return stream.ToArray();
				}
			}
		}
		public static DataTable ExcelToDataTable(byte[] excelBytes)
		{
			using (var stream = new MemoryStream(excelBytes))
			using (var package = new ExcelPackage(stream))
			{
				var worksheet = package.Workbook.Worksheets[0];
				var dataTable = new DataTable();

				foreach (var firstRowCell in worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column])
				{
					dataTable.Columns.Add(firstRowCell.Text);
				}
				for (int rowNum = 2; rowNum <= worksheet.Dimension.End.Row; rowNum++)
				{
					var wsRow = worksheet.Cells[rowNum, 1, rowNum, worksheet.Dimension.End.Column];
					var row = dataTable.NewRow();
					foreach (var cell in wsRow)
					{
						row[cell.Start.Column - 1] = cell.Text;
					}
					dataTable.Rows.Add(row);
				}
				return dataTable;
			}
		}
		//byte[] excelBytes = ExcelHelper.DataTableToExcelBytes(dataTable);
		//var filePath = @"C:\path\to\your\file.xlsx";
		//File.WriteAllBytes(filePath, excelBytes);
		#endregion
	}
}
