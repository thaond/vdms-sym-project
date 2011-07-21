<%@ WebHandler Language="C#" Class="download" %>

using System;
using System.Linq;
using System.Web;
using VDMS.II.Linq;
using VDMS.II.Entity;

public class download : IHttpHandler
{
	public void ProcessRequest(HttpContext context)
	{
		long Id = 0;
		try
		{
			Id = long.Parse(context.Request.QueryString[0]);
		}
		catch { }

		var db = DCFactory.GetDataContext<BasicDataContext>();
		File f = db.Files.SingleOrDefault(p => p.FileId == Id);
		if (f == null) return;

		context.Response.AddHeader("Content-Disposition", "attachment; filename=" + new System.IO.FileInfo(f.FileName.Replace(' ', '_')).Name);
		context.Response.ContentType = VDMS.Helper.FileHelper.GetContentTypeOfFile(f.FileName);

		try
		{
			byte[] r = VDMS.Common.Utils.Compressor.Decompress(f.Body);
			context.Response.BinaryWrite(r);
		}
		catch { }
	}

	public bool IsReusable
	{
		get
		{
			return false;
		}
	}
}