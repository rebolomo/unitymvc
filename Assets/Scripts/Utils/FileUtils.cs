using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;

namespace UnityMVC.Utils
{
    // WebPlayer
    public class FileUtils
    {
        /** 
        * path 
        * name 
        *  info 
        */
        public static void CreateFile(string path, string name, string info)
        {
            //  
            StreamWriter sw;
            FileInfo t = new FileInfo(path + "//" + name);
            if (!t.Exists)
            {
                //  
                sw = t.CreateText();
            }
            else
            {
                //  
                sw = t.AppendText();
            }
            //  
            sw.WriteLine(info);
            //  
            sw.Close();
            //  
            sw.Dispose();
        }

        public static void CreateModelFile(string path, string name, byte[] info, int length)
        {
            //  
            //StreamWriter sw;  
            Stream sw;
            FileInfo t = new FileInfo(path + "//" + name);
            if (!t.Exists)
            {
                //  
                sw = t.Create();
            }
            else
            {
                //  
                //sw = t.Append();  
                return;
            }
            //  
            //sw.WriteLine(info);  
            sw.Write(info, 0, length);
            //  
            sw.Close();
            //  
            sw.Dispose();
        }

        /** 
         * path 
         * name 
         */
        public static void DeleteFile(string path, string name)
        {
            File.Delete(path + "//" + name);
        }

        /** 
           *  
           * path 
           * name 
           */
        public static ArrayList LoadFileByArray(string path, string name)
        {
            //  
            StreamReader sr = null;
            try
            {
                sr = File.OpenText(path + "//" + name);
            }
            catch (Exception e)
            {
                //  
				ClientLogger.Warn("warn\n"+e.Message);
                return null;
            }
            string line;
            ArrayList arrlist = new ArrayList();
            while ((line = sr.ReadLine()) != null)
            {
                //  
                //  
                arrlist.Add(line);
            }
            //  
            sr.Close();
            //  
            sr.Dispose();
            //  
            return arrlist;
        }

        /** 
       *  
       * path 
       * name 
       */
        public static string LoadFile(string path, string name)
        {
            //  
            StreamReader sr = null;
            try
            {
                string line = null;
                sr = File.OpenText(path + "//" + name);
                line = sr.ReadToEnd(); //
                //  
                sr.Close();
                //  
                sr.Dispose();
                return line;
            }
            catch (Exception e) //  
            {
#if UNITY_EDITOR
                ClientLogger.Info("==> LoadFile Error : " + e.Message);
#endif
                //  
                sr.Close();
                //  
                sr.Dispose();
                return null;
            }
        }

        //(
        public static string LoadFileByLine(string path, string name)
        {
            FileInfo t = new FileInfo(path + "//" + name);
            if (!t.Exists)
            {
                return "error";
            }
            StreamReader sr = null;
            sr = File.OpenText(path + "//" + name);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                break;
            }
            sr.Close();
            sr.Dispose();
            return line;
        }

        //(
        static void FindFiles(string path, string filter, out string[] files)
        {
            List<string> filelist = new List<string>();

            //(file://)
            DirectoryInfo t = new DirectoryInfo(path);
            if (!t.Exists)
            {
               ClientLogger.Info("! path = " + path);
                files = null;
                return;
            }

            //
            FileInfo[] f = t.GetFiles();
            if (f != null)
            {
                for (int i = 0; i < f.Length; i++)
                {
                    if (filter == null || filter == "")
                    {
                        filelist.Add(f[i].FullName);
                    }
                    else
                    {
                        //
                        string[] strs = f[i].FullName.Split(new char[] { '.' });
                        if (strs[strs.Length - 1].Equals(filter))
                        {
                            filelist.Add(f[i].FullName);
                        }
                    }
                }
            }

            DirectoryInfo[] d = t.GetDirectories();
            if (d != null)
            {
                //
                for (int i = 0; i < d.Length; i++)
                {
                    string[] fl;
                    FindFiles(d[i].FullName, filter, out fl);
                    if (fl != null)
                    {
                        for (int j = 0; j < fl.Length; j++)
                        {
                            if (filter == null || filter == "")
                            {
                                filelist.Add(f[i].FullName);
                            }
                            else
                            {
                                //
                                string[] strs = f[j].FullName.Split(new char[] { '.' });
                                if (strs[strs.Length - 1].Equals(filter))
                                {
                                    filelist.Add(fl[j]); //
                                }
                            }
                        }
                    }
                }
            }

            files = filelist.ToArray(); //
        }

