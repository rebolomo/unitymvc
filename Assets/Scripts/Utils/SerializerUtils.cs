using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Text;
using System.Xml;
using System.Collections.Generic;
using UnityEngine;
using UnityMVC.Core.Model;
using MiniJSON;
using System.Reflection;

namespace UnityMVC.Utils
{
	public class SerializerUtils
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		public static void binarySerialize (String path, object data)
		{
			try {
				FileStream fileStream = new FileStream (path, FileMode.Create);
				IFormatter formatter = new BinaryFormatter ();
				formatter.Serialize (fileStream, data);
				fileStream.Close ();
			} catch (Exception ex) {
				ClientLogger.Error ("binarySerialize Error : " + ex.Message);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		public static object binaryDeserialize (byte[] bytes)
		{
			try {
				MemoryStream ms = new MemoryStream (bytes);
				BinaryFormatter formatter = new BinaryFormatter ();
				formatter.Binder = new UBinder ();
				object data = formatter.Deserialize (ms);
				ms.Close ();
				ms.Dispose ();
				return data;
			} catch (Exception ex) {
				ClientLogger.Error ("binaryDeserialize Error : " + ex.Message);
			}
			return null;
		}

//        /// <summary>
//        /// json
//        /// </summary>
//        /// <param name="bytes"></param>
//        /// <returns></returns>
//        public static object jsonDeserialize(byte[] bytes)
//        {
//            Type typeObj = null;
//            Dictionary<string, string> jsonTable = null;
//            Dictionary<string, object> objectTable = null;
//            
//            string output = Encoding.UTF8.GetString(bytes);
//            object objData = JsonConvert.DeserializeObject(output, typeof(Dictionary<string, Dictionary<string, string>>));
//            Dictionary<string, Dictionary<string, string>> data = (Dictionary<string, Dictionary<string, string>>)objData;
//            Dictionary<String, Dictionary<String, object>> releaseData = new Dictionary<string, Dictionary<string, object>>();
//            //int time = Environment.TickCount;
//
//            foreach (string key in data.Keys)
//            {
//                jsonTable = data[key];
//                typeObj = BaseDataMgr.instance.getClzType(key);
//                objectTable = new Dictionary<string, object>();
//
//                foreach (string keyId in jsonTable.Keys)
//                {
//                    objectTable.Add(keyId, JsonConvert.DeserializeObject(jsonTable[keyId], typeObj));
//                    //Debug.Log("SerializerUtils", jsonTable[keyId]);
//                }
//                releaseData.Add(key, objectTable);
//               ClientLogger.Info("SerializerUtils", "key:" + key + ",typeObj:" + typeObj.ToString());
//                //Debug.Log("", "\n");
//            }
//            //Debug.Log("SerializerUtils", "-jsonDeserialize() :" + (Environment.TickCount - time) + " ms");
//            return releaseData;
//        }

		/// <summary>
		/// Json2Class
		/// </summary>
		/// <returns>The json.</returns>
		/// <param name="filepath">Filepath.</param>
		/// <param name="type">Type.</param>
		public static Dictionary<string, object> json2class (string filepath, Type type)
		{
			//1.json
			string fileData = null;
			StreamReader reader = new StreamReader (filepath);
			fileData = reader.ReadToEnd ();
			reader.Close ();

			//2.
			Dictionary<string, object> jsonObjects = null;
			try {
				if(fileData != null){
					jsonObjects = MiniJSON.Json.Deserialize (fileData) as Dictionary<string, object>;
				}
			} catch (Exception ex) {
				ClientLogger.Error ("json2class Deserialize Error  : ex = " + ex.Message);
				return null;
			}

			//3.
			FieldInfo[] infos;
			string key = "-";
			Dictionary<string, object> keyValueDic = new Dictionary<string, object> ();
			if (jsonObjects != null) {
				foreach (KeyValuePair<string,object> tempNode in jsonObjects) {
					// type
					object generatedObject = System.Activator.CreateInstance (type);
					if (generatedObject == null) {
						const string temp = "type:{0} cannot be found!";
						#if UNITY_EDITOR
						ClientLogger.Error("type null:" + type);    
						#endif
						throw new NullReferenceException (string.Format (temp, type));
					}
					infos = generatedObject.GetType ().GetFields ();  //
					if (infos [0].Name.Equals ("unikey")) {
						key = (string)tempNode.Key;
					} else {
						ClientLogger.Error ("json2class Error :  no unikey !! filepath = " + filepath);
						continue;
					}
					foreach (FieldInfo info in infos) {
						try {
							Dictionary<string, object> tempDic = tempNode.Value as Dictionary<string, object>;
							object value = tempDic [info.Name].ToString ();
							if (info.FieldType.Equals (typeof(string))) {
								info.SetValue (generatedObject, value);
							} else if (info.FieldType.Equals (typeof(int)) || info.FieldType.Equals (typeof(long)) 
								|| info.FieldType.Equals (typeof(bool)) || info.FieldType.Equals (typeof(uint)) || info.FieldType.Equals (typeof(float))) {
								// int float bool long 
								System.Type t = info.FieldType;
								System.Reflection.MethodInfo method = t.GetMethod ("Parse", new Type[] { typeof(string) });
								//Public
								System.Reflection.BindingFlags flag = System.Reflection.BindingFlags.Public | BindingFlags.Static | System.Reflection.BindingFlags.Instance;
								if (value != null) {
									object[] parameters = new object[] { value };
									info.SetValue (generatedObject, method.Invoke (null, flag, Type.DefaultBinder, parameters, null));
								}
							}
						} catch (Exception ex) {
							ClientLogger.Error ("json2class Error :  key = " + key + " " + info.Name + "! error=" + ex.Message);
							break;
						}
					}
					if (key != "-")
						keyValueDic.Add (key, generatedObject);
				}
			}
			return keyValueDic;
		}

		/// <summary>
		/// XML
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		public static object xmlDeserialize (Type type, string text)
		{
			if (text == null)
				return null;
			try {
#if UNITY_EDITOR
                //Debug.Log("====> xmlDeserialize : type = " + type + " text = " + text);
#endif
				StringReader stringReader = new StringReader (text);
				XmlReader reader = XmlReader.Create (stringReader);
				XmlSerializer xs = new XmlSerializer (type);
				return xs.Deserialize (reader);
			} catch (Exception ex) {
				ClientLogger.Error ("warn : text = " + text + " error=" + ex.Message);
			}
			return null;
		}

		/// <summary>
		/// XML
		/// </summary>
		/// <param name="pObject"></param>
		/// <returns></returns>
		public static string xmlSerialize (object pObject, Type type)
		{
			try {
				string XmlizedString = null;
				MemoryStream memoryStream = new MemoryStream ();
				XmlSerializer xs = new XmlSerializer (type);
				XmlTextWriter xmlTextWriter = new XmlTextWriter (memoryStream, Encoding.UTF8);
				xs.Serialize (xmlTextWriter, pObject);
				memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
				XmlizedString = UTF8ByteArrayToString (memoryStream.ToArray ());
				return XmlizedString;
			} catch (Exception ex) {
				ClientLogger.Error ("warn\n" + ex.Message);
			}
			return null;
		}

//        /// <summary>
//        /// XML
//        /// </summary>
//        /// <param name="fileName"></param>
//        /// <param name="_infos"></param>
//        /// <returns></returns>
//        public static Dictionary<string, object> loadXML(string fileName, Type type)
//        {
//            int i = 0;
//            string _itemData = "";
//            string key = "";
//            try
//            {
//                TextAsset textAsset = Resources.Load("Data/BinData/" + fileName, typeof(TextAsset)) as TextAsset;
//                if (textAsset != null)
//                {
//#if UNITY_EDITOR
//                    //Debug.Log("====> loadXML : type = " + type + " fileName = " + fileName);
//#endif
//                    //XmlDocument
//                    //xml
//                    StringReader stringReader = new StringReader(textAsset.text);
//                    XmlReader reader = XmlReader.Create(stringReader);
//                    XmlDocument xmlDoc = new XmlDocument();
//                    string _data = stringReader.ReadToEnd();
//                    //
//                    _data.Replace("&", "&amp;");
//                    xmlDoc.LoadXml(_data);
//                    xmlDoc.Load(Application.dataPath + @"Resources/Data/BinData/" + fileName + ".xml");
//                    reader.Close();
//                    XmlNodeList nodeList = xmlDoc.SelectNodes("root/item");
//                    int numGoods = nodeList.Count;
//                    //XML
//                    if (numGoods > 0)
//                    {
//                        Dictionary<string, object> list = new Dictionary<string, object>();
//                        foreach (XmlNode node in nodeList)
//                        {
//                            _itemData = "<" + fileName + ">" + node.InnerXml + "</" + fileName + ">";
//                            object content = xmlDeserialize(type, _itemData); //
//                            list.Add(node.FirstChild.InnerText, content);
//                            i++;
//                        }
//                        return list;
//                    }
//                    
//                }
//            } catch (Exception ex)
//            {
//                ClientLogger.Error(" ==> loadXML failed : fileName = " + fileName + " _itemData=" + _itemData + " error=" + ex.Message);
//            }
//            return null;
//        }

		private static string UTF8ByteArrayToString (byte[] characters)
		{
			UTF8Encoding encoding = new UTF8Encoding ();
			string constructedString = encoding.GetString (characters);
			return (constructedString);
		}

		private static byte[] StringToUTF8ByteArray (string pXmlString)
		{
			UTF8Encoding encoding = new UTF8Encoding ();
			byte[] byteArray = encoding.GetBytes (pXmlString);
			return byteArray;
		}
	}
}
