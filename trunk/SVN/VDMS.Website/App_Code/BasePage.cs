using System;
using System.Collections;
using System.Globalization;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using VDMS.Common.Utils;
using VDMS.II.Security;
using VDMS.Provider;
using System.Web;
using VDMS.Helper;

/// <summary>
/// Summary description for BasePage
/// </summary>
public class BasePage : Page
{

    #region Control States

    private Hashtable ControlStates;

    protected void Page_Init(object sender, EventArgs e)
    {
        this.Page.RegisterRequiresControlState(this);
    }

    protected override void LoadControlState(object savedState)
    {
        object[] ctlState = (object[])savedState;
        base.LoadControlState(ctlState[0]);
        this.ControlStates = (Hashtable)ctlState[1];
    }

    protected override object SaveControlState()
    {
        object[] ctlState = new object[2];
        ctlState[0] = base.SaveControlState();
        ctlState[1] = this.ControlStates;
        return ctlState;
    }

    public void SaveState(object key, object value)
    {
        if (ControlStates == null) ControlStates = new Hashtable();

        if (this.ControlStates.ContainsKey(key))
        {
            this.ControlStates[key] = value;
        }
        else
        {
            this.ControlStates.Add(key, value);
        }
    }

    public object LoadState(object key)
    {
        if (this.ControlStates != null && this.ControlStates.Contains(key)) return this.ControlStates[key];
        return null;
    }

    #endregion

    #region Eval Data

    public string EvalDate(object date)
    {
        return DataFormat.ToDateString(date);
    }
    public string EvalDateTime(object date)
    {
        return DataFormat.ToDateTimeString(date);
    }
    public string EvalNumber(object data, int n)
    {
        string fm = string.Format("N{0}", n);
        try { return ((double)data).ToString(fm); }
        catch
        {
            try { return ((long)data).ToString(fm); }
            catch
            {
                try { return ((decimal)data).ToString(fm); }
                catch
                {
                    try { return ((int)data).ToString(fm); }
                    catch { return ""; }
                }
            }
        }
    }
    public string EvalNumber(object data)
    {
        return EvalNumber(data, 0);
    }

    #endregion

    protected const string DeleteConfirmQuestion = "if (!confirm(SysMsg[0])) return false;";
    protected const string SaveConfirmQuestion = "if (!confirm(SysMsg[1])) return false;";
    protected const string DoActionQuestion = "if (!confirm(SysMsg[4])) return false;";

    public string CreateConfirmScript(string Question)
    {
        return string.Format("if (confirm('{0}')==false) return false;", Question);
    }

    protected override void InitializeCulture()
    {
        SetCulture(VDMSProvider.Language);
        base.InitializeCulture();
    }

    void HandleButton(Control control)
    {
        if ((control is IButtonControl) && ((IButtonControl)control).CommandName != "Cancel" && ((IButtonControl)control).CommandName != "Page" && (control as WebControl).Enabled)
        {
            (control as WebControl).Enabled = !Security.IsDeniedTask(Page.Request.Url.AbsolutePath, ((IButtonControl)control).CommandName);
        }
        foreach (Control c in control.Controls)
            HandleButton(c);
    }

