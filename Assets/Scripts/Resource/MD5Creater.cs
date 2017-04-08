using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.IO;

namespace UnityMVC.Resource
{
    public class MD5Creater
    {
        public static string GenerateMd5Code(string filePathName)
        {
            MD5CryptoServiceProvider md5Generator = new MD5CryptoServiceProvider();
            FileStream file = new FileStream(filePathName, FileMode.Open, FileAccess.Read, FileShare.Read);
            byte[] md5CodeBytes = md5Generator.ComputeHash(file);
            string strMD5Code = System.BitConverter.ToString(md5CodeBytes);
            file.Close();
            return strMD5Code;
        }
    }
}
