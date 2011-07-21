using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.II.Entity;
using VDMS.II.Linq;

public partial class ShowMessage : BasePage
{

    public static Message message { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            message = VDMS.II.BasicData.MessageDAO.GetById(Convert.ToInt32(Request.QueryString["id"]));
            repAttachment.DataSource = GetAttachment(message.MessageId);
            repAttachment.DataBind();
        }

    }
    protected List<TempFile> GetAttachment(long MessageId)
    {

        var db = DCFactory.GetDataContext<BasicDataContext>();
        var query = (from file in db.Files
                   where file.MessageId == MessageId
                   select new TempFile
                   {
                       FileId = file.FileId,
                       FileName = file.FileName,
                       MessageId = file.MessageId
                   }).ToList();

        return query;
    }

    protected class TempFile
    {
        public long MessageId { get; set; }
        public long FileId { get; set; }
        public string FileName { get; set; }
    }
}
