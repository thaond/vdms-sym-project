using System;

/// <summary>
/// Summary description for CacheHelper
/// </summary>
namespace VDMS.Helper
{
	public class CacheHelper
	{
		public static void Insert(object Id, object obj)
		{
			System.Web.HttpContext.Current.Cache.Insert(string.Concat(obj.GetType().FullName, Id), obj);
		}

		public static T Retrieve<T>(object Id)
		{
			return (T)System.Web.HttpContext.Current.Cache[string.Concat(typeof(T).FullName, Id)];
		}

		public static T GetAndCache<T>(object Id, Func<T> retrieveObject)
		{
			T value = Retrieve<T>(Id);
			if (value == null)
			{
				value = retrieveObject(); //this makes the call to the database
				if (value != null) Insert(Id, value);
			}
			return Retrieve<T>(Id);
		}

		public static void Clear()
		{
			System.Collections.IDictionaryEnumerator cacheContents = System.Web.HttpContext.Current.Cache.GetEnumerator();
			while (cacheContents.MoveNext())
			{
				System.Web.HttpContext.Current.Cache.Remove(cacheContents.Key.ToString());
			}
		}

		public static void Remove<T>(object Id)
		{
			System.Web.HttpContext.Current.Cache.Remove(string.Concat(typeof(T).FullName, Id));
		}

		public static void RemoveAll(string Pattern)
		{
			System.Collections.IDictionaryEnumerator cacheContents = System.Web.HttpContext.Current.Cache.GetEnumerator();
			while (cacheContents.MoveNext())
			{
				if (cacheContents.Key.ToString().Contains(Pattern))
					System.Web.HttpContext.Current.Cache.Remove(cacheContents.Key.ToString());
			}
		}
	}
}