        public static string[] FindFiles(string path, string filter)
        {
            string[] files;
            FindFiles(path, filter, out files);

            if (files != null)
            {
               ClientLogger.Info(" path = " + path);
                for (int i = 0; i < files.Length; i++)
                {
                   ClientLogger.Info("files[" + i + "]" + files[i]);
                }
            }
            else
            {
               ClientLogger.Info(" ! path = " + path);
            }
            return files;
        }

        public static bool isFileExist(string path)
        {
            return File.Exists(path);
        }

        public static string GetArtFileNames(string assetPath, int trimOff)
        {
           ClientLogger.Info("GetArtFileNames : assetPath = " + assetPath + ", trimOff = " + trimOff);
            string final = "";
            string[] aFilePaths = Directory.GetFiles(assetPath);
            if (aFilePaths.Length > 0)
            {
                for (int i = 0; i < aFilePaths.Length; ++i)
                {
                    string sAssetPath = aFilePaths[i].Remove(0, assetPath.Length);//sFilePath.Substring(sDataPath.Length - 6);
                    if (sAssetPath.Contains(".meta")) //Ignore meta files, don't include them in the bundle (unless you want to include them)
                        continue;
                    else
                        sAssetPath = sAssetPath.Remove(sAssetPath.Length - trimOff, trimOff); //Trim off the .png part

                    //Debug.Log(sFilePath);
                    if (i + 1 < aFilePaths.Length - 1)
                        final += sAssetPath + ", ";
                    else
                        final += sAssetPath;
                }
            }
            else
            {
               ClientLogger.Info("No files in path");
            }

            return final;
        }

        public static string GetFiles(string assetPath, int trimOff)
        {
           ClientLogger.Info("GetFiles : assetPath = " + assetPath + ", trimOff = " + trimOff);
            string final = "";
            string[] aFilePaths = Directory.GetFiles(assetPath);
            foreach (string sFilePath in aFilePaths)
            {
                string sAssetPath = sFilePath.Remove(0, assetPath.Length + 1);//sFilePath.Substring(sDataPath.Length - 6);
                if (sAssetPath.Contains(".meta")) //Ignore meta files, don't include them in the bundle (unless you want to include them)
                    continue;
                else
                    sAssetPath = sAssetPath.Remove(sAssetPath.Length - trimOff, trimOff); //Trim off the .png part
                //Debug.Log(sFilePath);
                final += '"' + sAssetPath + '"' + ", ";
            }
            return final;
        }

        //.meta
        public static string[] GetFiles(string assetPath, SearchOption option = SearchOption.TopDirectoryOnly)
        {
            //Debug.Log("GetFiles : assetPath = " + assetPath);
            if (Directory.Exists(assetPath))
            {
                string[] aFilePaths = Directory.GetFiles(assetPath, "*.*", option);
                List<string> finalResult = new List<string>();
                if (aFilePaths != null && aFilePaths.Length > 0)
                {
                    for (int i = 0; i < aFilePaths.Length; ++i)
                    {
                        if (!aFilePaths[i].Contains(".meta")) // (aFilePaths[i].Contains(".assetbundle") || aFilePaths[i].Contains(".unity3d")
                            finalResult.Add(aFilePaths[i]);
                    }
                }
                return finalResult.ToArray();
            }
            return null;
        }

        //.meta
        public static void GetAllFiles(string assetPath, out string[] filelist)
        {
            List<string> finalResult = new List<string>();
            DirectoryInfo t = new DirectoryInfo(assetPath);
            if (!t.Exists)
            {
               ClientLogger.Info("GetFiles : !");
                filelist = null;
                return;
            }

            //
            FileInfo[] aFilePaths = t.GetFiles();
            if (aFilePaths != null)
            {
                for (int i = 0; i < aFilePaths.Length; i++)
                {
                    if (!aFilePaths[i].Name.Contains(".meta") && (aFilePaths[i].Name.Contains(".assetbundle") || aFilePaths[i].Name.Contains(".unity3d")))
                    {
                        finalResult.Add(aFilePaths[i].FullName);
                        //Debug.Log("GetFiles : finalResult.Add " + aFilePaths[i].FullName);
                    }
                }
            }

            //
            DirectoryInfo[] aDirPaths = t.GetDirectories();
            if (aDirPaths != null)
            {
                for (int i = 0; i < aDirPaths.Length; i++)
                {
                    string[] fl;
                    GetAllFiles(aDirPaths[i].FullName, out fl);
                    if (fl.Length > 0)
                    {
                        for (int j = 0; j < fl.Length; j++)
                        {
                            finalResult.Add(fl[j]);
                            //Debug.Log("GetFiles : finalResult.Add " + fl[j]);
                        }
                    }
                }
            }

            filelist = finalResult.ToArray();
        }

