using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Collections;
using System.Web.UI;
using VDMS.Common.Web.JavaScript;
using VDMS.Common.Web;

namespace VDMS
{
    namespace Common.Web.CustomControl
    {
        /// <summary>
        /// Like RequiredFieldValidator, but applied for list items/databound control 
        /// to require control has user input (typed in or select ....) for at least one item.
        /// Now pupported: 
        ///     1. Check box list
        ///     2. Databound control with child controls to check(gridview,...)
        ///     3. List of controls(textbox, combobox, literal/label -> validate text, checkbox), can be place in databound control
        /// Note: System has two type of validate
        ///     1. Normal state required(has: Text/Checked/SelectedValue)
        ///     2. Revert state required(has: Empty Text/Un Checked/Empty SelectedValue)
        /// Usage:
        ///     1. Setup [ControlToValidate] property
        ///     2. With validate type:
        ///         a. List of controls: Set these properties(separate control by ",")
        ///             i.  ListControlsToValidate:       for normal state required
        ///             ii. ListControlsToRevertValidate: for revert state required
        ///         b. CheckBoxList/DataBound controls: handle event [OnIsRevert]
        ///             i.  Return "true" for revert state required
        ///             ii. Return "false" for normal state required (this is default value)
        ///     3. Set [EnableClientScript] to validate on client
        ///     4. For ListItemControl set ValidateEmptyList=true(default is false) to assume control is invalid if it has no items.
        /// Edit ChildControlIsValid() to support more controls
        /// </summary>
        public class RequiredOneItemValidator : CustomValidator
        {
            public RequiredOneItemValidator()
            {
                this.ValidateEmptyList = false;
                this.ValidateEmptyText = true;
            }

            #region Private member

            private Control _targetControl = null;

            #endregion

            #region New Property

            public event VDMS.Common.Web.WebTools.ItemValidator OnIsRevert;

            /// <summary>
            /// List of controls to validate, separate by ","
            /// </summary>
            public string ListControlsToValidate { get; set; }
            /// <summary>
            /// List of controls to validate, separate by ","
            /// </summary>
            public string ListControlsToRevertValidate { get; set; }

            /// <summary>
            /// If true validator is invalid if no items found in validated control
            /// Default is set to false in constructor
            /// </summary>
            public bool ValidateEmptyList { get; set; }

            /// <summary>
            /// For databound control this store control ID as child item to validate
            /// Such as textbox or checkbox.
            /// </summary>
            public string ChildControlToValidate { get; set; }

            /// <summary>
            /// Return ControlToValidate as Control
            /// </summary>
            protected Control TargetControl
            {
                get
                {
                    if (_targetControl == null) _targetControl = this.FindControl(this.ControlToValidate);
                    return _targetControl;
                }
            }

            #endregion

            #region Overrided method

            protected override bool ControlPropertiesValid()
            {
                return true; // accept all :D
            }

            protected override void OnPreRender(EventArgs e)
            {
                this.ValidateEmptyText = true;
                if (this.EnableClientScript)
                {
                    this.RegisterClientScript();
                }
                base.OnPreRender(e);

            }

            protected override bool EvaluateIsValid()
            {
                bool result = true;
                if (!string.IsNullOrEmpty(ListControlsToValidate) || !string.IsNullOrEmpty(ListControlsToRevertValidate))
                {
                    result = EvaluateListControls();
                }
                else
                    if (this.TargetControl is CheckBoxList)
                    {
                        result = this.EvaluateCheckBoxList();
                    }
                    else
                    {
                        result = this.EvaluateDataBoundControl();
                    }

                return result;
            }

            #endregion

            #region New Protect mothod

            protected bool IsRevert(object c)
            {
                return (OnIsRevert != null) ? OnIsRevert(c) : false;
            }

            /// <summary>
            /// Determine whether one control has user input in or not
            /// Used to support EvaluateDataBoundControl() function
            /// </summary>
            /// <param name="ctrl"></param>
            /// <returns></returns>
            protected bool ChildControlIsValid(Control ctrl, bool revert)
            {
                bool result = false;
                if (ctrl is DropDownList)
                {
                    result = !string.IsNullOrEmpty((ctrl as DropDownList).SelectedValue);
                }
                if (ctrl is TextBox)
                {
                    result = !string.IsNullOrEmpty((ctrl as TextBox).Text.Trim());
                }
                else if (ctrl is CheckBox)
                {
                    result = (ctrl as CheckBox).Checked;
                }
                else if (ctrl is Literal || ctrl is Label)
                {
                    result = !string.IsNullOrEmpty((ctrl as ITextControl).Text.Trim());
                }

                return revert ? !result : result;
            }