    void BasePage_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(((Button)sender).CommandName))
            TestPermissionOfTask(((Button)sender).CommandName);
    }

    protected override void OnInit(EventArgs e)
    {
        HttpContext.Current.Session["CurrentUser.DatabaseCode"] = (UserHelper.ApplicationName == "/") ? UserHelper.Profile.DatabaseCode : UserHelper.Dealer.DatabaseCode;
        if(HttpContext.Current.Session["IsDeniedURLWithProvider"] == null)
            HttpContext.Current.Session["IsDeniedURLWithProvider"] = Security.IsDeniedURLWithProvider(Page.Request.Url.AbsolutePath, UserHelper.ApplicationName, null);
        base.OnInit(e);
    }

    Devart.Data.Oracle.OracleMonitor myMonitor = new Devart.Data.Oracle.OracleMonitor();
    protected override void OnLoad(EventArgs e)
    {
        myMonitor.IsActive = true;
        base.OnLoad(e);
        HandleButton(this);
    }

    void TestPermissionOfTask(string TaskName)
    {
        if (Security.IsDeniedTask(Page.Request.Url.AbsolutePath, TaskName.ToLower())) Response.Redirect("~/AccessDeny.aspx");
    }

    protected void SetCulture(string name)
    {
        try
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(name);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(name);
        }
        catch { }
    }

    protected void ReLoad()
    {
        Response.Redirect(Page.Request.Url.AbsoluteUri);
    }

    public void EnableControl(Control c, bool en)
    {
        foreach (var ctrl in c.Controls)
        {

        }
    }

    protected void DisableButton()
    {
        foreach (Control c in this.Page.Controls)
            DisableButton(c);
    }

    void DisableButton(Control control)
    {
        if (control.GetType() == typeof(Button) && ((Button)control).CommandName != "Cancel") ((Button)control).Enabled = false;
        foreach (Control c in control.Controls)
            DisableButton(c);
    }

    public void SetOpenForm(Button btn, string url)
    {
        if (btn != null) btn.OnClientClick = string.Format("javascript:window.location = '{0}'; return false;", url);
    }

    public void InitDefaultEnterButton(WebControl ctrl, WebControl btn)
    {
        ctrl.Attributes["onkeydown"] = "javascript:if((event.which && event.which == 13) || (event.keyCode && event.keyCode == 13)){document.getElementById('" + btn.ClientID + "').click();return false;}else return true;";
    }
    public void RemoveDefaultEnterButton(WebControl ctrl)
    {
        ctrl.Attributes["onkeydown"] = "javascript:if((event.which && event.which == 13) || (event.keyCode && event.keyCode == 13)){ return false; }else return true;";
    }

    public void ScanControl<T, CT>(Control startCtrl, Action<T> act) where T : WebControl
    {
        if (startCtrl is CT) act((T)startCtrl);
        foreach (Control item in startCtrl.Controls)
        {
            ScanControl<T, CT>(item, act);
        }
    }

    protected void ResetControl(Control control)
    {
        if (control.GetType() == typeof(TextBox)) ((TextBox)control).Text = string.Empty;
        foreach (Control c in control.Controls)
            ResetControl(c);
    }

    //protected void LockControl(Control control)
    //{
    //    if (control.GetType() == typeof(TextBox)) ((TextBox)control).Text = string.Empty;
    //    foreach (Control c in control.Controls)
    //        ResetControl(c);
    //}

    protected void GoHome()
    {
        Response.Redirect("~/default.aspx");
    }

    #region show errors

    private BulletedList _errorList;
    private BulletedList _infoList;
    protected BulletedList blErrorList
    {
        get
        {
            if (_errorList == null)
            {
                _errorList = new BulletedList();
                _errorList.CssClass = "error";
                _errorList.ID = Guid.NewGuid().ToString();
            }
            return _errorList;
        }
    }
    protected BulletedList blInfoList
    {
        get
        {
            if (_infoList == null)
            {
                _infoList = new BulletedList();
                _infoList.CssClass = "normalInfo";
                _infoList.ID = Guid.NewGuid().ToString();
            }
            return _infoList;
        }
    }

    protected void InitErrMsgControl(Control container)
    {
        this.InitErrMsgControl(container, 0);
    }
    protected void InitErrMsgControl(Control container, int position)
    {
        if (container.FindControl(blErrorList.ID) == null)
        {
            container.Controls.AddAt(position, blErrorList);
        }
        blErrorList.Items.Clear();
    }

    protected void InitInfoMsgControl(Control container)
    {
        this.InitInfoMsgControl(container, 0);
    }

    protected void InitInfoMsgControl(Control container, int position)
    {
        if (container.FindControl(blInfoList.ID) == null)
        {
            container.Controls.AddAt(position, blInfoList);
        }
        blInfoList.Items.Clear();
    }

    protected void AddErrorMsg(string msg)
    {
        blErrorList.Items.Add(msg);
    }

    protected void AddInfoMsg(string msg)
    {
        blInfoList.Items.Add(msg);
    }
    #endregion
}
