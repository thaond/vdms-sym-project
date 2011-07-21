using System.Configuration;
using System.IO;
using System.Web;

namespace VDMS.Helper
{
	public class FileHelper
	{
		public static bool WriteToFileText(string Filename, string Content, bool IsAppend)
		{
			return WriteToFileText(ConfigurationManager.AppSettings["CurrentDir"], Filename, Content, IsAppend, false);
		}

		public static bool WriteLineToFileText(string Filename, string Content, bool IsAppend)
		{
			return WriteToFileText(ConfigurationManager.AppSettings["CurrentDir"], Filename, Content, IsAppend, true);
		}

		/// <summary>
		/// Write to the file text
		/// </summary>
		/// <param name="Filename"></param>
		/// <param name="Content"></param>
		/// <param name="IsAppend"></param>
		/// <returns></returns>
		public static bool WriteToFileText(string path, string Filename, string Content, bool IsAppend)
		{
			return WriteToFileText(path, Filename, Content, IsAppend, false);
		}

		public static bool WriteLineToFileText(string path, string Filename, string Content, bool IsAppend)
		{
			return WriteToFileText(path, Filename, Content, IsAppend, true);
		}

		private static bool WriteToFileText(string path, string Filename, string Content, bool IsAppend, bool appEnter)
		{
			bool result = true;
			StreamWriter myStreamWriter = null;
			try
			{
				//var FilePath = Path.Combine(ConfigurationManager.AppSettings["CurrentDir"], Filename);
				var FilePath = Path.Combine(path, Filename);
				if (IsAppend) myStreamWriter = File.AppendText(FilePath);
				else myStreamWriter = File.CreateText(FilePath);

				if (appEnter) myStreamWriter.WriteLine(Content);
				else myStreamWriter.Write(Content);
				myStreamWriter.Flush();
			}
			catch
			{
				result = false;
			}
			finally
			{
				if (myStreamWriter != null) myStreamWriter.Close();
			}
			return result;
		}

		/// <summary>
		/// Read the content of file text
		/// </summary>
		/// <param name="Filename">The filename, path relative</param>
		/// <returns>The string of content</returns>
		public static string ReadFileTextContent(string Filename)
		{
			StreamReader myStreamReader = null;
			var FilePath = Path.Combine(ConfigurationManager.AppSettings["CurrentDir"], Filename);
			var result = string.Empty;
			try
			{
				myStreamReader = File.OpenText(FilePath);
				result = myStreamReader.ReadToEnd();
			}
			catch
			{
			}
			finally
			{
				if (myStreamReader != null) myStreamReader.Close();
			}
			return result;
		}

		/// <summary>
		/// Get the type of content of file
		/// </summary>
		/// <param name="FileName"></param>
		/// <returns></returns>
		public static string GetContentTypeOfFile(string FileName)
		{
			FileInfo fileinfo = null;
			try
			{
				fileinfo = new FileInfo(FileName);
			}
			catch
			{
				return "application/octet-stream";
			}
			switch (fileinfo.Extension)
			{
				case ".txt":
					return "text/plain";
				case ".htm":
				case ".html":
					return "text/html";
				case ".rtf":
					return "text/richtext";
				case ".jpg":
				case ".jpeg":
					return "image/jpeg";
				case ".gif":
					return "image/gif";
				case ".bmp":
					return "image/bmp";
				case ".mpg":
				case ".mpeg":
					return "video/mpeg";
				case ".avi":
					return "video/avi";
				case ".pdf":
					return "application/pdf";
				case ".doc":
				case ".dot":
					return "application/msword";
				case ".csv":
				case ".xls":
				case ".xlt":
					return "application/vnd.msexcel";
				default:
					return "application/octet-stream";
			}
		}
	}
}