using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;


namespace VDMS
{
    namespace Common.Data
    {

        public class TreeListBaseObject
        {
            public int RowSpan;         // span level for all column correspond to this.Properties
        }

        public class TreeListObjects : TreeListBaseObject
        {
            public int BindOrder;       // specify priority of property to expand values
            public bool BindAllEvenEmpty;
            private ArrayList items;

            public ArrayList Items
            {
                get { return items; }
                set { items = value; }
            }

            // bindAll = true => bind all property except "Items" event <TreeListObjects> has no child items
            public int FillDataTable(DataTable table, Hashtable values, bool bindAll)
            {
                object obj;
                bool rootObj = false, // this is the first objectList (used to assign to GridView)
                     haveDataToBind = false;

                Type t = this.GetType();
                PropertyInfo[] props = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                // get current level values
                if (values == null) { values = new Hashtable(); rootObj = true; }
                foreach (PropertyInfo prop in props)
                {
                    if (prop.Name == "Items") continue; // bypass children
                    obj = prop.GetValue(this, null);
                    if ((obj != null) && (table.Columns.Contains(prop.Name)))
                    {
                        values[prop.Name] = obj;
                        if (obj != null) haveDataToBind = true;
                    }
                }

                // get child level values
                RowSpan = 0;
                if (Items.Count > 0)
                    foreach (TreeListObject child in this.Items)
                    {
                        RowSpan += child.FillDataTable(table, values, bindAll);
                    }
                else if ((bindAll && (!rootObj)) || // do not fill root obj when it has no child => show empty Grid
                          haveDataToBind)           // is there any data in current level to bind?
                {
                    DataRow row = table.NewRow();
                    foreach (DictionaryEntry val in values) { row[val.Key.ToString()] = val.Value; }
                    table.Rows.Add(row);
                    RowSpan = 1;
                }

                // clear current level values
                foreach (PropertyInfo prop in props)
                {
                    if ((prop.Name != "Items") && (values.Contains(prop.Name))) values.Remove(prop.Name);
                }

                // row span for this level
                return RowSpan;
            }
            public int FillDataTable(DataTable table)
            {
                return FillDataTable(table, null, BindAllEvenEmpty);
            }
            public TreeListObjects(bool bindAll)
                : this()
            {
                BindAllEvenEmpty = bindAll;
            }
            public TreeListObjects()
            {
                Items = new ArrayList();
                BindOrder = int.MaxValue - 1;
                BindAllEvenEmpty = true;
                RowSpan = 1;
            }
        }

        public class TreeListObjectsOrderComparer : IComparer
        {
            private object objToGetVal;
            public object ObjectToGetVal
            {
                set { objToGetVal = value; }
                get { return objToGetVal; }
            }
            int IComparer.Compare(Object x, Object y)
            {
                if (!((x is PropertyInfo) && (y is PropertyInfo))) return 0;
                int xOrder = ((TreeListObjects)((PropertyInfo)x).GetValue(ObjectToGetVal, null)).BindOrder;
                int yOrder = ((TreeListObjects)((PropertyInfo)y).GetValue(ObjectToGetVal, null)).BindOrder;
                return (xOrder > yOrder) ? 1 : ((xOrder < yOrder) ? -1 : 0);
            }
            public TreeListObjectsOrderComparer(object obj)
            {
                ObjectToGetVal = obj;
            }
        }

        public class TreeListObject : TreeListBaseObject
        {
            // determine whether "type" is <TreeListObjects>
            protected bool IsTreeListChildObjects(Type type)
            {
                if (type == null) return false;
                if (type == typeof(TreeListObjects)) return true;
                return IsTreeListChildObjects(type.BaseType);
            }

            // Classify Property to <bind> child level or <not bind>
            protected void ClassifyProperty(PropertyInfo[] PropList, ArrayList field, ArrayList bind)
            {
                foreach (PropertyInfo prop in PropList)
                {
                    // this prop is field property
                    if (!IsTreeListChildObjects(prop.PropertyType)) { if (field != null) field.Add(prop); continue; }

                    // bind property
                    if (prop.GetValue(this, null) != null) { if (bind != null) bind.Add(prop); }
                }
            }

            // bindAll = true => bind all property except "Items" event <TreeListObjects> has no child items
            public int FillDataTable(DataTable table, Hashtable values, bool bindAll)
            {
                this.RowSpan = 0;
                Type t = this.GetType();
                ArrayList fieldProp = new ArrayList(), bindProp = new ArrayList();

                // Classify Property and sort binding property by "BindOrder"
                IComparer bindOrderComparer = new TreeListObjectsOrderComparer(this);
                PropertyInfo[] props = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                ClassifyProperty(props, fieldProp, bindProp);
                bindProp.Sort(bindOrderComparer);

                // get current level values from fields property
                if (values == null) values = new Hashtable();
                foreach (PropertyInfo prop in fieldProp)
                {
                    if (table.Columns.Contains(prop.Name))
                    {
                        values[prop.Name] = prop.GetValue(this, null);
                    }
                }

                // get child level values from binds property
                if (bindProp.Count > 0)
                    foreach (PropertyInfo prop in bindProp)
                    {
                        TreeListObjects objs = (TreeListObjects)prop.GetValue(this, null);
                        if (objs != null) { this.RowSpan += objs.FillDataTable(table, values, bindAll); }
                    }
                // leaf ==> create and add row
                else
                {
                    DataRow row = table.NewRow();
                    foreach (DictionaryEntry val in values) { row[val.Key.ToString()] = val.Value; }
                    table.Rows.Add(row);
                    this.RowSpan = 1;
                }

                // clear current level values
                foreach (PropertyInfo prop in fieldProp)
                {
                    if (values.Contains(prop.Name)) values.Remove(prop.Name);
                }

                // after all return row span for this object level
                return this.RowSpan;
            }

            public ArrayList GetSortedBindProperties()
            {
                ArrayList bindProp = new ArrayList();
                IComparer bindOrderComparer = new TreeListObjectsOrderComparer(this);
                Type t = this.GetType();
                PropertyInfo[] props = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                ClassifyProperty(props, null, bindProp);
                bindProp.Sort(bindOrderComparer);
                return bindProp;
            }
        }
    }
}