////////////////////////////////////////////////////
//// File Name :        Monster.cs
//// Tables :              nothing
//// Autor :               rebolomo
//// Create Date :    	2015.8.24
//// Content :           
////////////////////////////////////////////////////
namespace UnityMVC.Utils
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    public class StringUtils
    {
        //string.Format("{0:D3}",23)   023  string.Format("{0:000.00}",023.00);
        //string.Format("{0:N3}",14200)  14,200.00
        //string.Format("{0:P2}",0.2456)   24.56%
        /**
         * @return true:,false:
         * **/

        public static bool isEmpty(String param)
        {
            return param != null && param.Trim().Length > 0 ? false : true;
        }

        /**
         * @return true:,false:
         * **/

        public static bool isEquals(String param1, String param2)
        {
            if (param1 == null || param2 == null) return false;
            return param1.Equals(param2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="total"></param>
        /// <returns></returns>
        public static string formatCurrency(int total)
        {
            string str = total.ToString();
            if (total >= 100000000)
            {
                int n = total / 10000;
                str = String.Format("{0}", n);
            }
            else if (total >= 10000)
            {
                int n = total / 10000;
                str = String.Format("{0}", n);
            }
            //
            return str;
        }

        /// <summary>  
        /// ()  
        /// </summary>  
        /// <param name="s"></param>  
        /// <param name="precision"></param>  
        /// <param name="scale"></param>  
        /// <returns></returns>  
        public static bool IsNumber(string s, int precision, int scale)
        {
            if ((precision == 0) && (scale == 0))
            {
                return false;
            }
            string pattern = @"(^\d{1," + precision + "}";
            if (scale > 0)
            {
                pattern += @"\.\d{0," + scale + "}$)|" + pattern;
            }
            pattern += "$)";
            return Regex.IsMatch(s, pattern);
        }

        //
        public static string GetNoSuffixString(string str)
        {
            string result = str.Remove(str.LastIndexOf("."));
            return result;
        }

        //
        public static string GetRemoveStrString(List<string> str, string cha = ":")
        {
            string returnStr = null;
            for (int i = 0; i < str.Count; i++)
            {
                for (int j = 0; j < str[i].Split(':').Length; j++)
                {
                    returnStr = returnStr + str[i].Split(':')[j];
                }
            }
            return returnStr;
        }

        // Splits the vo string.
        public static string[] SplitVoString(string str, string delimiter = ",")
        {
            if (CheckValid(str))
            {
                //ClientLogger.Info("==> SplitVoString : str = " + str);
                string[] resultArr;
                str = str.Replace(" ", ""); //
                str = str.TrimStart('[');
                str = str.TrimEnd(']');
                str = str.Replace(delimiter, ":");
                resultArr = str.Split(':');
                //resultArr = Regex.Split(str,delimiter);
                return resultArr;
            }
            return null;
        }

        // '['']'
        public static string GetValueString(string str)
        {
            if (CheckValid(str))
            {
                str = str.Replace(" ", ""); //
                str = str.TrimStart('[');
                str = str.TrimEnd(']');
                //resultArr = Regex.Split(str,delimiter);
                return str;
            }
            return null;
        }

        //[[[100,300],4000],[[301,500],4000],[[501,600],1500],[[601,700],500]]
        public static void GetAttrRange(string str, out int attrMin, out int attrMax)
        {
            str = StringUtils.GetValueString(str); //[[100,200],7000],[[201,300],3000],...
            string[] strDescribe1;
            strDescribe1 = StringUtils.SplitVoString(str, "],["); //[100,200],7000 [201,300],3000 ...

            attrMin = 0;
            attrMax = 0;
            int attrLow, attrHigh;
            string[] strDescribe2;

            strDescribe2 = strDescribe1[0].Split(',');
            attrLow = int.Parse(strDescribe2[0].TrimStart('['));
            attrHigh = int.Parse(strDescribe2[1].TrimEnd(']'));
            attrMin = attrLow;
            attrMax = attrHigh;
            for (int i = 1; i < strDescribe1.Length; ++i)
            {
                strDescribe2 = strDescribe1[i].Split(',');
                attrLow = int.Parse(strDescribe2[0].TrimStart('['));
                attrHigh = int.Parse(strDescribe2[1].TrimEnd(']'));
                attrMin = attrMin < attrLow ? attrMin : attrLow;
                attrMax = attrMax > attrHigh ? attrMax : attrHigh;
            }
        }

        //
        public static bool IsValidConfigParam(string param)
        {
            return (null != param) && (param.Trim().Length > 0) && ("0" != param);
        }

        //[[5,480,10],[22,96,90]](5,480,10) (22,96,10)
        //[[5,480],[22,96]](5,480) (22,96)
        //[[5,480]](5,480)
        public static string[] GetStringVo(string str)
        {
            if (CheckValid(str))
            {
                string[] result;
                result = StringUtils.SplitVoString(str, "],");
                for (int i = 0; i < result.Length; ++i)
                {
                    result[i] = result[i].TrimStart('[');
                    result[i] = result[i].TrimEnd(']');
                }
                return result;
            }
            return null;
        }

        //{5,480,10},{22,96,90}(5,480,10) (22,96,10)
        //{...}
        public static string[] GetWaveString(string str)
        {
            if (CheckValid(str))
            {
                string[] result;
                result = StringUtils.SplitVoString(str, "},");
                //string log_str = "==>GetStringVo2 : ";
                for (int i = 0; i < result.Length; ++i)
                {
                    result[i] = result[i].TrimStart('{');
                    result[i] = result[i].TrimEnd('}');
                    // log_str += result[i];
                }
                //ClientLogger.Info(log_str);
                return result;
            }
            return null;
        }

        //[XXX,XXX,...]
        public static string[] GetStringSubVo(string str, char separator = ',')
        {
            if (CheckValid(str))
            {
                string[] resultArr;
                str = str.TrimStart('[');
                str = str.TrimEnd(']');
                str = str.Replace(" ", ""); //
                resultArr = str.Split(separator);
                return resultArr;
            }
            return null;
        }

        //{XXX,XXX,...}
        public static string[] GetStringSubVo2(string str, char separator = ',')
        {
            if (CheckValid(str))
            {
                string[] resultArr;
                str = str.TrimStart('{');
                str = str.TrimEnd('}');
                str = str.Replace(" ", ""); //
                resultArr = str.Split(separator);
                return resultArr;
            }
            return null;
        }


        //[{0, [100001], 1},  {5, [100002], 1}, {3, [100003], 2}, {4, [100004], 1}, {0, [100005], 3}]
        public static string[] GetMonsterList(string str)
        {
            if (CheckValid(str))
            {
                string[] resultArr;
                str = str.Replace("[", ""); //
                str = str.Replace("]", ""); //
                str = Regex.Replace(str, @"\{[0-9]+,", "");
                str = Regex.Replace(str, @"[0-9]+\},", "");
                str = Regex.Replace(str, @"[0-9]+\}", "");
                //
                str = str.Substring(0, str.Length - 2);
                str = str.Replace(" ", "");
                resultArr = str.Split(',');
                return resultArr;
            }
            return null;
        }

        /// <summary>
        /// [[1,2],[3,4]] int[]
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int[] GetArrayStringToInt(string str)
        {
            if (CheckValid(str))
            {
                string pStr = str.Substring(1, str.Length - 2);
                pStr = pStr.Replace("[", "");
                pStr = pStr.Replace("]", "");
                string[] proS = pStr.Split(',');
                if (proS.Count() > 0)
                {
                    int[] result = new int[proS.Count()];
                    for (int i = 0; i < proS.Count(); i++)
                    {
                        result[i] = int.Parse(proS[i]);
                    }
                    return result;
                }
                return new int[0];
            }
            return null;
        }

        //
        public static int GetCharLength(string str)
        {
            int length = 0;

            for (int i = 0; i < str.Length; i++)
            {
                if (Char.ConvertToUtf32(str, i) >= Convert.ToInt32("4e00", 16)
                    && Char.ConvertToUtf32(str, i) <= Convert.ToInt32("9fff", 16))
                {
                    length += 2;
                }
                else
                {
                    length += 1;
                }
            }

            return length;
        }

        //--------------------------------------------------------------------//
        //{XXX,XXX,...}
        public static int[] GetStringValueListToInt(string str, char separator = ',')
        {
            if (CheckValid(str))
            {
                string[] resultArr;
                str = str.TrimStart('{');
                str = str.TrimEnd('}');
                str = str.Replace(" ", ""); //
                resultArr = str.Split(separator);
                int[] resultArr2 = new int[resultArr.Length];
                for (int i = 0; i < resultArr2.Length; i++)
                {
                    resultArr2[i] = resultArr[i] != null ? int.Parse(resultArr[i]) : 0; //
                }
                return resultArr2;
            }
            return null;
        }

        /// <summary>
        /// 1,2,3 int[]
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int[] GetStringToInt(string str, char separator = ',') //, int count = -1
        {
            if (CheckValid(str))
            {
#if UNITY_EDITOR //&& DEBUG_STRINGUTIL
                //ClientLogger.Info("==> GetStringToInt : " + str + " separator = " + separator);
#endif
                string[] proS = str.Split(separator);
                int[] resultArr = new int[proS.Length];
                for (int i = 0; i < resultArr.Length; i++)
                {
                    resultArr[i] = proS[i] != null ? int.Parse(proS[i]) : 0; //
                }
                return resultArr;
            }
            return null;
        }

		/// <summary>
		/// 1,2,3 int[]
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static int[] GetStringToInt(string str, char separator, char separator1) //, int count = -1
		{
			if (CheckValid(str))
			{
				#if UNITY_EDITOR //&& DEBUG_STRINGUTIL
				//ClientLogger.Info("==> GetStringToInt : " + str + " separator = " + separator);
				#endif
				string[] proS = str.Split(separator, separator1);
				int[] resultArr = new int[proS.Length];
				for (int i = 0; i < resultArr.Length; i++)
				{
					resultArr[i] = proS[i] != null ? int.Parse(proS[i]) : 0; //
				}
				return resultArr;
			}
			return null;
		}

        /// <summary>
        /// 1,2,3 float[]
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static float[] GetStringToFloat(string str, char separator = ',')
        {
            if (CheckValid(str))
            {
                string[] proS = str.Split(separator);
                //ClientLogger.Info("==>GetStringToFloat : " + str + " separator = " + separator + " proS.Length " + proS.Length);
                float[] resultArr = new float[proS.Length];
                for (int i = 0; i < resultArr.Length; i++)
                {
                    resultArr[i] = proS[i] != null ? float.Parse(proS[i]) : 0; //
                }
                //ClientLogger.Info("==> GetStringToFloat : " + str + ", resultArr.Length = " + resultArr.Length);
                return resultArr;
            }
            return null;
        }

        /// <summary>
        /// 1,2,3 str[]
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string[] GetStringValue(string str, char separator = ',') //, int count = -1
        {
            if (CheckValid(str))
            {
                //ClientLogger.Info("==> GetStringValue : " + str);
                string[] resultArr1 = null;
                str = str.Replace(" ", ""); //
                resultArr1 = str.Split(separator);
                //int Length = resultArr1 != null ? resultArr1.Length : 0;
                //ClientLogger.Info("==> GetStringValue : resultArr.Length = " + Length);
                return resultArr1;
            }
            return null;
        }

        /// <summary>
        /// 1,2,3 str[], 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string[] GetStringValues(string str, int index, char separator1 = ',', char separator2 = '|')
        {
            if (CheckValid(str))
            {
                string[] resultArr1, resultArr2;
                str = str.Replace(" ", ""); //
                resultArr1 = str.Split(separator1); //
                List<string> strs = new List<string>();
                //string log_str = "==> GetStringValues : ";
                if (resultArr1 != null && resultArr1[0] != "")
                {
                    for (int i = 0; i < resultArr1.Length; i++)
                    {
                        resultArr2 = resultArr1[i].Split(separator2); //
                        if (resultArr2 != null && index < resultArr2.Length) //index
                        {
                            strs.Add(resultArr2[index]);
                            //log_str += resultArr2[index];
                        }
                    }
                }
                //ClientLogger.Info(log_str);
                return strs.ToArray();
            }
            return null;
        }

        /// <summary>
        /// 1,2,3 str[]
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int[] GetStringValuesToInt(string str, int index, char separator1 = ',', char separator2 = '|')
        {
            if (CheckValid(str))
            {
                string[] resultArr1;
                int[] resultArr2;
                str = str.Replace(" ", ""); //
                resultArr1 = str.Split(separator1); //
                List<int> strs = new List<int>();
                if (resultArr1 != null && resultArr1[0] != "")
                {
                    for (int i = 0; i < resultArr1.Length; i++)
                    {
                        resultArr2 = GetStringToInt(resultArr1[i], separator2);
                        if (resultArr2 != null && index < resultArr2.Length) //index
                        {
                            strs.Add(resultArr2[index]);
                        }
                    }
                }
                //ClientLogger.Info("==> GetStringValuesToInt : " + str + ", resultArr.Length = " + strs.Count);
                return strs.ToArray();
            }
            return null;
        }

        /// <summary>
        /// 1,2,3 str[]
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static float[] GetStringValuesToFloat(string str, int index, char separator1 = ',', char separator2 = '|')
        {
            if (CheckValid(str))
            {
                string[] resultArr1;
                float[] resultArr2;
                str = str.Replace(" ", ""); //
                resultArr1 = str.Split(separator1); //
                List<float> strs = new List<float>();
                if (resultArr1 != null && resultArr1[0] != "")
                {
                    for (int i = 0; i < resultArr1.Length; i++)
                    {
                        resultArr2 = GetStringToFloat(resultArr1[i], separator2);
                        if (resultArr2 != null && index < resultArr2.Length) //index
                        {
                            strs.Add(resultArr2[index]);
                        }
                    }
                }
                //ClientLogger.Info("==> GetStringValuesToFloat : " + str + ", resultArr.Length = " + strs.Count);
                return strs.ToArray();
            }
            return null;
        }

        public static string FormatTimeInMinutes(int seconds, bool positive = true)
        {
            if (seconds < 0 && positive)
                seconds = 0;
            var hour = seconds / 3600;
            seconds = seconds % 3600;
            if (hour == 0)
                return string.Format("{0:D2}:{1:D2}", seconds / 60, seconds % 60);
            else
                return string.Format("{2}:{0:D2}:{1:D2}", seconds / 60, seconds % 60, hour);
        }

        public static string FormatNumberPlus(int plus)
        {
            if (plus <= 0)
                return "";
            return "+" + plus;
        }

        public static string FormatNumber(int number)
        {
            //{0:N2}
            return string.Format("{0:N0}", number);
        }

        public static string DumpObject(object o)
        {
            if (o == null) return null;
            var type = o.GetType();
            try
            {
                var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                var props = type.GetProperties(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public |
                                               BindingFlags.DeclaredOnly | BindingFlags.GetProperty);
                return fields.Aggregate("", (current, fd) => current + (fd.Name + ": " + fd.GetValue(o) + "; ")) +
                       props.Aggregate("", (current, fd) =>
                       {
                           var method = fd.GetGetMethod();
                           if (method == null) return current;
                           return current + (fd.Name + ": " + method.Invoke(o, new object[0]) + "; ");
                       });
            }
            catch (Exception)
            {
                return type.ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetTailNoEnterStr(String str)
        {
            if (str == null || str.Split('\n').Length <= 1) return str;
            List<string> strList = str.Split('\n').ToList();
            string recordStr = null;
            for (int i = 0; i < strList.Count; i++)
            {
                if (strList[i] == "" || strList[i] == null || strList[i] == " ")
                {
                    strList.Remove(strList[i]);
                }
            }
            for (int i = 0; i < strList.Count; i++)
            {
                recordStr += strList[i] + (i == (strList.Count - 1) ? "" : "\n");
            }
            return recordStr;
        }

        #region 

        //
        public static string ReturnMinute(int number)
        {
            string str = null;
            str = string.Format("{0}{1}{2}", ((number % 300) / 60).ToString(), ":",
                ((number % 300) % 60) < 10 ? "0" + ((number % 300) % 60).ToString() : ((number % 300) % 60).ToString());
            return str;
        }

        #endregion

        #region 

        /// <summary>
        /// Checks the id valid.
        /// </summary>
        /// <returns><c>true</c>, if valid was checked, <c>false</c> otherwise.</returns>
        /// <param name="str">String.</param>
        public static bool CheckValid(int id)
        {
            if (id == null || id <= 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Checks the str valid.
        /// </summary>
        /// <returns><c>true</c>, if valid was checked, <c>false</c> otherwise.</returns>
        /// <param name="str">String.</param>
        public static bool CheckValid(string str)
        {
			if (str == null || str.Equals("[]") || str.Equals("") || str.Equals("Null") || str.Equals("null")) //|| str.Equals("0")
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Checks the strs valid.
        /// </summary>
        /// <returns><c>true</c>, if valid was checked, <c>false</c> otherwise.</returns>
        /// <param name="str">String.</param>
        public static bool CheckValid(string[] str)
        {
            if (str == null || str.Length == 0)
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}
