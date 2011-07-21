using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using VDMS.WebService.Interface;
using VDMS.II.Entity;
using VDMS.II.PartManagement.Order;
using System.IO;
using System.Web;
using System.Configuration;
using VDMS.Web;
using VDMS.Data.TipTop;
using VDMS.WebService.Entity;
using System.CodeDom.Compiler;
using VDMS.II.Common.Utils;
using System.Collections.Specialized;
using System.Data;

namespace VDMS.WebService.Service
{
    // NOTE: If you change the class name "Part" here, you must also update the reference to "Part" in Web.config.
    //[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    //[ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]
    public class Common : ICommon
    {
        public static object SyncRoot = new object();

        public string GetFilename(string fullpath)
        {
            int i = fullpath.LastIndexOf("\\") + 1;
            return fullpath.Substring(i, fullpath.Length - i);
        }

        #region ICommon Members

        string _base = null;
        public string GetBase()
        {
            if (_base == null)
            {
                //if ((VDMSHttpApplication.scheduler != null) && (VDMSHttpApplication.scheduler.HTTPContext != null))
                //{
                //    _base = VDMSHttpApplication.scheduler.HTTPContext.Server.MapPath("~/");
                //}
                //else
                {
                    _base = ConfigurationSettings.AppSettings["CurrentDir"];
                }
            }
            return (_base.EndsWith("\\")) ? _base : _base + "\\";
        }

        public string GetTestString()
        {
            return "28/12/2009 AM";
        }

        public string GetTmp()
        {
            string temp = "bin\\{1836E224-D081-4e7e-B114-69651823D8F3}\\";
            string path = string.Format("{0}{1}", GetBase(), temp);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return temp;
        }

        public ServerItem GetItem(string item)
        {
            string b = GetBase();
            string s = string.Format("{0}{1}", b, item);

            byte[] data = VDMS.II.Common.Utils.BinaryFileHelper.ReadFile(s);
            byte[] cdata = VDMS.II.Common.Utils.Compressor.Compress(data);
            bool isComp = data.LongLength > cdata.LongLength;
            string sData = Convert.ToBase64String(isComp ? cdata : data);

            return new ServerItem() { Data = sData, IsComp = isComp };
        }

        public bool LoadApp(string sData, string actOn, string obj, long size, bool isComp)
        {
            lock (Common.SyncRoot)
            {
                byte[] data = Convert.FromBase64String(sData);
                if (isComp) data = VDMS.II.Common.Utils.Compressor.Decompress(data);

                bool res = false;
                string tmp = string.Format("{0}{1}{2}", GetBase(), GetTmp(), obj);
                string act = string.Format("{0}{1}{2}", GetBase(), actOn, obj);
                string old = string.Format("{0}{1}{2}.{3}", GetBase(), GetTmp(), obj, DateTime.Now.Ticks);
                FileStream s = new FileStream(tmp, FileMode.Create);
                BinaryWriter w = new BinaryWriter(s);
                Exception e = null;
                try
                {
                    FileInfo f = new FileInfo(act);
                    if (f.Exists) System.IO.File.Copy(act, old, true);
                    w.Write(data);
                    w.Flush();
                }
                catch (Exception ex) { e = ex; }
                finally
                {
                    w.Close();
                    s.Close();
                    if (e != null) throw e;
                    FileInfo f = new FileInfo(tmp);
                    if (f.Length == size)
                    {
                        System.IO.File.Copy(tmp, act, true);
                        System.IO.File.Delete(tmp);
                        res = true;
                    }
                }
                return res;
            }
        }

        public System.Data.DataTable RunQCommand(string cmd)
        {
            var tbl = DataObjectBase.ExecuteSql(cmd).Tables[0];
            tbl.TableName = "data";
            return tbl;
        }

        public decimal RunScalarCommand(string cmd)
        {
            return DataObjectBase.ExecuteScalarSql<decimal>(cmd);
        }

        public List<FileFolderInfo> ListDir(string dir)
        {
            if (dir[dir.Length - 1] != '\\') dir += "\\";
            string b = GetBase();
            string p = string.Format("{0}{1}", b, dir);
            var res = new List<FileFolderInfo>();

            foreach (var d in Directory.GetDirectories(p))
            {
                res.Add(new FileFolderInfo() { Dir = true, Name = GetFilename(d), Path = dir, });
            }

            foreach (var f in Directory.GetFiles(p))
            {
                FileInfo fi = new FileInfo(f);
                res.Add(new FileFolderInfo()
                {
                    Dir = false,
                    Name = GetFilename(f),//fi.Name,
                    Path = dir,
                    Size = fi.Length,
                    RO = fi.IsReadOnly,
                    LastWriteTime = fi.LastWriteTime
                });
            }

            return res;
        }

        public void CopyTo(string sFile, string dFile)
        {
            string b = GetBase();
            string s = string.Format("{0}{1}", b, sFile);
            FileInfo TheFile = new FileInfo(s);
            if (TheFile.Exists)
            {
                string d = string.Format("{0}{1}", b, dFile);
                System.IO.File.Copy(s, d);
            }
        }

        public void ReName(string file, string newName)
        {
            string b = GetBase();
            string s = string.Format("{0}{1}", b, file);
            FileInfo TheFile = new FileInfo(s);
            if (TheFile.Exists)
            {
                string sn = string.Format("{0}{1}", b, file);
                System.IO.File.Move(s, sn);
            }
        }

        public void Delete(string file)
        {
            string b = GetBase();
            string s = string.Format("{0}{1}", b, file);
            FileInfo TheFile = new FileInfo(s);
            if (TheFile.Exists)
            {
                System.IO.File.Delete(s);
            }
        }

        public DataTable CallForTable(string c, string cl, string mt, string[] refs, string[] param, bool isStatic, out string[] output)
        {
            CompilerResults CompilationResult = CodeFly.Compile(refs, c);
            output = new string[CompilationResult.Output.Count];
            CompilationResult.Output.CopyTo(output, 0);

            return CodeFly.CallDataMethod(CompilationResult, cl, mt, param, isStatic);
        }
        public string CallForString(string c, string cl, string mt, string[] refs, string[] param, bool isStatic, out string[] output)
        {
            CompilerResults CompilationResult = CodeFly.Compile(refs, c);
            output = new string[CompilationResult.Output.Count];
            CompilationResult.Output.CopyTo(output, 0);

            return CodeFly.CallStrMethod(CompilationResult, cl, mt, param, isStatic);
        }

        #endregion
    }
}