            /// <summary>
            /// For Validate controls set by ListControlsToValidate
            /// </summary>
            /// <returns></returns>
            protected ArrayList GetControlsByList(string list)
            {
                ArrayList childs = new ArrayList();

                foreach (string item in list.Split(','))
                {
                    var c = this.NamingContainer;
                    var ctrl = c.FindControl(item);
                    if (ctrl == null) ctrl = WebTools.FindControlById(item, this.Page);
                    if (ctrl != null) childs.Add(ctrl);
                }

                return childs;
            }
            //protected ArrayList GetControlsByList()
            //{
            //    var res = GetControlsByList(ListControlsToValidate);

            //    res.AddRange(GetControlsByList(ListControlsToRevertValidate));
            //    return res;
            //}

            /// <summary>
            /// Get all child items to validate in a DataboundControl
            /// It will throw an Exception if ControlToValid not found
            /// </summary>
            /// <returns></returns>
            protected ArrayList GetDataBoundControlChildren(bool? revert)
            {
                if (this.TargetControl == null)
                {
                    throw new Exception(string.Format("Cannot find ControlToValidate ({0}) of {1}.", this.ControlToValidate, this.ID));
                }
                ArrayList childs = new ArrayList();
                if (revert.HasValue)
                    WebTools.FindControls(this.ChildControlToValidate, this.TargetControl, ref childs, delegate(object c) { return IsRevert(c) == revert.Value; });
                else
                    WebTools.FindControls(this.ChildControlToValidate, this.TargetControl, ref childs);

                return childs;
            }

            /// <summary>
            /// Evaluate function when ControlToValidate is CheckBoxList
            /// Used to support EvaluateIsValid() function
            /// </summary>
            /// <returns>True if at least one item has been checked</returns>
            protected bool EvaluateCheckBoxList()
            {
                ListItemCollection items = (this.TargetControl as CheckBoxList).Items;
                if ((!this.ValidateEmptyList) && (items.Count == 0)) return true;

                foreach (ListItem item in items)
                {
                    if (item.Selected || (IsRevert(item) && !item.Selected)) return true;
                }
                return false;
            }

            /// <summary>
            /// Evaluate function when ControlToValidate is DataBoundControl
            /// Used to support EvaluateIsValid() function 
            /// </summary>
            /// <returns>True if at least one item has user input</returns>
            protected bool EvaluateDataBoundControl()
            {
                ArrayList childs = this.GetDataBoundControlChildren(null);
                if ((!this.ValidateEmptyList) && (childs.Count == 0)) return true;

                foreach (Control child in childs)
                {
                    if (this.ChildControlIsValid(child, IsRevert(child))) return true;
                }

                return false;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            protected bool EvaluateListControls()
            {
                return EvaluateListControls(ListControlsToValidate, false) || EvaluateListControls(ListControlsToRevertValidate, true);
            }
            protected bool EvaluateListControls(string list, bool revert)
            {
                ArrayList childs = this.GetControlsByList(list);
                if ((!this.ValidateEmptyList) && (childs.Count == 0)) return true;

                foreach (Control child in childs)
                {
                    if (this.ChildControlIsValid(child, revert)) return true;
                }

                return false;
            }

            /// <summary>
            /// Build list of child items's ID on client
            /// </summary>
            /// <returns></returns>
            protected List<string> GetClientNamesOfChildren()
            {
                if (!string.IsNullOrEmpty(ListControlsToValidate))
                {
                    return GetClientNameOfListControls(ListControlsToValidate);
                }
                else
                {
                    if (this.TargetControl is CheckBoxList)
                    {
                        return this.GetClientNamesOfCheckBoxListChildren(false);
                    }
                    else
                    {
                        return this.GetClientNamesOfDataBoundControlChildren(false);
                    }
                }
            }
            protected List<string> GetClientNamesOfRevertChildren()
            {
                if (!string.IsNullOrEmpty(ListControlsToRevertValidate))
                {
                    return GetClientNameOfListControls(ListControlsToRevertValidate);
                }
                else
                {
                    if (this.TargetControl is CheckBoxList)
                    {
                        return this.GetClientNamesOfCheckBoxListChildren(true);
                    }
                    else
                    {
                        return this.GetClientNamesOfDataBoundControlChildren(true);
                    }
                }
            }

            /// <summary>
            /// Build list of child controls's ID on client
            /// </summary>
            /// <returns></returns>
            protected List<string> GetClientNamesOfDataBoundControlChildren(bool revert)
            {
                List<string> names = new List<string>();
                foreach (Control child in this.GetDataBoundControlChildren(revert))
                {
                    names.Add(child.ClientID);
                }
                return names;
            }

            /// <summary>
            /// Build list of all checkbox ID on client
            /// </summary>
            /// <returns></returns>
            protected List<string> GetClientNamesOfCheckBoxListChildren(bool revert)
            {
                List<string> names = new List<string>();
                var list = (this.TargetControl as CheckBoxList).Items;
                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    if (IsRevert(list[i]) == revert)
                        names.Add(string.Format("{0}_{1}", this.TargetControl.ClientID, i));
                }

                return names;
            }

