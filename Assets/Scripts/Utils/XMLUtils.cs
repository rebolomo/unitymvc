using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Xml;
using UnityEngine;

namespace UnityMVC.Utils
{
    class XMLUtils
    {
        public static void writeData2XML(string path, Dictionary<string, Dictionary<string, object>> datas)
        {
            
        }


        /// <summary>
        /// 解析XML生成对象  ,返回 unikey ， object的字典
        /// </summary>
        /// <param name="file">xml文件路径</param>
        /// <param name="type">生成对象的类名</param>
        public static Dictionary<string, object> ReadXMLToObject(string file, Type type)
        {
			Dictionary<string, object> keyValueDic = new Dictionary<string, object>();
            FieldInfo[] infos;
            //int a = null;
            //读取xml文件
            string fileData = "";
            StreamReader reader = new StreamReader(file);
            fileData = reader.ReadToEnd();
            reader.Close();

            //XML解析
            XMLNode node = XMLParser.Parse(fileData);
            XMLNodeList xmlObjects = node.GetNodeList("root>0>item");
            string value = "";
            string key = "-";

            foreach (XMLNode tempNode in xmlObjects)
            {
                //创建对象 ，type为类名，不包含命名空间，写死了
                object generatedObject = System.Activator.CreateInstance(type);
                if (generatedObject == null)
                {
                    const string temp = "type:{0} cannot be found!";
#if UNITY_EDITOR
                    ClientLogger.Error("type null:" + type);    
#endif
                    throw new NullReferenceException(string.Format(temp, type));
                }
                infos = generatedObject.GetType().GetFields();  //获取属性
                foreach (FieldInfo info in infos)
                {
                    try
                    {
                        value = tempNode.GetValue(info.Name.ToString() + ">0>_text");
                        if (info.Name.ToString().Equals("unikey"))
                            key = value;
                        if (info.FieldType.Equals(typeof(string)))
                        {
                            info.SetValue(generatedObject, value);
                        }
                        else if (info.FieldType.Equals(typeof(int)) || info.FieldType.Equals(typeof(float)) || info.FieldType.Equals(typeof(long)) || info.FieldType.Equals(typeof(bool)) || info.FieldType.Equals(typeof(uint)))
                        {   
                            //字符串转 int float bool long 类型的值
                            System.Type t = info.FieldType;
                            System.Reflection.MethodInfo method = t.GetMethod("Parse", new Type[] { typeof(string) });
                            //调用方法的一些标志位，这里的含义是Public并且是实例方法，这也是默认的值
                            System.Reflection.BindingFlags flag = System.Reflection.BindingFlags.Public | BindingFlags.Static | System.Reflection.BindingFlags.Instance;
                            if (value != null)
                            {
                                object[] parameters = new object[] { value };
                                info.SetValue(generatedObject, method.Invoke(null, flag, Type.DefaultBinder, parameters, null));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ClientLogger.Error("XMLUtil Error :  key = " + key + " 解析字段" + info.Name + "出错! error=" + ex.Message);
                    }
                }
                if (key != "-")
                    keyValueDic.Add(key, generatedObject);
            }
            return keyValueDic;
        }
    }
}
