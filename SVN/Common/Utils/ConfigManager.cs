using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;

namespace VDMS.II.Common.Utils
{
	public class CodeFly
	{
		public static string GetFullReference(string relativeReference)
		{
			// First, get the path for this executing assembly.
			Assembly a = Assembly.GetExecutingAssembly();
			string path = Path.GetDirectoryName(a.Location);

			// if the file exists in this Path - prepend the path
			string fullReference = Path.Combine(path, relativeReference);
			if (File.Exists(fullReference))
				return fullReference;
			else
			{
				// Strip off any trailing ".dll" if present.
				if (string.Compare(relativeReference.Substring(relativeReference.Length - 4), ".dll", true) == 0)
					fullReference = relativeReference.Substring(0, relativeReference.Length - 4);
				else
					fullReference = relativeReference;

				// See if the required assembly is already present in our current AppDomain
				foreach (Assembly currAssembly in
				AppDomain.CurrentDomain.GetAssemblies())
				{
					if (string.Compare(currAssembly.GetName().Name, fullReference,
					true) == 0)
					{
						// Found it, return the location as the full reference.
						return currAssembly.Location;
					}
				}

				// The assembly isn't present in our current application, so attempt to
				// load it from the GAC, using the partial name.
				try
				{
					Assembly tempAssembly =
					Assembly.LoadWithPartialName(fullReference);
					return tempAssembly.Location;
				}
				catch
				{
					// If we cannot load or otherwise access the assembly from the GAC then just
					// return the relative reference and hope for the best.
					return relativeReference;
				}
			}
		}

		public static CompilerResults Compile(string[] refs, string c)
		{
			// setup------------------------
			CodeDomProvider RuntimeCompiler = new Microsoft.CSharp.CSharpCodeProvider();
			CompilerParameters Parameters = new CompilerParameters();
			Parameters.GenerateExecutable = false;
			Parameters.GenerateInMemory = true;
			Parameters.IncludeDebugInformation = true;
			foreach (string item in refs)
			{
				Parameters.ReferencedAssemblies.Add(CodeFly.GetFullReference(item));
			}

			// compile------------------------
			CompilerResults CompilationResult = RuntimeCompiler.CompileAssemblyFromSource(Parameters, c);
			return CompilationResult;
		}

		public static object[] GetParams(string[] param)
		{
			List<object> res = new List<object>();
			foreach (string item in param)
			{
				if (!string.IsNullOrEmpty(item))
				{
					string line = item.TrimStart();
					string type = line.Substring(0, line.IndexOf(':')).ToLower();
					string val = line.Substring(type.Length + 1);
					switch (type)
					{
						case "string": res.Add(val); break;
						case "char": res.Add(char.Parse(val)); break;
						case "int": res.Add(int.Parse(val)); break;
						case "long": res.Add(long.Parse(val)); break;
						case "decimal": res.Add(decimal.Parse(val)); break;
						case "bool": res.Add(bool.Parse(val)); break;
						case "float": res.Add(float.Parse(val)); break;
						case "datetime": res.Add(DateTime.Parse(val)); break;
						default: throw new Exception(string.Format("Parameter type not found: {0}", type));
					}
				}
			}
			return res.Count > 0 ? res.ToArray() : null;
		}

		public static string CallStrMethod(CompilerResults CompilationResult, string cl, string mt, string[] param, bool isStatic)
		{
			if (!CompilationResult.Errors.HasErrors)
			{
				object res = null;
				if (isStatic)
				{
					Type type = CompilationResult.CompiledAssembly.GetType(cl, true, false);
					res = type.GetMethod(mt).Invoke(null, GetParams(param));
				}
				else
				{
					object obj = CompilationResult.CompiledAssembly.CreateInstance(cl, false);
					res = obj.GetType().GetMethod(mt).Invoke(obj, GetParams(param));
				}
				return res == null ? "Null" : (string)res;
			}
			return null;
		}
		public static DataTable CallDataMethod(CompilerResults CompilationResult, string cl, string mt, string[] param, bool isStatic)
		{
			if (!CompilationResult.Errors.HasErrors)
			{
				object res = null;
				if (isStatic)
				{
					Type type = CompilationResult.CompiledAssembly.GetType(cl, true, false);
					res = type.GetMethod(mt).Invoke(null, GetParams(param));
				}
				else
				{
					object obj = CompilationResult.CompiledAssembly.CreateInstance(cl, false);
					res = obj.GetType().GetMethod(mt).Invoke(obj, GetParams(param));
				}
				return (DataTable)res;
			}
			return null;
		}
	}

	//public enum ConfigFileType
	//{
	//    WebConfig,
	//    AppConfig
	//}

