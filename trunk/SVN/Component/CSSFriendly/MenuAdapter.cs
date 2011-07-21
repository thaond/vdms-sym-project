using System;
using System.Collections;
using System.Reflection;
using System.IO;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace CSSFriendly
{
    public class MenuAdapter : System.Web.UI.WebControls.Adapters.MenuAdapter
    {
        private WebControlAdapterExtender _extender = null;
        private WebControlAdapterExtender Extender
        {
            get
            {
                if (((_extender == null) && (Control != null)) ||
                        ((_extender != null) && (Control != _extender.AdaptedControl)))
                {
                    _extender = new WebControlAdapterExtender(Control);
                }

                System.Diagnostics.Debug.Assert(_extender != null, "CSS Friendly adapters internal error", "Null extender instance");
                return _extender;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (Extender.AdapterEnabled)
            {
                RegisterScripts();
            }
        }

        private void RegisterScripts()
        {
            Extender.RegisterScripts();

            /* 
             * Modified for support of compiled CSSFriendly assembly
             * 
             * We will first search for embedded JavaScript files. If they are not
             * found, we default to the standard approach.
             */

            Type type = typeof(CSSFriendly.MenuAdapter);

            // MenuAdapter.js
            // Helpers.RegisterClientScript("CSSFriendly.JavaScript.MenuAdapter.js", type, this.Page);

            // Menu.css
            // Helpers.RegisterEmbeddedCSS("CSSFriendly.CSS.Menu.css", type, this.Page);

            // Only send IEMenu6.css if it's IE version <7
            //if (this.Browser.Browser.ToUpper().IndexOf("IE") >= 0
            //        && this.Browser.MajorVersion < 7)
            //{
            //    Helpers.RegisterEmbeddedCSS("CSSFriendly.CSS.BrowserSpecific.IEMenu6.css", type, this.Page);
            //}
        }

        protected override void RenderBeginTag(HtmlTextWriter writer)
        {
            //if (Extender.AdapterEnabled)
            //{
            //    Extender.RenderBeginTag(writer, "AspNet-Menu-" + Control.Orientation.ToString());
            //}
            //else
            //{
            //    base.RenderBeginTag(writer);
            //}
        }

        protected override void RenderEndTag(HtmlTextWriter writer)
        {
            //if (Extender.AdapterEnabled)
            //{
            //    Extender.RenderEndTag(writer);
            //}
            //else
            //{
            //    base.RenderEndTag(writer);
            //}
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (Extender.AdapterEnabled)
            {
                writer.Indent++;
                BuildItems(Control.Items, true, writer);
                writer.Indent--;
                writer.WriteLine();
            }
            else
            {
                base.RenderContents(writer);
            }
        }

        private void BuildItems(MenuItemCollection items, bool isRoot, HtmlTextWriter writer)
        {
            if (items.Count > 0)
            {
                writer.WriteLine();

                writer.WriteBeginTag("ul");
                if (isRoot)
                {
                    //writer.WriteAttribute("class", "sf-menu sf-navbar");
                }
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Indent++;
                int i = 0;
                int itemscount = items.Count;
                while (i < itemscount)
                {
                    if (i == 0)
                        BuildItem(items[i], writer, 1);
                    else
                        if(i == itemscount - 1)
                            BuildItem(items[i], writer, 3);
                        else
                            BuildItem(items[i], writer, 2);
                    i++;
                }
                writer.Indent--;
                writer.WriteLine();
                writer.WriteEndTag("ul");
            }
        }

        /// <summary>
        /// mvbinh edit:
        /// Flag = 1 : Frist Item
        /// Flag = 2 : Normal Item
        /// Flag = 3 : Last Item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="writer"></param>
        /// <param name="Flag"></param>
        private void BuildItem(MenuItem item, HtmlTextWriter writer, int Flag)
        {
            Menu menu = Control as Menu;
            if ((menu != null) && (item != null) && (writer != null))
            {
                writer.WriteLine();
                writer.WriteBeginTag("li");
                if (Flag == 1)
                    writer.WriteAttribute("style", "border-left: 0pt none;");
                if (Flag == 3)
                    writer.WriteAttribute("style", "border-right: 0pt none;");
                //string theClass = (item.ChildItems.Count > 0) ? "AspNet-Menu-WithChildren" : "AspNet-Menu-Leaf";
                //string selectedStatusClass = GetSelectStatusClass(item);
                //if (!String.IsNullOrEmpty(selectedStatusClass))
                //{
                //    theClass += " " + selectedStatusClass;
                //}
                //writer.WriteAttribute("class", theClass);

                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Indent++;
                writer.WriteLine();

                if (((item.Depth < menu.StaticDisplayLevels) && (menu.StaticItemTemplate != null)) ||
                        ((item.Depth >= menu.StaticDisplayLevels) && (menu.DynamicItemTemplate != null)))
                {
                    writer.WriteBeginTag("div");
                    //writer.WriteAttribute("class", GetItemClass(menu, item));
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.Indent++;
                    writer.WriteLine();

                    MenuItemTemplateContainer container = new MenuItemTemplateContainer(menu.Items.IndexOf(item), item);
                    //added to solve the <a href='<%# Eval("Text")%>'> binding problem
                    //http://forums.asp.net/t/1069719.aspx
                    //http://msdn2.microsoft.com/en-us/library/system.web.ui.control.bindingcontainer.aspx
                    //The BindingContainer property is the same as the NamingContainer property, 
                    //except when the control is part of a template. In that case, the BindingContainer 
                    //property is set to the Control that defines the template.
                    menu.Controls.Add(container);

                    if ((item.Depth < menu.StaticDisplayLevels) && (menu.StaticItemTemplate != null))
                    {
                        menu.StaticItemTemplate.InstantiateIn(container);
                    }
                    else
                    {
                        menu.DynamicItemTemplate.InstantiateIn(container);
                    }
                    container.DataBind(); //Databinding must occurs before rendering
                    container.RenderControl(writer);
                    writer.Indent--;
                    writer.WriteLine();
                    writer.WriteEndTag("div");
                }
                else
                {
                    if (IsLink(item))
                    {
                        writer.WriteBeginTag("a");
                        if (!String.IsNullOrEmpty(item.NavigateUrl))
                        {
                            writer.WriteAttribute("href", Page.Server.HtmlEncode(menu.ResolveClientUrl(item.NavigateUrl)));
                        }
                        else
                        {
                            writer.WriteAttribute("href", Page.ClientScript.GetPostBackClientHyperlink(menu, "b" + item.ValuePath.Replace(menu.PathSeparator.ToString(), "\\"), true));
                        }

                        //string s = GetItemClass(menu, item);
                        //if (s != string.Empty) writer.WriteAttribute("class", s);
                        WebControlAdapterExtender.WriteTargetAttribute(writer, item.Target);

                        if (!String.IsNullOrEmpty(item.ToolTip))
                        {
                            writer.WriteAttribute("title", item.ToolTip);
                        }
                        else if (!String.IsNullOrEmpty(menu.ToolTip))
                        {
                            writer.WriteAttribute("title", menu.ToolTip);
                        }
                        writer.Write(HtmlTextWriter.TagRightChar);
                        writer.Indent++;
                        writer.WriteLine();
                    }
                    else
                    {
                        writer.WriteBeginTag("a");
                        //writer.WriteAttribute("class", GetItemClass(menu, item));
                        writer.Write(HtmlTextWriter.TagRightChar);
                        writer.Indent++;
                        writer.WriteLine();
                    }

                    if (!String.IsNullOrEmpty(item.ImageUrl))
                    {
                        writer.WriteBeginTag("img");
                        writer.WriteAttribute("src", menu.ResolveClientUrl(item.ImageUrl));
                        writer.WriteAttribute("alt", !String.IsNullOrEmpty(item.ToolTip) ? item.ToolTip : (!String.IsNullOrEmpty(menu.ToolTip) ? menu.ToolTip : item.Text));
                        writer.Write(HtmlTextWriter.SelfClosingTagEnd);
                    }

                    writer.Write(item.Text);

                    if (IsLink(item))
                    {
                        writer.Indent--;
                        writer.WriteEndTag("a");
                    }
                    else
                    {
                        writer.Indent--;
                        writer.WriteEndTag("a");
                    }

                }

                if ((item.ChildItems != null) && (item.ChildItems.Count > 0))
                {
                    BuildItems(item.ChildItems, false, writer);
                }

                writer.Indent--;
                writer.WriteLine();
                writer.WriteEndTag("li");
            }
        }

        protected virtual bool IsLink(MenuItem item)
        {
            return (item != null) && item.Enabled && ((!String.IsNullOrEmpty(item.NavigateUrl)) || item.Selectable);
        }

        private string GetItemClass(Menu menu, MenuItem item)
        {
            //string value = "AspNet-Menu-NonLink";
            //if (item != null)
            //{
            //    if (((item.Depth < menu.StaticDisplayLevels) && (menu.StaticItemTemplate != null)) ||
            //            ((item.Depth >= menu.StaticDisplayLevels) && (menu.DynamicItemTemplate != null)))
            //    {
            //        value = "AspNet-Menu-Template";
            //    }
            //    else if (IsLink(item))
            //    {
            //        value = "AspNet-Menu-Link";
            //    }
            //    string selectedStatusClass = GetSelectStatusClass(item);
            //    if (!String.IsNullOrEmpty(selectedStatusClass))
            //    {
            //        value += " " + selectedStatusClass;
            //    }
            //}
            //return value;
            if (item == null) return string.Empty;
            if (item.Depth == 1) return "sf-link-bar";
            if (item.ChildItems.Count > 0) return "sf-with-ul";
            return string.Empty;
        }

        private string GetSelectStatusClass(MenuItem item)
        {
            string value = "";
            if (item.Selected)
            {
                value += " AspNet-Menu-Selected";
            }
            else if (IsChildItemSelected(item))
            {
                value += " AspNet-Menu-ChildSelected";
            }
            else if (IsParentItemSelected(item))
            {
                value += " AspNet-Menu-ParentSelected";
            }
            return value;
        }

        private bool IsChildItemSelected(MenuItem item)
        {
            bool bRet = false;

            if ((item != null) && (item.ChildItems != null))
            {
                bRet = IsChildItemSelected(item.ChildItems);
            }

            return bRet;
        }

        private bool IsChildItemSelected(MenuItemCollection items)
        {
            bool bRet = false;

            if (items != null)
            {
                foreach (MenuItem item in items)
                {
                    if (item.Selected || IsChildItemSelected(item.ChildItems))
                    {
                        bRet = true;
                        break;
                    }
                }
            }

            return bRet;
        }

        private bool IsParentItemSelected(MenuItem item)
        {
            bool bRet = false;

            if ((item != null) && (item.Parent != null))
            {
                if (item.Parent.Selected)
                {
                    bRet = true;
                }
                else
                {
                    bRet = IsParentItemSelected(item.Parent);
                }
            }

            return bRet;
        }


        /*
         * COMMENTED OUT AS OF 5/6/08.
         * 
         * If you need this functionality, uncomment this method. As it is, it uses reflection, which could cause
         * trust errors in some hosting environments.
        /// <summary>
        /// Clear the embedded header styles that ASP.NET automatically adds for the menu.
        /// </summary>
        /// <param name="e"></param>
        /// <remarks>
        /// Patch provided by LonelyRollingStar via CodePlex issue #2714:
        /// 
        /// The ASP.NET Menu control automatically adds several styles to the embedded header style. For example:
        /// 
        /// <c>.ctl00_ucMenu_MenuCBM_0 { background-color:white;visibility:hidden;display:none;position:absolute;left:0px;top:0px; }</c>
        /// 
        /// This is unnecessary and probably breaks your ability to fully style your menu with external stylesheets.
        /// 
        /// The culprit is an internal method of the Menu control called EnsureRenderSettings. It calls 
        /// <c>this.Page.Header.StyleSheet.CreateStyleRule()</c> several times, adding style rules to the embedded stylesheet. 
        /// <c>EnsureRenderSettings</c> is called at the beginning of the Menu's <c>OnPreRender</c> method. 
        /// 
        /// Hey, the menu adapter exposes OnPreRender! That means we're getting closer to an answer, right? 
        /// Right, but if we override <c>OnPreRender</c> and don't call the base implementation we'll be 
        /// missing some important functionality. This uses internal methods so we cannot emulate it in the menu adapter.
        /// 
        /// The solution is a bit of a hack, but it's quick and direct. We override <c>OnPreRender</c> and after calling 
        /// the base implementation as normal, we clear the embedded header styles that were just added.
        /// </remarks>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            // Clear the embedded header styles that ASP.NET automatically adds for the menu
            ArrayList selectorStyles = Helpers.GetPrivateField(Page.Header.StyleSheet, "_selectorStyles") as ArrayList;
            if (selectorStyles != null)
            {
                selectorStyles.Clear();
            }
        }
         */
    }
}
