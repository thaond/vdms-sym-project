using System;
using VDMS.II.Entity;

public partial class Controls_ContactInfo : System.Web.UI.UserControl
{
    public Contact Contact { get; private set; }
    public bool InViewMode { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack && Contact != null)
        {
            tb1.Text = Contact.FullName;
            tb2.Text = Contact.Address;
            tb3.Text = Contact.Phone;
            tb4.Text = Contact.Email;
            tb5.Text = Contact.AdditionalInfo;
            txtFax.Text = Contact.Fax;
            txtMobile.Text = Contact.Mobile;
        }
    }

    public void LoadInfo(Contact contact)
    {
        Contact = contact;
        if (contact != null)
        {
            tb1.Text = lb1.Text = contact.FullName;
            tb2.Text = lb2.Text = contact.Address;
            tb3.Text = lb3.Text = contact.Phone;
            tb4.Text = lb4.Text = contact.Email;
            tb5.Text = lb5.Text = contact.AdditionalInfo;
            txtFax.Text = lbFax.Text = Contact.Fax;
            txtMobile.Text = lbMobile.Text = Contact.Mobile;
        }
        if (InViewMode) MultiView1.ActiveViewIndex = MultiView2.ActiveViewIndex = mv1.ActiveViewIndex = mv2.ActiveViewIndex = mv3.ActiveViewIndex = mv4.ActiveViewIndex = mv5.ActiveViewIndex = 1;
    }

    public Contact GetInfo()
    {
        return Contact = new Contact()
            {
                FullName = tb1.Text.Trim() == string.Empty ? null : tb1.Text.Trim(),
                Address = tb2.Text.Trim() == string.Empty ? null : tb2.Text.Trim(),
                Phone = tb3.Text.Trim() == string.Empty ? null : tb3.Text.Trim(),
                Email = tb4.Text.Trim() == string.Empty ? null : tb4.Text.Trim(),
                AdditionalInfo = tb5.Text.Trim() == string.Empty ? null : tb5.Text.Trim(),
                Fax = txtFax.Text.Trim(),
                Mobile = txtMobile.Text.Trim(),
            };
    }

    public void GetInfo(Contact c)
    {
        c.FullName = tb1.Text.Trim() == string.Empty ? null : tb1.Text.Trim();
        c.Address = tb2.Text.Trim() == string.Empty ? null : tb2.Text.Trim();
        c.Phone = tb3.Text.Trim() == string.Empty ? null : tb3.Text.Trim();
        c.Email = tb4.Text.Trim() == string.Empty ? null : tb4.Text.Trim();
        c.AdditionalInfo = tb5.Text.Trim() == string.Empty ? null : tb5.Text.Trim();
        c.Fax = txtFax.Text.Trim();
        c.Mobile = txtMobile.Text.Trim();
    }
}