            protected List<string> GetClientNameOfListControls(string list)
            {
                List<string> names = new List<string>();
                foreach (Control child in this.GetControlsByList(list))
                {
                    names.Add(child.ClientID);
                }
                return names;
            }

            /// <summary>
            /// Generate client validate function. Used by RegisterClientScript function
            /// </summary>
            /// <param name="funcName">The name of validate function</param>
            /// <returns>Generated clint script</returns>
            protected string GenClientScript(string funcName)
            {
                StringBuilder script = new StringBuilder();
                script.Append("\n<script type=\"text/javascript\"> \n");
                script.Append("//<![CDATA[ \n");
                script.Append("function ").Append(funcName).Append("(source, args) { \n");
                List<string> childNames1 = this.GetClientNamesOfChildren();
                List<string> childNames2 = this.GetClientNamesOfRevertChildren();
                if (childNames1.Count > 0 || childNames2.Count > 0)
                {
                    if (childNames1.Count > 0) GenClientScript(ref script, childNames1, false);
                    if (childNames2.Count > 0) GenClientScript(ref script, childNames2, true);
                    script.Append("return args.IsValid = false; \n");
                }
                else
                {
                    if (this.ValidateEmptyList)
                    {
                        script.Append("return args.IsValid = false; \n");
                    }
                    else
                    {
                        script.Append("return args.IsValid = true; \n");
                    }
                }
                script.Append("} \n");
                script.Append("//]]> \n");
                script.Append("</script> \n");

                return script.ToString();
            }
            protected void GenClientScript(ref StringBuilder script, List<string> childNames, bool revert)
            {
                string arr = revert ? "childNames1" : "childNames2";
                string cnt = revert ? "count1" : "count2";
                string chd = revert ? "child1" : "child2"; 
                
                script.Append("var " + arr + " = new Array(");
                script.Append("\"").Append(childNames[0]).Append("\"");
                for (int i = 1; i < childNames.Count; i++)
                {
                    script.Append(",\"").Append(childNames[i]).Append("\"");
                }
                script.Append("); \n");
                script.Append(string.Format("var " + cnt + "={0}; \n", childNames.Count));
                script.Append("for (i=0; i<" + cnt + "; i++) { \n");
                script.Append("     var " + chd + " = (document.all)? document.all[" + arr + "[i]] : document.getElementById(" + arr + "[i]); \n");
                script.Append("     //alert(child.type); \n");
                script.Append("     if (" + chd + ") { \n");
                if (revert)
                    script.Append("         if (((" + chd + ".type.toLowerCase() == \"text\") && (trim(" + chd + ".value) == \"\")) || ((" + chd + ".type.toLowerCase() == \"checkbox\") && !" + chd + ".checked) || ((" + chd + ".type.toLowerCase() == \"select-one\") && !" + chd + ".value) || ((" + chd + ".tagName.toLowerCase() == \"span\") && !" + chd + ".firstChild.nodeValue)) { \n");
                else
                    script.Append("         if (((" + chd + ".type.toLowerCase() == \"text\") && (trim(" + chd + ".value) != \"\")) || ((" + chd + ".type.toLowerCase() == \"checkbox\") && " + chd + ".checked) || ((" + chd + ".type.toLowerCase() == \"select-one\") && " + chd + ".value) || ((" + chd + ".tagName.toLowerCase() == \"span\") && " + chd + ".firstChild.nodeValue)) { \n");
                script.Append("             return args.IsValid = true; \n");
                script.Append("         } \n");
                script.Append("     } \n");
                script.Append("} \n");
            }

            /// <summary>
            /// Register ClientScript to validate on client
            /// </summary>
            protected void RegisterClientScript()
            {
                // using block :)
                if (!this.Page.ClientScript.IsClientScriptBlockRegistered(StringScript.ScriptKey))
                {
                    this.Page.ClientScript.RegisterClientScriptBlock(typeof(IClientScriptBlock), StringScript.ScriptKey, StringScript.GetScript());
                }
                // Register Client Script to validate
                var c = this.NamingContainer;
                var vldCtrl = c.FindControl(this.ControlToValidate) as WebControl;
                if (vldCtrl == null) vldCtrl = WebTools.FindControlById(this.ControlToValidate, this.Page) as WebControl;

                if (vldCtrl != null)
                {
                    this.ClientValidationFunction = string.Format("{0}_vefify_{1}", this.ID, vldCtrl.ClientID);
                    this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), this.ClientValidationFunction + this.ClientID, this.GenClientScript(this.ClientValidationFunction));
                }
            }

            #endregion
        }
    }

}