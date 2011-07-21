using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CSSFriendly
{
	/// <summary>
	/// Overrides the default table layout of the <see cref="System.Web.UI.WebControls.CheckBoxList"/>
	/// control, to a XHTML unordered list layout (structural markup).
	/// </summary>
	public class CheckBoxListAdapter : System.Web.UI.WebControls.Adapters.WebControlAdapter
	{
		private const string WRAPPER_CSS_CLASS = "AspNet-CheckBoxList";
		private const string ITEM_CSS_CLASS = "AspNet-CheckBoxList-Item";
		private const string DISABLED_CSS_CLASS = "AspNet-CheckBoxList-Disabled";
		private const string REPEATDIRECTION_CSS_CLASS = "AspNet-CheckBoxList-RepeatDirection-";

		protected override void RenderBeginTag(HtmlTextWriter writer)
		{
			// Div
			writer.AddAttribute(HtmlTextWriterAttribute.Class, WRAPPER_CSS_CLASS);
			writer.AddAttribute(HtmlTextWriterAttribute.Id, this.Control.ClientID);
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
			writer.Indent++;

			// Ul
			string cssClass = "";
			if (!string.IsNullOrEmpty(this.Control.CssClass))
			{
				cssClass = this.Control.CssClass;
			}

			CheckBoxList checkList = this.Control as CheckBoxList;
			if (checkList != null)
			{
				cssClass += " " + REPEATDIRECTION_CSS_CLASS + checkList.RepeatDirection.ToString();
			}

			writer.AddAttribute(HtmlTextWriterAttribute.Class, cssClass.Trim());

			writer.RenderBeginTag(HtmlTextWriterTag.Ul);
		}

		protected override void RenderEndTag(HtmlTextWriter writer)
		{
			writer.RenderEndTag(); // Ul
			writer.Indent--;
			writer.RenderEndTag(); // Div
		}

		protected override void RenderContents(HtmlTextWriter writer)
		{
			CheckBoxList checkList = this.Control as CheckBoxList;
			if (checkList != null)
			{
				foreach (ListItem li in checkList.Items)
				{
					string itemClientID = Helpers.GetListItemClientID(checkList, li);

					// Li
					string cssClass = ITEM_CSS_CLASS;
					if (li.Enabled == false || checkList.Enabled == false)
					{
						cssClass += " " + DISABLED_CSS_CLASS;
					}
					writer.AddAttribute(HtmlTextWriterAttribute.Class, cssClass);
					writer.RenderBeginTag(HtmlTextWriterTag.Li);

					if (checkList.TextAlign == TextAlign.Right)
					{
						RenderCheckBoxListInput(writer, checkList, li);
						RenderCheckBoxListLabel(writer, checkList, li);
					}
					else // TextAlign.Left
					{
						RenderCheckBoxListLabel(writer, checkList, li);
						RenderCheckBoxListInput(writer, checkList, li);
					}

					writer.RenderEndTag(); // </li>
					writer.WriteLine();

					if (this.Page != null)
					{
						Page.ClientScript.RegisterForEventValidation(checkList.UniqueID, li.Value);
					}
				}

				if (this.Page != null)
				{
					Page.ClientScript.RegisterForEventValidation(checkList.UniqueID);
				}
			}
		}

		private void RenderCheckBoxListInput(HtmlTextWriter writer, CheckBoxList checkList, ListItem li)
		{
			// Input
			writer.AddAttribute(HtmlTextWriterAttribute.Id, Helpers.GetListItemClientID(checkList, li));
			writer.AddAttribute(HtmlTextWriterAttribute.Type, "checkbox");
			writer.AddAttribute(HtmlTextWriterAttribute.Name, Helpers.GetListItemUniqueID(checkList, li));
			writer.AddAttribute(HtmlTextWriterAttribute.Value, li.Value);
			if (li.Selected)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Checked, "checked");
			}
			if (li.Enabled == false || checkList.Enabled == false)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
			}
			if (li.Enabled == true && checkList.Enabled == true && checkList.AutoPostBack)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Onclick,
					String.Format(@"setTimeout('__doPostBack(\'{0}\',\'\')', 0)",
						Helpers.GetListItemUniqueID(checkList, li)));
			}
			if (checkList.TabIndex != 0)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Tabindex, checkList.TabIndex.ToString());
			}

			writer.RenderBeginTag(HtmlTextWriterTag.Input);
			writer.RenderEndTag(); // </input>
		}

		private void RenderCheckBoxListLabel(HtmlTextWriter writer, CheckBoxList checkList, ListItem li)
		{
			// Label
			writer.AddAttribute("for", Helpers.GetListItemClientID(checkList, li));
			writer.RenderBeginTag(HtmlTextWriterTag.Label);
			writer.Write(li.Text);
			writer.RenderEndTag(); // </label>
		}
	}
}