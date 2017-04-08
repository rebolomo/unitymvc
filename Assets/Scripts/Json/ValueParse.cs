using System;
using System.Collections;
using System.Reflection;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace UnityMVC.Net
{
	public static class ValueParse
	{
		/// <summary>
		/// int32
		/// </summary>
		/// <returns><c>true</c>, , <c>false</c> .</returns>
		/// <param name="idic">.</param>
		/// <param name="Key">.</param>
		/// <param name="intvalue">int32.</param>
		public static bool GetIntValue (IDictionary idic, string Key, out int intvalue)
		{
			intvalue = 0;
			if (idic.Contains (Key)) {
				intvalue = int.Parse (idic [Key].ToString ());
				return true;
			}
			return false;
		}
		/// <summary>
		/// int32
		/// </summary>
		/// <returns>int32.</returns>
		/// <param name="idic">.</param>
		/// <param name="Key">.</param>
		public static int GetIntValue (IDictionary idic, string Key)
		{
			return int.Parse (idic [Key].ToString ());
		}
		/// <summary>
		/// bool.
		/// </summary>
		/// <returns><c>true</c>, , <c>false</c> .</returns>
		/// <param name="idic">.</param>
		/// <param name="Key">.</param>
		/// <param name="boolvalue">bool.</param>
		public static bool GetBoolValue (IDictionary idic, string Key, out bool boolvalue)
		{
			boolvalue = false;
			if (idic.Contains (Key)) {
				boolvalue = bool.Parse (idic [Key].ToString ());
				return true;
			}
			return false;
		}
		/// <summary>
		/// bool.
		/// </summary>
		/// <returns>bool.</returns>
		/// <param name="idic">.</param>
		/// <param name="Key">.</param>
		public static bool GetBoolValue (IDictionary idic, string Key)
		{
			return bool.Parse (idic [Key].ToString ());
		}
		/// <summary>
		/// string.
		/// </summary>
		/// <returns><c>true</c>, , <c>false</c> .</returns>
		/// <param name="idic">.</param>
		/// <param name="Key">.</param>
		/// <param name="stringvalue">string.</param>
		public static bool GetStringValue (IDictionary idic, string Key, out string stringvalue)
		{
			stringvalue = string.Empty;
			if (idic.Contains (Key)) {
				stringvalue = idic [Key].ToString ();
				return true;
			}
			return false;
		}
		/// <summary>
		/// string.
		/// </summary>
		/// <returns>string.</returns>
		/// <param name="idic">.</param>
		/// <param name="Key">.</param>
		public static string GetStringValue (IDictionary idic, string Key)
		{
			return (string)idic [Key];
		}
		/// <summary>
		/// int64.
		/// </summary>
		/// <returns><c>true</c>, , <c>false</c> .</returns>
		/// <param name="idic">.</param>
		/// <param name="Key">.</param>
		/// <param name="longvalue">int64.</param>
		public static bool GetInt64Value (IDictionary idic, string Key, out long longvalue)
		{
			longvalue = 0;
			if (idic.Contains (Key)) {
				longvalue = long.Parse (idic [Key].ToString ());
				return true;
			}
			return false;
		}
		/// <summary>
		/// int64.
		/// </summary>
		/// <returns>int64.</returns>
		/// <param name="idic">.</param>
		/// <param name="Key">.</param>
		public static long GetInt64Value (IDictionary idic, string Key)
		{
			return long.Parse (idic [Key].ToString ());
		}
		/// <summary>
		/// url.
		/// </summary>
		/// <returns>url.</returns>
		/// <param name="baseUrl">url.</param>
		/// <param name="paramlist">.</param>
		public static string UrlParse (string baseUrl, IDictionary paramlist)
		{
			string finalUrl = string.Empty;
			IList keyList = new ArrayList (paramlist.Keys);
			IList valueList = new ArrayList (paramlist.Values);
			
			if (keyList.Count == 0) {
				return baseUrl;
			}
			
			for (int i = 0; i < paramlist.Count; i++) {
				if (i == 0) {
					finalUrl = string.Format ("{0}?", baseUrl);
				} else {
					finalUrl = string.Format ("{0}&", finalUrl);
				}
				finalUrl = string.Format ("{0}{1}={2}", finalUrl, keyList [i].ToString (), valueList [i].ToString ());
			}
			return finalUrl;
		}

		//	REBOL note, 
		private static bool isList (Type type)
		{
			if (type.IsGenericType && type.GetGenericTypeDefinition () == typeof(List<>)) {
				return true;
			} else {
				return false;
			}
		}

		private static bool isDict (Type type)
		{
			bool r = type.IsGenericType && type.GetGenericTypeDefinition () == typeof(Dictionary<,>);
			//Type keyType = type.GetGenericArguments()[0];
			//Type valueType = type.GetGenericArguments()[1];
			//Debug.Log("Key type: " + keyType.ToString());
			//Debug.Log("Value type: " + valueType.ToString());
			return r;
		}

		/// <summary>
		/// Parses the json to class.
		/// </summary>
		/// <returns>The json to class.</returns>
		/// <param name="baseType">Base type.</param>
		/// <param name="baseValue">Base value.</param>
		public static object ParseJsonToClass (Type baseType, object baseValue)
		{
			if (baseValue == null)
				return null;
//#if UNITY_EDITOR
//			Debug.Log("ParseJsonToClass : " +baseType);
//#endif
			#region parse array
			//	REBOL add, list collection
			if (isList (baseType)) {
				IList jsonList = (IList)baseValue;               
				IList returnObj = (IList)Activator.CreateInstance (baseType);
				//	List<X> X type
				Type myListElementType = baseType.GetGenericArguments () [0];
				for (int i = 0; i < jsonList.Count; i++) {
					object obj = ParseJsonToClass (myListElementType, jsonList [i]);
					returnObj.Add (obj);
				}
				return returnObj;
			} else if (baseType.IsArray) {
				IList jsonList = (IList)baseValue;
				Array returnObj = (Array)Activator.CreateInstance (baseType, jsonList.Count);
				for (int i = 0; i < returnObj.Length; i++) {
					returnObj.SetValue (ParseJsonToClass (baseType.GetElementType (), jsonList [i]), i);
				}
				return returnObj;
			}
			#endregion
			#region parse class
			else if (baseType.Namespace.StartsWith ("XYClient")) {
				object returnObj = Activator.CreateInstance (baseType);
				PropertyInfo[] pis = baseType.GetProperties (BindingFlags.Public | BindingFlags.Instance);
				IDictionary keyPairs = (IDictionary)baseValue;
				string piName = null;
				foreach (PropertyInfo pi in pis) {
					piName = pi.Name;
					if (!keyPairs.Contains (piName)) {
						if (!(pi.Name.Equals ("New"))) {
							continue;
						} else {
							piName = "new";
						}
					}
					if (keyPairs [piName] == null) {
						if (!(pi.Name.Equals ("New"))) {
							continue;
						} else {
							piName = "new";
						}
					}
					pi.SetValue (returnObj, ParseJsonToClass (pi.PropertyType, keyPairs [piName]), null);
				}
				return returnObj;
			}
			#endregion
			#region parse basic values
			else if (baseType.IsValueType || baseType.Equals (typeof(string))) {
				if (baseType.Equals (typeof(short))) {
					return Convert.ToInt16 (baseValue);
				} else if (baseType.Equals (typeof(int))) {
					return Convert.ToInt32 (baseValue);
				} else if (baseType.Equals (typeof(long))) {
					return Convert.ToInt64 (baseValue);
				} else if (baseType.Equals (typeof(float))) {
					return Convert.ToSingle (baseValue);
				} else if (baseType.Equals (typeof(double))) {
					return Convert.ToDouble (baseValue);
				} else if (baseType.Equals (typeof(bool))) {
					return Convert.ToBoolean (baseValue);
				} else if (baseType.Equals (typeof(byte))) {
					return Convert.ToByte (baseValue);
				} else if (baseType.Equals (typeof(string))) {
					return baseValue.ToString ();
				}
			} else if (baseType.Equals (typeof(ArrayList))) {
				IList jsonList = (IList)baseValue;
				if (jsonList == null || jsonList.Count == 0) {
					return new ArrayList ();
				} else {
					return new ArrayList (jsonList);
				}
			} else if (baseType.Equals (typeof(DictionaryBase))) {
				Dictionary<string, object> jsonList = (Dictionary<string, object>)baseValue;
				if (jsonList == null || jsonList.Count == 0) {
					return new Dictionary<string, object> ();
				} else {
					return new Dictionary<string, object> (jsonList);
				}
			} else if (isDict (baseType)) {	//	REBOL note, IDictionary collection.
				Dictionary<string, object> jsonList = (Dictionary<string, object>)baseValue;
				IDictionary returnObj = (IDictionary)Activator.CreateInstance (baseType);
				//	Dict<string, X> X type
				Type myDictElementType = baseType.GetGenericArguments () [1];
				for (int i = 0; i < jsonList.Keys.Count; i++) {
					string key = jsonList.Keys.ElementAt (i);
					object obj = ParseJsonToClass (myDictElementType, jsonList [key]);
					returnObj.Add (key, obj);
				}
				return returnObj;
			}
			#endregion
			return null;
		}

		/// <summary>
		/// Parses the class to json.
		/// </summary>
		/// <returns>The class to json.</returns>
		/// <param name="bValue">B value.</param>
		public static object ParseClassToJson (object bValue)
		{
//#if UNITY_EDITOR
//			Debug.Log("ParseClassToJson : " +bValue);
//#endif
			if (bValue == null) {
				return null;
			}
			Type baseObjectType = bValue.GetType ();
			if (baseObjectType.IsArray) {
				if (((Array)bValue).Length == 0) {
					return null;
				}
				IList returunList = new ArrayList ();
				for (int i = 0; i < ((Array)bValue).Length; i++) {
					returunList.Add (ParseClassToJson (((Array)bValue).GetValue (i)));
				}
				return returunList;
			} else if (baseObjectType.Namespace.StartsWith ("XYClient")) {
				IDictionary returnObject = new Hashtable ();
				PropertyInfo[] pis = baseObjectType.GetProperties (BindingFlags.Public | BindingFlags.Instance);
				
				foreach (PropertyInfo pi in pis) {
					returnObject.Add (pi.Name, (ParseClassToJson (pi.GetValue (bValue, null))));
				}
				return returnObject;
			} else if (baseObjectType.IsValueType || 
				baseObjectType.Equals (typeof(string)) ||
				baseObjectType.Equals (typeof(ArrayList)) ||
				baseObjectType.Equals (typeof(BitArray)) ||
				baseObjectType.Equals (typeof(DictionaryBase))) {
				return bValue;
			}
			return null;
		}
	}
}
