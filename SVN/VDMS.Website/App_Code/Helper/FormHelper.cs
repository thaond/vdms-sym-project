using System.Web.UI;
using System.Web.UI.WebControls;

namespace VDMS.Helper
{
	public class FormHelper
	{
		public static void ResetControl(Control control)
		{
			if (control.GetType() == typeof(TextBox)) ((TextBox)control).Text = string.Empty;
			foreach (Control c in control.Controls)
				ResetControl(c);
		}
	}
}