        //
        public static string projectName
        {
            get
            {
                //shell  project-$1 
                // project - 
                // 91 
                foreach (string arg in System.Environment.GetCommandLineArgs())
                {
                    if (arg.StartsWith("project"))
                    {
                        return arg.Split("-"[0])[1];
                    }
                }
                return "";
            }
        }

        //
        public static void DeleteFolder(string dir)
        {
            //ClientLogger.Info("==> DeleteFolder......................" + dir);
            if (Directory.Exists(dir)) //
            {
                foreach (string d in Directory.GetFileSystemEntries(dir))
                {
                    if (File.Exists(d)) //
                    {
                        FileInfo f = new FileInfo(d);
                        if (f.Attributes.ToString().IndexOf("ReadOnly") != -1)
                            f.Attributes = FileAttributes.Normal;
                        //ClientLogger.Info("==> DeleteFolder2 : " + f.Name);
                        f.Delete(); //
                    }
                    else if (Directory.Exists(d)) //
                    {
                        DirectoryInfo d1 = new DirectoryInfo(d);
                        //ClientLogger.Info("==> DeleteFolder : " + d1.GetFiles().Length);
                        if (d1.GetFiles().Length != 0)
                        {
                            DeleteFolder(d1.FullName);//
                        }
                        //ClientLogger.Info("==> DeleteFolder1 : " + d1.FullName);
                        Directory.Delete(d1.FullName);
                    }
                }
            }
        }

        //
        public static void CopyDirectory(string abssourcePath, string absdestinationPath, string filter = "")
        {
            if (abssourcePath.EndsWith("/*")) //
            {
                abssourcePath = abssourcePath.Substring(0, abssourcePath.IndexOf("/*"));
                foreach (string dir in Directory.GetDirectories(abssourcePath)) //
                {
                    DirectoryInfo d = new DirectoryInfo(dir);
                    string destName = Path.Combine(absdestinationPath, d.Name);
                    //ClientLogger.Info("==> CopyDirectory * : dir = " + dir + ",destName = " + destName);
                    CopyDirectory(dir, destName);
                }
            }
            else
            {
                if (abssourcePath.EndsWith("/"))
                {
                    abssourcePath = abssourcePath.Substring(0, abssourcePath.Length - 1);
                }
                ClientLogger.Info("==> CopyDirectory : sourcePath = " + abssourcePath + ",destinationPath = " + absdestinationPath);
                DirectoryInfo info = new DirectoryInfo(abssourcePath);
                Directory.CreateDirectory(absdestinationPath);
                foreach (FileSystemInfo fsi in info.GetFileSystemInfos())
                {
                    if (!fsi.Name.Contains(".svn") && !fsi.Name.Contains(".meta")) //.svn .meta
                    {
                        string destName = Path.Combine(absdestinationPath, fsi.Name);
                        if (fsi is System.IO.DirectoryInfo)
                        {
                            Directory.CreateDirectory(destName);
                            CopyDirectory(fsi.FullName, destName);
                        }
                        else
                        {
                            if (filter == "" || fsi.FullName.Contains(filter))
                            {
                                File.Copy(fsi.FullName, destName, true);
                            }
                        }
                    }
                }
            }
        }

        //
        public static void CopyFileToFolder(string sourceFile, string destinationFolder, string filter ="")
        {
            //ClientLogger.Info("==> CopyFileToFolder : sourceFile = " + sourceFile + ",destinationFolder = " + destinationFolder);
            if (!Directory.Exists(destinationFolder))
            {
                Directory.CreateDirectory(destinationFolder);
            }
            FileInfo t = new FileInfo(sourceFile);
            string destFile = destinationFolder + t.Name;
            if (filter == "" || sourceFile.Contains(filter))
            {
                File.Copy(sourceFile, destFile, true);
            }
        }
    }
}