	//public class ConfigFileManager
	//{
	//    private string _currentNode;
	//    private XmlNode node = null;
	//    private XmlDocument cfgDoc;
	//    private XmlNamespaceManager nsmgr;
	//    public string CurrentNSPrefix { get; set; }

	//    public string docName = String.Empty;
	//    public ConfigFileType ConfigType { get; private set; }
	//    public string CurrentNodePath
	//    {
	//        get { return _currentNode; }
	//        set
	//        {
	//            _currentNode = value;
	//            node = cfgDoc.SelectSingleNode(_currentNode, nsmgr);
	//        }
	//    }

	//    public ConfigFileManager(ConfigFileType cfgType)
	//    {
	//        ConfigType = cfgType;
	//        cfgDoc = new XmlDocument();
	//        loadConfigDoc();
	//    }

	//    public void AddNamespace(string alias, string ns)
	//    {
	//        if (nsmgr != null) nsmgr.AddNamespace(alias, ns);
	//    }

	//    public bool SetValue(string nodeName, string keyName, string keyVal, string newKeyVal)
	//    {
	//        return SetValue(nodeName, keyName, keyVal, keyName, newKeyVal);
	//    }
	//    public bool SetValue(string nodeName, string keyName, string keyVal, string setName, string setVal)
	//    {
	//        if (node == null)
	//        {
	//            throw new System.InvalidOperationException("Current node hasn't been selected!");
	//        }

	//        try
	//        {
	//            // XPath select setting "add" element that contains this key    
	//            XmlElement addElem = (XmlElement)node.SelectSingleNode(string.Format("//{0}[@{1}='{2}']", nodeName, keyName, keyVal), nsmgr);
	//            if (addElem != null)
	//            {
	//                addElem.SetAttribute(setName, setVal);
	//            }
	//            // not found, so we need to add the element, key and value
	//            else
	//            {
	//                XmlElement entry = cfgDoc.CreateElement(nodeName);//, nsmgr.LookupNamespace(CurrentNSPrefix));
	//                entry.SetAttribute(keyName, keyVal);
	//                entry.SetAttribute(setName, setVal);
	//                node.AppendChild(entry);
	//            }
	//            return true;
	//        }
	//        catch
	//        {
	//            return false;
	//        }
	//    }

	//    public string GetAttribute(string name)
	//    {
	//        if (node == null)
	//        {
	//            throw new System.InvalidOperationException("Current node hasn't been selected!");
	//        }
	//        return node.Attributes[name].Value;
	//    }

	//    public XmlElement GetElement(string nodeName, string keyName, string keyVal)
	//    {
	//        if (node == null)
	//        {
	//            throw new System.InvalidOperationException("Current node hasn't been selected!");
	//        }
	//        return (XmlElement)node.SelectSingleNode(string.Format("//{0}[@{1}='{2}']", nodeName, keyName, keyVal), nsmgr);
	//    }
	//    public void AddElement(XmlElement element)
	//    {
	//        node.AppendChild(element);
	//    }
	//    public bool RemoveNode(string nodeName, string keyName, string keyVal)
	//    {
	//        try
	//        {
	//            if (node == null)
	//            {
	//                throw new System.InvalidOperationException("Current node hasn't been selected!");
	//            }
	//            // XPath select nodeName element that contains keyName with keyVal to remove   
	//            node.RemoveChild(node.SelectSingleNode(string.Format("//{0}[@{1}='{2}']", nodeName, keyName, keyVal), nsmgr));

	//            return true;
	//        }
	//        catch
	//        {
	//            return false;
	//        }
	//    }

	//    public void SaveConfigDoc()
	//    {
	//        try
	//        {
	//            XmlTextWriter writer = new XmlTextWriter(docName, null);
	//            writer.Formatting = Formatting.Indented;
	//            cfgDoc.WriteTo(writer);
	//            writer.Flush();
	//            writer.Close();
	//            return;
	//        }
	//        catch
	//        {
	//            throw;
	//        }
	//    }

	//    private XmlDocument loadConfigDoc()
	//    {
	//        // load the config file 
	//        if (ConfigType == ConfigFileType.AppConfig)
	//        {

	//            docName = ((Assembly.GetEntryAssembly()).GetName()).Name;
	//            docName += ".exe.config";
	//        }
	//        else
	//        {
	//            docName = System.Web.HttpContext.Current.Server.MapPath("~/web.config");
	//        }
	//        cfgDoc.Load(docName);
	//        nsmgr = new XmlNamespaceManager(cfgDoc.NameTable);
	//        return cfgDoc;
	//    }
	//}
}
